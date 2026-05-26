namespace LibSys.Models
{
	public class Tag
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;

		public bool IsArchived { get; set; } = false;

		public virtual ICollection<Book> Books { get; set; } = new List<Book>();
	}
}