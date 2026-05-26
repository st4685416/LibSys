namespace LibSys.Models
{
	public class Reader : Person
	{
		public string PassportNumber { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;
		public string? Email { get; set; }
		public long BirthDate { get; set; }
	}
}