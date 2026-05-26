namespace LibSys.Models
{
	public abstract class Person
	{
		public int Id { get; set; }
		public string? LastName { get; set; }
		public string? FirstName { get; set; }
		public string? MiddleName { get; set; }

		public string FullName => string.Join(" ",
			new[] { LastName, FirstName, MiddleName }.Where(s => !string.IsNullOrWhiteSpace(s)));
	}
}