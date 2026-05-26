using Microsoft.EntityFrameworkCore;
using LibSys.Models;
using LibSys.Data;

namespace LibSys.Services
{
    public interface IArchiveManager
    {
        Task<ArchiveImpact> GetAuthorArchiveImpactAsync(int authorId);
        Task<OperationResult> ArchiveAuthorAsync(int authorId, bool confirmed = false);

        Task<ArchiveImpact> GetPublisherArchiveImpactAsync(int publisherId);
        Task<OperationResult> ArchivePublisherAsync(int publisherId, bool confirmed = false);

        Task<ArchiveImpact> GetTagArchiveImpactAsync(int tagId);
        Task<OperationResult> ArchiveTagAsync(int tagId, bool confirmed = false);

        Task<ArchiveImpact> GetBookArchiveImpactAsync(int bookId);
        Task<OperationResult> ArchiveBookAsync(int bookId, bool confirmed = false);

        Task<OperationResult> RestoreBookAsync(int bookId);
        Task<OperationResult> RestoreAuthorAsync(int authorId);
        Task<OperationResult> RestorePublisherAsync(int publisherId);
        Task<OperationResult> RestoreTagAsync(int tagId);
    }

    public class ArchiveImpact
    {
        public string Message { get; set; } = "";
        public bool IsDestructive { get; set; }
        public int BooksToDelete { get; set; }
        public int BooksToArchive { get; set; }
        public int CopiesToWriteOff { get; set; }
        public int TagsToDetach { get; set; }
    }

    public class ArchiveManager : IArchiveManager
    {
        // ========== АВТОРИ ==========
        public async Task<ArchiveImpact> GetAuthorArchiveImpactAsync(int authorId)
        {
            using var db = new LibraryContext();
            var author = await db.Authors
                .Include(a => a.Books).ThenInclude(b => b.BookCopies)
                .FirstOrDefaultAsync(a => a.Id == authorId);
            if (author == null) return new ArchiveImpact { Message = "Автора не знайдено." };

            int booksToDelete = 0, booksToArchive = 0, copiesToWriteOff = 0;
            foreach (var book in author.Books)
            {
                if (book.BookCopies.Any())
                {
                    booksToArchive++;
                    copiesToWriteOff += book.BookCopies.Count;
                }
                else booksToDelete++;
            }

            string msg = $"Архівація автора «{author.DisplayName}»:\n";
            if (booksToDelete > 0) msg += $"⚠️ Книги без примірників: {booksToDelete} (будуть остаточно видалені)\n";
            if (booksToArchive > 0) msg += $"📚 Книги з примірниками: {booksToArchive} (будуть архівовані)\n";
            if (copiesToWriteOff > 0) msg += $"📦 Примірники: {copiesToWriteOff} (отримають статус «Списано»)\n";
            if (booksToDelete == 0 && booksToArchive == 0) msg = "У автора немає книг. Він буде повністю видалений.";
            else if (booksToArchive == 0) msg += "Автор буде повністю видалений (разом з книгами без примірників).";
            else msg += "Автор буде архівований (доступний для історії).";

            return new ArchiveImpact
            {
                Message = msg,
                IsDestructive = booksToDelete > 0 || copiesToWriteOff > 0,
                BooksToDelete = booksToDelete,
                BooksToArchive = booksToArchive,
                CopiesToWriteOff = copiesToWriteOff
            };
        }

