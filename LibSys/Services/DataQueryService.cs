using Microsoft.EntityFrameworkCore;
using LibSys.Data;
using LibSys.Models;

namespace LibSys.Services
{
	public interface IDataQueryService
	{
		Task<List<CatalogItemDto>> GetCatalogAsync(string searchQuery, CatalogSortOption sortOption,
			HashSet<int> authorIds,
			HashSet<int> publisherIds, HashSet<int> years, HashSet<int> tagIds, bool showArchived = false);

		Task<List<InventoryItemDto>> GetInventoryAsync(string searchQuery, InventorySortOption sortOption,
			bool showAvailable,
			bool showLoaned, bool showLost, bool showWrittenOff);

		Task<List<ReaderItemDto>> GetReadersAsync(string searchQuery, bool debtorsOnly);
		Task<List<AvailableCopyDto>> GetAvailableCopiesForIssueAsync(string searchQuery);
		Task<List<JournalItemDto>> GetJournalAsync(DateTime startDate, DateTime endDate);
		Task<List<Author>> GetFilterAuthorsAsync();
		Task<List<Publisher>> GetFilterPublishersAsync();
		Task<List<int>> GetFilterYearsAsync();
		Task<List<Tag>> GetFilterTagsAsync();
		Task<byte[]?> GetCoverImageAsync(int bookId);
		Task<(int TotalCopies, int AvailableCopies)> GetCopiesCountAsync(int bookId);
		Task<Reader?> GetReaderByIdAsync(int readerId);
		Task<List<Loan>> GetLoansWithDetailsByReaderIdAsync(int readerId);
		Task<SystemSettings?> GetSystemSettingsAsync();
		Task<BookCopy?> GetBookCopyByIdAsync(int copyId);
		Task<List<Publisher>> GetAllPublishersNonArchivedAsync();
		Task<List<Author>> GetAllAuthorsNonArchivedAsync();
		Task<List<Tag>> GetAllTagsNonArchivedAsync();
		Task<List<Loan>> GetUnpaidLoansByReaderAsync(int readerId);
		Task<List<Author>> GetAuthorsFilteredAsync(bool showArchived, string search);
		Task<List<Publisher>> GetPublishersFilteredAsync(bool showArchived, string search);
		Task<List<Tag>> GetTagsFilteredAsync(bool showArchived, string search);
		Task<List<Book>> GetBooksFilteredAsync(bool showArchived, string search);
		Task<Loan?> GetLoanWithDetailsByIdAsync(int loanId);
		Task SaveSystemSettingsAsync(SystemSettings settings);
	}

	public class DataQueryService : IDataQueryService
	{
		public async Task<List<CatalogItemDto>> GetCatalogAsync(string searchQuery, CatalogSortOption sortOption,
			HashSet<int> authorIds,
			HashSet<int> publisherIds, HashSet<int> years, HashSet<int> tagIds, bool showArchived = false)
		{
			using var db = new LibraryContext();
			var query = db.Books
				.Include(b => b.Publisher)
				.Include(b => b.Authors)
				.Include(b => b.Tags)
				.Where(b => b.IsArchived == showArchived)
				.AsQueryable();

			if (authorIds.Count > 0) query = query.Where(b => b.Authors.Any(a => authorIds.Contains(a.Id)));
			if (publisherIds.Count > 0) query = query.Where(b => publisherIds.Contains(b.PublisherId));
			if (years.Count > 0) query = query.Where(b => years.Contains(b.PublicationYear));
			if (tagIds.Count > 0) query = query.Where(b => b.Tags.Any(t => tagIds.Contains(t.Id)));

			query = sortOption switch
			{
				CatalogSortOption.TitleAsc => query.OrderBy(b => b.Title),
				CatalogSortOption.TitleDesc => query.OrderByDescending(b => b.Title),
				CatalogSortOption.PublisherAsc => query.OrderBy(b => b.Publisher.Name),
				CatalogSortOption.PublisherDesc => query.OrderByDescending(b => b.Publisher.Name),
				CatalogSortOption.YearNewest => query.OrderByDescending(b => b.PublicationYear),
				CatalogSortOption.YearOldest => query.OrderBy(b => b.PublicationYear),
				_ => query.OrderBy(b => b.Title)
			};

			var rawBooks = await query.ToListAsync();

			if (!string.IsNullOrWhiteSpace(searchQuery))
			{
				rawBooks = rawBooks.Where(b => b.Title.ContainsAllSearchTerms(searchQuery)).ToList();
			}

			return rawBooks.Select(b => new CatalogItemDto
			{
				ID = b.Id,
				Назва = b.Title,
				Видавництво = b.Publisher.Name,
				Рік = b.PublicationYear,
				Автори = string.Join(", ", b.Authors.Select(a => a.DisplayName)),
				Теги = string.Join(", ", b.Tags.Select(t => t.Name))
			}).ToList();
		}

