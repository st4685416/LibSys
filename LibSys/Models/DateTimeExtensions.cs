namespace LibSys.Models
{
	public static class DateTimeExtensions
	{
		public static long ToUnixTimestamp(this DateTime dateTime)
		{
			return (long)((DateTimeOffset)dateTime).ToUnixTimeSeconds();
		}

		public static DateTime FromUnixTimestamp(this long timestamp)
		{
			return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
		}
	}
}