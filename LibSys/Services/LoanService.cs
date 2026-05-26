using Microsoft.EntityFrameworkCore;
using LibSys.Models;
using LibSys.Data;

namespace LibSys.Services
{
    public interface ILoanService
    {
        Task<OperationResult> IssueBookAsync(int readerId, int bookCopyId);
        Task<OperationResult> ReturnBookAsync(int loanId, int newState);
        Task<OperationResult> ReportLossAsync(int loanId);
        Task<OperationResult> PayDebtAsync(int loanId);
        Task<int> GetActiveLoansCountAsync(int readerId);
        Task<bool> HasUnpaidDebtsAsync(int readerId);
        Task<OperationResult> WriteOffCopyAsync(int copyId);
        Task MarkZeroDebtLoansAsPaidAsync(int readerId);
        Task UpdateOverdueLoansForReaderAsync(int readerId);
        Task UpdateAllOverdueLoansAsync();
    }

    public class LoanService : ILoanService
    {
        public async Task UpdateOverdueLoansForReaderAsync(int readerId)
        {
            using var db = new LibraryContext();
            var overdueLoans = await db.Loans
                .Where(l => l.ReaderId == readerId && l.ActualReturnDate == null && !l.IsLost && l.ExpectedReturnDate < DateTime.Now.Date && l.IsLossPaid)
                .ToListAsync();
            foreach (var loan in overdueLoans)
            {
                loan.IsLossPaid = false;
            }
            await db.SaveChangesAsync();
        }

        public async Task<OperationResult> IssueBookAsync(int readerId, int bookCopyId)
        {
            await UpdateOverdueLoansForReaderAsync(readerId);

            using var db = new LibraryContext();
            var settings = await db.SystemSettings.FirstOrDefaultAsync();
            if (settings == null) return OperationResult.Failure("Системні налаштування не знайдено.");

            var reader = await db.Readers.FindAsync(readerId);
            var bookCopy = await db.BookCopies
                .Include(bc => bc.Book)
                .FirstOrDefaultAsync(bc => bc.Id == bookCopyId);

            if (reader == null || bookCopy == null)
                return OperationResult.Failure("Читача або примірник не знайдено.");

            var activeLoans = await db.Loans
                .Where(l => l.ReaderId == readerId && l.ActualReturnDate == null)
                .ToListAsync();

            if (activeLoans.Count >= settings.MaxBooksPerReader)
                return OperationResult.Failure($"Досягнуто ліміт: у читача вже є {settings.MaxBooksPerReader} книги на руках.");

            if (activeLoans.Any(l => l.ExpectedReturnDate < DateTime.Now))
                return OperationResult.Failure("Заборона видачі: є прострочені та неповернуті книги.");

            if (await db.Loans.AnyAsync(l => l.ReaderId == readerId && !l.IsLossPaid))
                return OperationResult.Failure("Заборона видачі: наявна неоплачена заборгованість.");

            var banResult = await CheckTemporaryBanAsync(db, readerId, bookCopy.BookId, settings);
            if (!banResult.IsSuccess) return banResult;

            int loanDays = CalculateLoanDays(bookCopy, settings, db);

            var loan = new Loan
            {
                ReaderId = readerId,
                BookCopyId = bookCopyId,
                IssueDate = DateTime.Now,
                ExpectedReturnDate = DateTime.Now.Date.AddDays(loanDays), 
                FixedPrice = bookCopy.Book.Price,
                FixedPenaltyBaseAmount = settings.PenaltyBaseAmount,
                FixedPenaltyPercent = settings.PenaltyPercent
            };

            bookCopy.Status = BookCopyStatus.Loaned;
            db.Loans.Add(loan);
            await db.SaveChangesAsync();

            return OperationResult.Success("Книгу успішно видано.");
        }

        public async Task<OperationResult> ReturnBookAsync(int loanId, int newStateScore)
        {
            using var db = new LibraryContext();
            var loan = await db.Loans
                .Include(l => l.BookCopy)
                .FirstOrDefaultAsync(l => l.Id == loanId);
            if (loan == null) return OperationResult.Failure("Видачу не знайдено.");
            if (loan.ActualReturnDate != null) return OperationResult.Failure("Ця книга вже була повернута.");
            if (loan.IsLost) return OperationResult.Failure("Неможливо повернути книгу, яка вважається втраченою.");

            loan.ActualReturnDate = DateTime.Now;
            loan.BookCopy.Status = BookCopyStatus.Available;
            loan.BookCopy.StateScore = newStateScore;

            if (DateTime.Now.Date > loan.ExpectedReturnDate.Date)
            {
                loan.IsLossPaid = false;
                await db.SaveChangesAsync();
                return OperationResult.Success("Книгу повернуто із запізненням. Згенеровано штраф за прострочку.");
            }
            else
            {
                loan.IsLossPaid = true;
                await db.SaveChangesAsync();
                return OperationResult.Success("Книгу успішно повернуто вчасно.");
            }
        }