		public async Task<List<InventoryItemDto>> GetInventoryAsync(string searchQuery, InventorySortOption sortOption,
			bool showAvailable, bool showLoaned, bool showLost, bool showWrittenOff)
		{
			using var db = new LibraryContext();
			var query = db.BookCopies
				.Include(bc => bc.Book).ThenInclude(b => b.Authors)
				.Include(bc => bc.Book).ThenInclude(b => b.Publisher)
				.Where(bc =>
					(showAvailable && bc.Status == BookCopyStatus.Available) ||
					(showLoaned && bc.Status == BookCopyStatus.Loaned) ||
					(showLost && bc.Status == BookCopyStatus.Lost) ||
					(showWrittenOff && bc.Status == BookCopyStatus.WrittenOff)
				).AsQueryable();

			var list = await query.ToListAsync();

			if (!string.IsNullOrWhiteSpace(searchQuery))
			{
				bool isSingleNumber = int.TryParse(searchQuery.Trim(), out int copyId);
				list = list.Where(bc =>
				{
					if (isSingleNumber && bc.Id == copyId) return true;
					string fullSearchString =
						$"{bc.Book.Title} {bc.Book.Publisher.Name} {string.Join(" ", bc.Book.Authors.Select(a => a.DisplayName))}";
					return fullSearchString.ContainsAllSearchTerms(searchQuery);
				}).ToList();
			}

			list = sortOption switch
			{
				InventorySortOption.StateWorst => list.OrderBy(bc => bc.StateScore).ToList(),
				InventorySortOption.StateBest => list.OrderByDescending(bc => bc.StateScore).ToList(),
				InventorySortOption.DateNewest => list.OrderByDescending(bc => bc.ArrivalDate).ToList(),
				InventorySortOption.DateOldest => list.OrderBy(bc => bc.ArrivalDate).ToList(),
				_ => list
			};

			return list.Select(bc => new InventoryItemDto
			{
				ID_Книги = bc.BookId,
				Інв_Номер = bc.Id,
				Назва = bc.Book.Title,
				Автор = string.Join(", ", bc.Book.Authors.Select(a => a.DisplayName)),
				Видавництво = bc.Book.Publisher.Name,
				Рік = bc.Book.PublicationYear,
				Стан = bc.StateScore.ToString(),
				Статус = bc.Status.ToLocalizedString(),
				Надходження = bc.ArrivalDate.ToShortDateString()
			}).ToList();
		}

		public async Task<List<ReaderItemDto>> GetReadersAsync(string searchQuery, bool debtorsOnly)
		{
			using var db = new LibraryContext();
			var query = db.Readers.AsQueryable();

			if (debtorsOnly)
			{
				var debtorIds = await db.Loans.Where(l => l.IsLossPaid == false).Select(l => l.ReaderId).Distinct()
					.ToListAsync();
				query = query.Where(r => debtorIds.Contains(r.Id));
			}

			var readersList = await query.ToListAsync();

			if (!string.IsNullOrWhiteSpace(searchQuery))
			{
				readersList = readersList.Where(r =>
					$"{r.FullName} {r.PassportNumber} {r.Phone} {r.Email}".ContainsAllSearchTerms(searchQuery)
				).ToList();
			}

			return readersList.Select(r => new ReaderItemDto
			{
				ID = r.Id,
				Прізвище = r.LastName ?? "",
				Ім_я = r.FirstName ?? "",
				По_батькові = r.MiddleName ?? "—",
				Народження = r.BirthDate.FromUnixTimestamp().ToString("dd.MM.yyyy"),
				Паспорт = r.PassportNumber,
				Телефон = r.Phone,
				Email = r.Email ?? "—"
			}).ToList();
		}

