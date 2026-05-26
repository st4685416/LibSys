namespace LibSys.Models
{
	public enum BookCopyStatus
	{
		Available,
		Loaned,
		Lost,
		WrittenOff
	}

	public class BookCopy
	{
		public int Id { get; set; } // Унікальний інвентарний номер
		public int BookId { get; set; } // Унікальний книжний номер
		public DateTime ArrivalDate { get; set; }
		public int StateScore { get; set; } = 100;
		public BookCopyStatus Status { get; set; } = BookCopyStatus.Available;

		public virtual Book Book { get; set; } = null!;
		public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
	}
}