        public async Task<OperationResult> ReportLossAsync(int loanId)
        {
            using var db = new LibraryContext();
            var loan = await db.Loans.Include(l => l.BookCopy).FirstOrDefaultAsync(l => l.Id == loanId);
            if (loan == null) return OperationResult.Failure("Видачу не знайдено.");
            if (loan.IsLost) return OperationResult.Failure("Ця книга вже вважається втраченою.");
            if (loan.ActualReturnDate != null)
                return OperationResult.Failure("Неможливо втратити книгу, яку вже повернули!");

            loan.IsLost = true;
            loan.BookCopy.Status = BookCopyStatus.Lost;
            loan.IsLossPaid = false;

            await db.SaveChangesAsync();
            return OperationResult.Success("Втрату зафіксовано. Читачу нараховано борг.");
        }

        public async Task<OperationResult> PayDebtAsync(int loanId)
        {
            using var db = new LibraryContext();
            var loan = await db.Loans.FirstOrDefaultAsync(l => l.Id == loanId);
            if (loan == null) return OperationResult.Failure("Видачу не знайдено.");
            if (loan.IsLossPaid) return OperationResult.Failure("Борг по цій транзакції вже оплачено або відсутній.");

            loan.IsLossPaid = true;
            loan.PaymentDate = DateTime.Now;

            if (loan.IsLost && loan.ActualReturnDate == null)
            {
                loan.ActualReturnDate = DateTime.Now;
            }

            await db.SaveChangesAsync();
            return OperationResult.Success("Борг успішно погашено.");
        }

        private async Task<OperationResult> CheckTemporaryBanAsync(LibraryContext db, int readerId, int bookId, SystemSettings settings)
        {
            var banThresholdDate = DateTime.Now.AddDays(-settings.BanDaysForShortLoan);
            var recentShortLoan = await db.Loans
                .Where(l => l.ReaderId == readerId
                            && l.BookCopy.BookId == bookId
                            && l.ActualReturnDate != null
                            && l.ActualReturnDate >= banThresholdDate)
                .ToListAsync();

            var shortLoan = recentShortLoan.FirstOrDefault(l => (l.ExpectedReturnDate - l.IssueDate).TotalDays <= settings.ShortLoanDays);
            if (shortLoan != null && shortLoan.ActualReturnDate.HasValue)
            {
                var banEndDate = shortLoan.ActualReturnDate.Value.AddDays(settings.BanDaysForShortLoan);
                var daysLeft = (banEndDate - DateTime.Now).Days;
                if (daysLeft > 0)
                    return OperationResult.Failure($"Діє тимчасовий бан на отримання цієї книги ще {daysLeft} днів.");
            }

            return OperationResult.Success();
        }

        private int CalculateLoanDays(BookCopy copy, SystemSettings settings, LibraryContext db)
        {
            int bookAge = DateTime.Now.Year - copy.Book.PublicationYear;
            int totalCopies = db.BookCopies.Count(bc => bc.BookId == copy.BookId && bc.Status != BookCopyStatus.WrittenOff);

            if (bookAge <= settings.NewBookAgeThreshold && totalCopies < settings.RareBookCopiesThreshold)
            {
                return settings.ShortLoanDays;
            }
            return settings.LongLoanDays;
        }

        public async Task<int> GetActiveLoansCountAsync(int readerId)
        {
            using var db = new LibraryContext();
            return await db.Loans.CountAsync(l => l.ReaderId == readerId && l.ActualReturnDate == null);
        }

        public async Task<bool> HasUnpaidDebtsAsync(int readerId)
        {
            using var db = new LibraryContext();
            return await db.Loans.AnyAsync(l => l.ReaderId == readerId && !l.IsLossPaid);
        }

        public async Task<OperationResult> WriteOffCopyAsync(int copyId)
        {
            using var db = new LibraryContext();
            var copy = await db.BookCopies.FindAsync(copyId);
            if (copy == null)
                return OperationResult.Failure("Примірник не знайдено.");
            if (copy.Status == BookCopyStatus.Loaned)
                return OperationResult.Failure("Не можна списати книгу, яка зараз знаходиться на руках у читача!");
            if (copy.Status == BookCopyStatus.WrittenOff)
                return OperationResult.Failure("Цей примірник вже списано.");

            copy.Status = BookCopyStatus.WrittenOff;
            await db.SaveChangesAsync();
            return OperationResult.Success("Примірник успішно списано.");
        }

        public async Task MarkZeroDebtLoansAsPaidAsync(int readerId)
        {
            using var db = new LibraryContext();
            var loans = await db.Loans
                .Where(l => l.ReaderId == readerId && !l.IsLossPaid)
                .ToListAsync();

            foreach (var loan in loans)
            {
                if (loan.CalculateCurrentDebt() == 0 && loan.ActualReturnDate != null && !loan.IsLost)
                {
                    loan.IsLossPaid = true;
                }
            }

            await db.SaveChangesAsync();
        }
        public async Task UpdateAllOverdueLoansAsync()
        {
            using var db = new LibraryContext();
            var overdueLoans = await db.Loans
                .Where(l => l.ActualReturnDate == null && !l.IsLost && l.ExpectedReturnDate < DateTime.Now.Date && l.IsLossPaid)
                .ToListAsync();
            foreach (var loan in overdueLoans)
            {
                loan.IsLossPaid = false;
            }
            await db.SaveChangesAsync();
        }
        
    }
}