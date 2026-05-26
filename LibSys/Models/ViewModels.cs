namespace LibSys.Models
{
	// DTO для Каталогу
	public class CatalogItemDto
	{
		public int ID { get; set; }
		public string Назва { get; set; } = string.Empty;
		public string Видавництво { get; set; } = string.Empty;
		public int Рік { get; set; }
		public string Автори { get; set; } = string.Empty;
		public string Теги { get; set; } = string.Empty;
	}

	// DTO для Інвентарю
	public class InventoryItemDto
	{
		public int ID_Книги { get; set; }
		public int Інв_Номер { get; set; }
		public string Назва { get; set; } = string.Empty;
		public string Автор { get; set; } = string.Empty;
		public string Видавництво { get; set; } = string.Empty;
		public int Рік { get; set; }
		public string Стан { get; set; } = string.Empty;
		public string Статус { get; set; } = string.Empty;
		public string Надходження { get; set; } = string.Empty;
	}

	// DTO для Читачів
	public class ReaderItemDto
	{
		public int ID { get; set; }
		public string Прізвище { get; set; } = string.Empty;
		public string Ім_я { get; set; } = string.Empty;
		public string По_батькові { get; set; } = string.Empty;
		public string Народження { get; set; } = string.Empty;
		public string Паспорт { get; set; } = string.Empty;
		public string Телефон { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
	}

	// DTO для вікна видачі
	public class AvailableCopyDto
	{
		public int Інв_Номер { get; set; }
		public string Назва { get; set; } = string.Empty;
		public string Автор { get; set; } = string.Empty;
		public string Видавництво { get; set; } = string.Empty;
		public int Рік { get; set; }
		public int Стан { get; set; }
	}

	public class JournalItemDto
	{
		public string Дата { get; set; } = string.Empty;
		public string Книга { get; set; } = string.Empty;
		public string Автори { get; set; } = string.Empty;
		public string Читач { get; set; } = string.Empty;
		public string Операція { get; set; } = string.Empty;

		// Приховані поля для логіки та сортування
		public DateTime DateValue { get; set; }
		public int ReaderId { get; set; }
	}
}