        public async Task<OperationResult> ArchiveAuthorAsync(int authorId, bool confirmed = false)
        {
            if (!confirmed)
            {
                var impact = await GetAuthorArchiveImpactAsync(authorId);
                return OperationResult.Failure(impact.Message);
            }

            using var db = new LibraryContext();
            var author = await db.Authors
                .Include(a => a.Books).ThenInclude(b => b.BookCopies)
                .FirstOrDefaultAsync(a => a.Id == authorId);
                
            if (author == null) return OperationResult.Failure("Автора не знайдено.");
            if (author.IsArchived) return OperationResult.Failure("Автор вже в архіві.");

            bool anyChanges = false;
            foreach (var book in author.Books.ToList())
            {
                if (book.BookCopies.Any())
                {
                    book.IsArchived = true;
                    foreach (var copy in book.BookCopies)
                        copy.Status = BookCopyStatus.WrittenOff;
                    anyChanges = true;
                }
                else
                {
                    db.Books.Remove(book);
                    anyChanges = true;
                }
            }

            if (author.Books.Any(b => b.BookCopies.Any()))
                author.IsArchived = true;
            else
                db.Authors.Remove(author);

            if (anyChanges) await db.SaveChangesAsync();
            return OperationResult.Success("Операцію виконано.");
        }

        // ========== ВИДАВНИЦТВА ==========
        public async Task<ArchiveImpact> GetPublisherArchiveImpactAsync(int publisherId)
        {
            using var db = new LibraryContext();
            var publisher = await db.Publishers
                .Include(p => p.Books).ThenInclude(b => b.BookCopies)
                .FirstOrDefaultAsync(p => p.Id == publisherId);
            if (publisher == null) return new ArchiveImpact { Message = "Видавництво не знайдено." };

            int booksToDelete = 0, booksToArchive = 0, copiesToWriteOff = 0;
            foreach (var book in publisher.Books)
            {
                if (book.BookCopies.Any())
                {
                    booksToArchive++;
                    copiesToWriteOff += book.BookCopies.Count;
                }
                else booksToDelete++;
            }

            string msg = $"Архівація видавництва «{publisher.Name}»:\n";
            if (booksToDelete > 0) msg += $"⚠️ Книги без примірників: {booksToDelete} (будуть остаточно видалені)\n";
            if (booksToArchive > 0) msg += $"📚 Книги з примірниками: {booksToArchive} (будуть архівовані)\n";
            if (copiesToWriteOff > 0) msg += $"📦 Примірники: {copiesToWriteOff} (отримають статус «Списано»)\n";
            if (booksToDelete == 0 && booksToArchive == 0) msg = "У видавництва немає книг. Воно буде повністю видалене.";
            else if (booksToArchive == 0) msg += "Видавництво буде повністю видалене (разом з книгами без примірників).";
            else msg += "Видавництво буде архівоване (доступне для історії).";

            return new ArchiveImpact
            {
                Message = msg,
                IsDestructive = booksToDelete > 0 || copiesToWriteOff > 0,
                BooksToDelete = booksToDelete,
                BooksToArchive = booksToArchive,
                CopiesToWriteOff = copiesToWriteOff
            };
        }

        public async Task<OperationResult> ArchivePublisherAsync(int publisherId, bool confirmed = false)
        {
            if (!confirmed)
            {
                var impact = await GetPublisherArchiveImpactAsync(publisherId);
                return OperationResult.Failure(impact.Message);
            }

            using var db = new LibraryContext();
            var publisher = await db.Publishers
                .Include(p => p.Books).ThenInclude(b => b.BookCopies)
                .FirstOrDefaultAsync(p => p.Id == publisherId);
                
            if (publisher == null) return OperationResult.Failure("Видавництво не знайдено.");
            if (publisher.IsArchived) return OperationResult.Failure("Видавництво вже в архіві.");

            bool anyChanges = false;
            foreach (var book in publisher.Books.ToList())
            {
                if (book.BookCopies.Any())
                {
                    book.IsArchived = true;
                    foreach (var copy in book.BookCopies)
                        copy.Status = BookCopyStatus.WrittenOff;
                    anyChanges = true;
                }
                else
                {
                    db.Books.Remove(book);
                    anyChanges = true;
                }
            }

            if (publisher.Books.Any(b => b.BookCopies.Any()))
                publisher.IsArchived = true;
            else
                db.Publishers.Remove(publisher);

            if (anyChanges) await db.SaveChangesAsync();
            return OperationResult.Success("Операцію виконано.");
        }