		public async Task<List<AvailableCopyDto>> GetAvailableCopiesForIssueAsync(string searchQuery)
		{
			using var db = new LibraryContext();
			var query = db.BookCopies
				.Include(bc => bc.Book).ThenInclude(b => b.Authors)
				.Include(bc => bc.Book).ThenInclude(b => b.Publisher)
				.Where(bc => bc.Status == BookCopyStatus.Available)
				.AsQueryable();

			var list = await query.ToListAsync();

			if (!string.IsNullOrWhiteSpace(searchQuery))
			{
				bool isSingleNumber = int.TryParse(searchQuery.Trim(), out int copyId);
				list = list.Where(bc =>
				{
					if (isSingleNumber && bc.Id == copyId) return true;
					string fullSearchString =
						$"{bc.Book.Title} {bc.Book.Publisher.Name} {string.Join(" ", bc.Book.Authors.Select(a => a.DisplayName))}";
					return fullSearchString.ContainsAllSearchTerms(searchQuery);
				}).ToList();
			}

			return list.OrderByDescending(bc => bc.ArrivalDate).Select(bc => new AvailableCopyDto
			{
				Інв_Номер = bc.Id,
				Назва = bc.Book.Title,
				Автор = string.Join(", ", bc.Book.Authors.Select(a => a.DisplayName)),
				Видавництво = bc.Book.Publisher.Name,
				Рік = bc.Book.PublicationYear,
				Стан = bc.StateScore
			}).ToList();
		}

		public async Task<List<JournalItemDto>> GetJournalAsync(DateTime startDate, DateTime endDate)
		{
			using var db = new LibraryContext();
			var loans = await db.Loans
				.Include(l => l.Reader)
				.Include(l => l.BookCopy).ThenInclude(bc => bc.Book).ThenInclude(b => b.Authors)
				.Where(l =>
					(l.IssueDate.Date >= startDate.Date && l.IssueDate.Date <= endDate.Date) ||
					(l.ActualReturnDate != null && l.ActualReturnDate.Value.Date >= startDate.Date &&
					 l.ActualReturnDate.Value.Date <= endDate.Date))
				.ToListAsync();

			var journal = new List<JournalItemDto>();

			foreach (var l in loans)
			{
				string bookTitle = l.BookCopy.Book.Title;
				string authors = string.Join(", ", l.BookCopy.Book.Authors.Select(a => a.DisplayName));
				string readerName = l.Reader.FullName;

				if (l.IssueDate.Date >= startDate.Date && l.IssueDate.Date <= endDate.Date)
				{
					journal.Add(CreateJournalDto(l.IssueDate, bookTitle, authors, readerName,
						$"{UIIcons.ActionIssue} Видано", l.ReaderId));
				}

				if (l.ActualReturnDate.HasValue && l.ActualReturnDate.Value.Date >= startDate.Date &&
				    l.ActualReturnDate.Value.Date <= endDate.Date)
				{
					string operationType =
						l.IsLost ? $"{UIIcons.ActionLoss} Втрачено" : $"{UIIcons.ActionReturn} Повернено";
					journal.Add(CreateJournalDto(l.ActualReturnDate.Value, bookTitle, authors, readerName,
						operationType, l.ReaderId));
				}
			}

			return journal.OrderByDescending(j => j.DateValue).ToList();
		}

		private JournalItemDto CreateJournalDto(DateTime date, string book, string authors, string reader,
			string operation, int readerId)
		{
			return new JournalItemDto
			{
				Дата = date.ToShortDateString(),
				DateValue = date,
				Книга = book,
				Автори = authors,
				Читач = reader,
				Операція = operation,
				ReaderId = readerId
			};
		}

		public async Task<List<Author>> GetFilterAuthorsAsync()
		{
			using var db = new LibraryContext();
			return await db.Authors.Where(a => !a.IsArchived).OrderBy(a => a.LastName).ToListAsync();
		}

		public async Task<List<Publisher>> GetFilterPublishersAsync()
		{
			using var db = new LibraryContext();
			return await db.Publishers.Where(p => !p.IsArchived).OrderBy(p => p.Name).ToListAsync();
		}

		public async Task<List<int>> GetFilterYearsAsync()
		{
			using var db = new LibraryContext();
			return await db.Books.Where(b => !b.IsArchived).Select(b => b.PublicationYear).Distinct()
				.OrderByDescending(y => y).ToListAsync();
		}

		public async Task<List<Tag>> GetFilterTagsAsync()
		{
			using var db = new LibraryContext();
			return await db.Tags.Where(t => !t.IsArchived).OrderBy(t => t.Name).ToListAsync();
		}

		public async Task<byte[]?> GetCoverImageAsync(int bookId)
		{
			using var db = new LibraryContext();
			return await db.Books.Where(b => b.Id == bookId).Select(b => b.CoverImage).FirstOrDefaultAsync();
		}

		public async Task<(int TotalCopies, int AvailableCopies)> GetCopiesCountAsync(int bookId)
		{
			using var db = new LibraryContext();
			var copies = await db.BookCopies.Where(bc => bc.BookId == bookId).ToListAsync();
			int total = copies.Count;
			int available = copies.Count(bc => bc.Status == BookCopyStatus.Available);
			return (total, available);
		}

