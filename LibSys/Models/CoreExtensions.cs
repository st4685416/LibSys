namespace LibSys.Models
{
	public static class StringSearchExtensions
	{
		public static bool ContainsAllSearchTerms(this string source, string searchQuery)
		{
			if (string.IsNullOrWhiteSpace(searchQuery)) return true;
			if (string.IsNullOrWhiteSpace(source)) return false;

			var terms = searchQuery.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			return terms.All(term => source.Contains(term, StringComparison.OrdinalIgnoreCase));
		}
	}

	public enum CatalogSortOption
	{
		TitleAsc = 0,
		TitleDesc = 1,
		PublisherAsc = 2,
		PublisherDesc = 3,
		YearNewest = 4,
		YearOldest = 5
	}

	public enum InventorySortOption
	{
		StateWorst = 0,
		StateBest = 1,
		DateNewest = 2,
		DateOldest = 3
	}
}