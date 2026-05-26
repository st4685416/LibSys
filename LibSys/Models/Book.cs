namespace LibSys.Models
{
	public class Book
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public int PublicationYear { get; set; }
		public decimal Price { get; set; }
		public int PublisherId { get; set; }
		public byte[]? CoverImage { get; set; }

		public bool IsArchived { get; set; } = false;

		public virtual Publisher Publisher { get; set; } = null!;
		public virtual ICollection<Author> Authors { get; set; } = new List<Author>();
		public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
		public virtual ICollection<BookCopy> BookCopies { get; set; } = new List<BookCopy>();
	}
}