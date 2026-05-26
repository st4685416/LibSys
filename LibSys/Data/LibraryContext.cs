using Microsoft.EntityFrameworkCore;
using LibSys.Models;

namespace LibSys.Data
{
	public class LibraryContext : DbContext
	{
		public const string ConnectionString = "Data Source=library.db";

		public DbSet<Reader> Readers { get; set; } = null!;
		public DbSet<Author> Authors { get; set; } = null!;
		public DbSet<Publisher> Publishers { get; set; } = null!;
		public DbSet<Tag> Tags { get; set; } = null!;
		public DbSet<Book> Books { get; set; } = null!;
		public DbSet<BookCopy> BookCopies { get; set; } = null!;
		public DbSet<Loan> Loans { get; set; } = null!;
		public DbSet<SystemSettings> SystemSettings { get; set; } = null!;

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlite(ConnectionString);
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Reader>().ToTable("Readers");
			modelBuilder.Entity<Author>().ToTable("Authors");

			modelBuilder.Entity<SystemSettings>().HasData(
				new SystemSettings
				{
					Id = 1,
					MaxBooksPerReader = 3,
					ShortLoanDays = 14,
					LongLoanDays = 28,
					BanDaysForShortLoan = 7,
					PenaltyBaseAmount = 1.0m,
					PenaltyPercent = 0.03m,
					NewBookAgeThreshold = 1,
					RareBookCopiesThreshold = 5
				}
			);
		}
	}
}