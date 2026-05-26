namespace LibSys.Models
{
	public class Author : Person
	{
		public string? Pseudonym { get; set; }
		public bool IsArchived { get; set; } = false;

		public virtual ICollection<Book> Books { get; set; } = new List<Book>();

		public string DisplayName
		{
			get
			{
				string fullName = FullName;
				if (!string.IsNullOrWhiteSpace(Pseudonym))
					return !string.IsNullOrWhiteSpace(fullName) ? $"{fullName} ({Pseudonym})" : Pseudonym;

				return string.IsNullOrWhiteSpace(fullName) ? "Невідомий автор" : fullName;
			}
		}
	}
}