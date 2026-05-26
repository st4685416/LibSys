namespace LibSys.Models
{
	public static class BookCopyStatusExtensions
	{
		public static string ToLocalizedString(this BookCopyStatus status) => status switch
		{
			BookCopyStatus.Available => $"{UIIcons.Available} Доступна",
			BookCopyStatus.Loaned => $"{UIIcons.Loaned} На руках",
			BookCopyStatus.Lost => $"{UIIcons.Lost} Втрачена",
			BookCopyStatus.WrittenOff => $"{UIIcons.WrittenOff} Списана",
			_ => "Невідомо"
		};
	}
}