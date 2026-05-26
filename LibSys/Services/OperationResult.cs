namespace LibSys.Services
{
	// =====================================================================
	// ПАТЕРН RESULT
	// Дозволяє сервісам повертати статус виконання (успіх/помилка) і супутнє повідомлення.
	// =====================================================================
	public class OperationResult
	{
		public bool IsSuccess { get; }
		public string Message { get; }

		private OperationResult(bool isSuccess, string message)
		{
			IsSuccess = isSuccess;
			Message = message;
		}

		public static OperationResult Success(string message = "") => new OperationResult(true, message);
		public static OperationResult Failure(string message) => new OperationResult(false, message);
	}
}