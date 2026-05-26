using Microsoft.EntityFrameworkCore;
using LibSys.Data;
using LibSys.Models;

namespace LibSys.Services
{
    public interface IBookEditService
    {
        Task<Book?> GetBookWithDetailsAsync(int bookId);
        Task SaveBookAsync(Book book, int publisherId, List<int> authorIds, List<int> tagIds, byte[]? coverImage);
        Task<int> AddPublisherAsync(string name);
        Task<int> AddAuthorAsync(Author author);
        Task<int> AddTagAsync(string name);
        Task AddBookCopiesAsync(int bookId, int quantity, int initialState, DateTime arrivalDate);
    }

    public class BookEditService : IBookEditService
    {
        public async Task<Book?> GetBookWithDetailsAsync(int bookId)
        {
            using var db = new LibraryContext();
            return await db.Books
                .Include(b => b.Authors)
                .Include(b => b.Tags)
                .FirstOrDefaultAsync(b => b.Id == bookId);
        }

        public async Task SaveBookAsync(Book book, int publisherId, List<int> authorIds, List<int> tagIds, byte[]? coverImage)
        {
            using var db = new LibraryContext();
            book.PublisherId = publisherId;
            book.CoverImage = coverImage;

            if (book.Id == 0)
            {
                await db.Books.AddAsync(book);
            }
            else
            {
                var existing = await db.Books
                    .Include(b => b.Authors)
                    .Include(b => b.Tags)
                    .FirstOrDefaultAsync(b => b.Id == book.Id);
                if (existing == null) throw new Exception("Книгу не знайдено");

                existing.Title = book.Title;
                existing.PublicationYear = book.PublicationYear;
                existing.Price = book.Price;
                existing.PublisherId = publisherId;
                existing.CoverImage = coverImage;
                existing.Authors.Clear();
                existing.Tags.Clear();
                db.Entry(existing).State = EntityState.Modified;
                book = existing;
            }

            foreach (int id in authorIds)
            {
                var author = await db.Authors.FindAsync(id);
                if (author != null) book.Authors.Add(author);
            }
            foreach (int id in tagIds)
            {
                var tag = await db.Tags.FindAsync(id);
                if (tag != null) book.Tags.Add(tag);
            }

            await db.SaveChangesAsync();
        }

        public async Task<int> AddPublisherAsync(string name)
        {
            using var db = new LibraryContext();
            var pub = new Publisher { Name = name };
            await db.Publishers.AddAsync(pub);
            await db.SaveChangesAsync();
            return pub.Id;
        }

        public async Task<int> AddAuthorAsync(Author author)
        {
            using var db = new LibraryContext();
            await db.Authors.AddAsync(author);
            await db.SaveChangesAsync();
            return author.Id;
        }

        public async Task<int> AddTagAsync(string name)
        {
            using var db = new LibraryContext();
            var tag = new Tag { Name = name };
            await db.Tags.AddAsync(tag);
            await db.SaveChangesAsync();
            return tag.Id;
        }

        public async Task AddBookCopiesAsync(int bookId, int quantity, int initialState, DateTime arrivalDate)
        {
            using var db = new LibraryContext();
            var copies = new List<BookCopy>(quantity);
            for (int i = 0; i < quantity; i++)
            {
                copies.Add(new BookCopy
                {
                    BookId = bookId,
                    ArrivalDate = arrivalDate,
                    StateScore = initialState,
                    Status = BookCopyStatus.Available
                });
            }
            await db.BookCopies.AddRangeAsync(copies);
            await db.SaveChangesAsync();
        }
    }
}