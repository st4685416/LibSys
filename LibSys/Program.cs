using LibSys.Data;

namespace LibSys
{
	internal static class Program
	{
		[STAThread]
		static void Main()
		{
			ApplicationConfiguration.Initialize();
			using (var context = new LibraryContext())
			{
				context.Database.EnsureCreated();
			}
			Application.Run(new MainForm());
		}
	}
}