        // ========== ТЕГИ ==========
        public async Task<ArchiveImpact> GetTagArchiveImpactAsync(int tagId)
        {
            using var db = new LibraryContext();

            var tag = await db.Tags
                .Include(t => t.Books)
                .FirstOrDefaultAsync(t => t.Id == tagId);
                
            if (tag == null) return new ArchiveImpact { Message = "Тег не знайдено." };

            int totalBooks = tag.Books.Count;
            string msg = $"Архівація тегу «{tag.Name}»:\n";
            
            if (totalBooks > 0) 
            {
                msg += $"Тег зараз використовується у {totalBooks} книгах.\n";
                msg += "Він буде переведений в архів: залишиться у вже існуючих книгах, але зникне зі списку для нових.";
                
                return new ArchiveImpact
                {
                    Message = msg,
                    IsDestructive = false,
                    TagsToDetach = 0,
                    BooksToArchive = 0,
                    BooksToDelete = 0
                };
            }
            
            msg += "Тег не використовується у жодній книзі. Він буде остаточно видалений.";
            return new ArchiveImpact
            {
                Message = msg,
                IsDestructive = true,
                TagsToDetach = 0,
                BooksToArchive = 0,
                BooksToDelete = 0
            };
        }

        public async Task<OperationResult> ArchiveTagAsync(int tagId, bool confirmed = false)
        {
            if (!confirmed)
            {
                var impact = await GetTagArchiveImpactAsync(tagId);
                return OperationResult.Failure(impact.Message);
            }

            using var db = new LibraryContext();
            var tag = await db.Tags
                .Include(t => t.Books).ThenInclude(b => b.BookCopies)
                .FirstOrDefaultAsync(t => t.Id == tagId);
            if (tag == null) return OperationResult.Failure("Тег не знайдено.");
            if (tag.IsArchived) return OperationResult.Failure("Тег вже в архіві.");

            if (!tag.Books.Any())
            {
                db.Tags.Remove(tag);
                await db.SaveChangesAsync();
                return OperationResult.Success("Тег остаточно видалено.");
            }

            tag.IsArchived = true;
            await db.SaveChangesAsync();
            return OperationResult.Success("Тег архівовано. Він більше не з'являтиметься у списку для нових книг.");
        }

        // ========== КНИГИ ==========
        public async Task<ArchiveImpact> GetBookArchiveImpactAsync(int bookId)
        {
            using var db = new LibraryContext();
            var book = await db.Books.Include(b => b.BookCopies).FirstOrDefaultAsync(b => b.Id == bookId);
            if (book == null) return new ArchiveImpact { Message = "Книгу не знайдено." };
            if (book.BookCopies.Any())
                return new ArchiveImpact
                {
                    Message = $"Книга «{book.Title}» має {book.BookCopies.Count} примірник(ів). Всі вони будуть списані, книга архівована.",
                    CopiesToWriteOff = book.BookCopies.Count,
                    IsDestructive = true
                };
            else
                return new ArchiveImpact
                {
                    Message = $"Книга «{book.Title}» не має примірників. Вона буде остаточно видалена.",
                    IsDestructive = true
                };
        }

        public async Task<OperationResult> ArchiveBookAsync(int bookId, bool confirmed = false)
        {
            if (!confirmed)
            {
                var impact = await GetBookArchiveImpactAsync(bookId);
                return OperationResult.Failure(impact.Message);
            }
            using var db = new LibraryContext();
            var book = await db.Books.Include(b => b.BookCopies).FirstOrDefaultAsync(b => b.Id == bookId);
            if (book == null) return OperationResult.Failure("Книгу не знайдено.");
            if (book.IsArchived) return OperationResult.Failure("Книга вже в архіві.");
            if (book.BookCopies.Any())
            {
                book.IsArchived = true;
                foreach (var copy in book.BookCopies)
                    copy.Status = BookCopyStatus.WrittenOff;
            }
            else
                db.Books.Remove(book);
            await db.SaveChangesAsync();
            return OperationResult.Success("Операцію виконано.");
        }

