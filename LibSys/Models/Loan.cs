namespace LibSys.Models
{
	public class Loan
	{
		public int Id { get; set; }
		public int ReaderId { get; set; }
		public int BookCopyId { get; set; }

		public DateTime IssueDate { get; set; }
		public DateTime ExpectedReturnDate { get; set; }
		public DateTime? ActualReturnDate { get; set; }

		public bool IsLost { get; set; } = false;
		public DateTime? PaymentDate { get; set; }
		public bool IsLossPaid { get; set; } = true;

		// Знімки фінансових умов на момент видачі (захист від зміни глобальних налаштувань)
		public decimal FixedPrice { get; set; }
		public decimal FixedPenaltyBaseAmount { get; set; }
		public decimal FixedPenaltyPercent { get; set; }

		public virtual Reader Reader { get; set; } = null!;
		public virtual BookCopy BookCopy { get; set; } = null!;


		public decimal CalculateCurrentDebt()
		{
			if (IsLossPaid) return 0m;

			decimal dailyPenalty = FixedPenaltyBaseAmount + (FixedPrice * FixedPenaltyPercent);
			int overdueDays;

			if (IsLost)
			{
				DateTime endDate = PaymentDate ?? DateTime.Now;
				overdueDays = (endDate.Date - ExpectedReturnDate.Date).Days;
				if (overdueDays <= 0)
					return FixedPrice;
				return FixedPrice + (overdueDays * dailyPenalty);
			}

			if (ActualReturnDate.HasValue)
			{
				overdueDays = (ActualReturnDate.Value.Date - ExpectedReturnDate.Date).Days;
				if (overdueDays <= 0)
					return 0m;
				return overdueDays * dailyPenalty;
			}

			overdueDays = (DateTime.Now.Date - ExpectedReturnDate.Date).Days;
			if (overdueDays <= 0)
				return 0m;
			return overdueDays * dailyPenalty;
		}
	}
}