		public async Task<Reader?> GetReaderByIdAsync(int readerId)
		{
			using var db = new LibraryContext();
			return await db.Readers.FindAsync(readerId);
		}

		public async Task<List<Loan>> GetLoansWithDetailsByReaderIdAsync(int readerId)
		{
			using var db = new LibraryContext();
			return await db.Loans
				.Include(l => l.BookCopy).ThenInclude(c => c.Book)
				.Where(l => l.ReaderId == readerId)
				.OrderByDescending(l => l.IssueDate)
				.ToListAsync();
		}

		public async Task<SystemSettings?> GetSystemSettingsAsync()
		{
			using var db = new LibraryContext();
			return await db.SystemSettings.FirstOrDefaultAsync();
		}

		public async Task<BookCopy?> GetBookCopyByIdAsync(int copyId)
		{
			using var db = new LibraryContext();
			return await db.BookCopies.FindAsync(copyId);
		}

		public async Task<List<Publisher>> GetAllPublishersNonArchivedAsync()
		{
			using var db = new LibraryContext();
			return await db.Publishers.Where(p => !p.IsArchived).ToListAsync();
		}

		public async Task<List<Author>> GetAllAuthorsNonArchivedAsync()
		{
			using var db = new LibraryContext();
			return await db.Authors.Where(a => !a.IsArchived).ToListAsync();
		}

		public async Task<List<Tag>> GetAllTagsNonArchivedAsync()
		{
			using var db = new LibraryContext();
			return await db.Tags.Where(t => !t.IsArchived).ToListAsync();
		}

		public async Task<List<Loan>> GetUnpaidLoansByReaderAsync(int readerId)
		{
			using var db = new LibraryContext();
			return await db.Loans
				.Include(l => l.BookCopy).ThenInclude(bc => bc.Book)
				.Where(l => l.ReaderId == readerId && (!l.IsLossPaid || l.ActualReturnDate == null))
				.ToListAsync();
		}

		public async Task<List<Author>> GetAuthorsFilteredAsync(bool showArchived, string search)
		{
			using var db = new LibraryContext();
			var authors = await db.Authors
				.Where(a => a.IsArchived == showArchived)
				.ToListAsync();

			if (!string.IsNullOrWhiteSpace(search))
			{
				var terms = search.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				authors = authors.Where(a =>
					terms.All(term =>
						(a.LastName != null && a.LastName.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
						(a.FirstName != null && a.FirstName.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
						(a.MiddleName != null && a.MiddleName.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
						(a.Pseudonym != null && a.Pseudonym.Contains(term, StringComparison.OrdinalIgnoreCase))
					)
				).ToList();
			}
			return authors;
		}

		public async Task<List<Publisher>> GetPublishersFilteredAsync(bool showArchived, string search)
		{
			using var db = new LibraryContext();
			var publishers = await db.Publishers
				.Where(p => p.IsArchived == showArchived)
				.ToListAsync();

			if (!string.IsNullOrWhiteSpace(search))
			{
				var terms = search.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				publishers = publishers.Where(p =>
					terms.All(term => p.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
				).ToList();
			}
			return publishers;
		}


		public async Task<List<Tag>> GetTagsFilteredAsync(bool showArchived, string search)
		{
			using var db = new LibraryContext();
			var tags = await db.Tags
				.Where(t => t.IsArchived == showArchived)
				.ToListAsync();

			if (!string.IsNullOrWhiteSpace(search))
			{
				var terms = search.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				tags = tags.Where(t =>
					terms.All(term => t.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
				).ToList();
			}
			return tags;
		}

		public async Task<List<Book>> GetBooksFilteredAsync(bool showArchived, string search)
		{
			using var db = new LibraryContext();
			var books = await db.Books
				.Where(b => b.IsArchived == showArchived)
				.ToListAsync();

			if (!string.IsNullOrWhiteSpace(search))
			{
				var terms = search.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				books = books.Where(b =>
					terms.All(term => b.Title.Contains(term, StringComparison.OrdinalIgnoreCase))
				).ToList();
			}
			return books;
		}

		public async Task<Loan?> GetLoanWithDetailsByIdAsync(int loanId)
		{
			using var db = new LibraryContext();
			return await db.Loans
				.Include(l => l.BookCopy)
				.ThenInclude(c => c.Book)
				.FirstOrDefaultAsync(l => l.Id == loanId);
		}
		public async Task SaveSystemSettingsAsync(SystemSettings settings)
		{
			using var db = new LibraryContext();
			db.SystemSettings.Update(settings);
			await db.SaveChangesAsync();
		}
	}
}