        // ========== ВІДНОВЛЕННЯ ==========
        public async Task<OperationResult> RestoreBookAsync(int bookId)
        {
            using var db = new LibraryContext();
            var book = await db.Books
                .Include(b => b.Publisher)
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Id == bookId);
            if (book == null) return OperationResult.Failure("Книгу не знайдено.");
            if (!book.IsArchived) return OperationResult.Failure("Книга не знаходиться в архіві.");

            bool isPublisherArchived = book.Publisher.IsArchived;
            var archivedAuthors = book.Authors.Where(a => a.IsArchived).ToList();

            if (isPublisherArchived && archivedAuthors.Any())
            {
                string authorsList = string.Join(", ", archivedAuthors.Select(a => a.DisplayName));
                return OperationResult.Failure(
                    $"Неможливо відновити книгу «{book.Title}», оскільки:\n" +
                    $"• Видавництво «{book.Publisher.Name}» знаходиться в архіві.\n" +
                    $"• Автори: {authorsList} знаходяться в архіві.\n\n" +
                    $"Будь ласка, спочатку відновіть видавництво та авторів у розділі «Налаштування».");
            }
            if (isPublisherArchived)
            {
                return OperationResult.Failure(
                    $"Неможливо відновити книгу «{book.Title}», оскільки видавництво «{book.Publisher.Name}» знаходиться в архіві.\n\n" +
                    $"Будь ласка, спочатку відновіть видавництво у розділі «Налаштування».");
            }
            if (archivedAuthors.Any())
            {
                string authorsList = string.Join(", ", archivedAuthors.Select(a => a.DisplayName));
                return OperationResult.Failure(
                    $"Неможливо відновити книгу «{book.Title}», оскільки наступні автори знаходяться в архіві:\n" +
                    $"{authorsList}\n\nБудь ласка, спочатку відновіть авторів у розділі «Налаштування».");
            }

            book.IsArchived = false;
            await db.SaveChangesAsync();
            return OperationResult.Success(
                $"Книгу «{book.Title}» успішно відновлено. Для створення нових примірників використовуйте кнопку «Додати примірники».");
        }

        public async Task<OperationResult> RestoreAuthorAsync(int authorId)
        {
            using var db = new LibraryContext();
            var author = await db.Authors.FindAsync(authorId);
            if (author == null) return OperationResult.Failure("Автора не знайдено.");
            if (!author.IsArchived) return OperationResult.Failure("Автор не знаходиться в архіві.");
            author.IsArchived = false;
            await db.SaveChangesAsync();
            return OperationResult.Success($"Автора «{author.DisplayName}» успішно відновлено.");
        }

        public async Task<OperationResult> RestorePublisherAsync(int publisherId)
        {
            using var db = new LibraryContext();
            var publisher = await db.Publishers.FindAsync(publisherId);
            if (publisher == null) return OperationResult.Failure("Видавництво не знайдено.");
            if (!publisher.IsArchived) return OperationResult.Failure("Видавництво не знаходиться в архіві.");
            publisher.IsArchived = false;
            await db.SaveChangesAsync();
            return OperationResult.Success($"Видавництво «{publisher.Name}» успішно відновлено.");
        }

        public async Task<OperationResult> RestoreTagAsync(int tagId)
        {
            using var db = new LibraryContext();
            var tag = await db.Tags.FindAsync(tagId);
            if (tag == null) return OperationResult.Failure("Тег не знайдено.");
            if (!tag.IsArchived) return OperationResult.Failure("Тег не знаходиться в архіві.");
            tag.IsArchived = false;
            await db.SaveChangesAsync();
            return OperationResult.Success($"Тег «{tag.Name}» успішно відновлено.");
        }
    }
}