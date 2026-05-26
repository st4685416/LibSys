namespace LibSys.Models
{
	public class SystemSettings
	{
		public int Id { get; set; } // Завжди буде 1
		public int MaxBooksPerReader { get; set; } = 3;
		public int ShortLoanDays { get; set; } = 14;
		public int LongLoanDays { get; set; } = 28;
		public int BanDaysForShortLoan { get; set; } = 7;
		public decimal PenaltyBaseAmount { get; set; } = 1.0m;
		public decimal PenaltyPercent { get; set; } = 0.03m;
		public int NewBookAgeThreshold { get; set; } = 1;
		public int RareBookCopiesThreshold { get; set; } = 5;
	}
}