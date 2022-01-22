using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;

namespace ArenaServer.Data
{
	public sealed class AppDbContextFactory : IAppDbContextFactory
	{
		private readonly DbContextOptions options;

		public AppDbContextFactory()
		{
			options = new DbContextOptionsBuilder()
				.UseSqlServer(GetDatabaseConnection())
				.UseLazyLoadingProxies()
				.Options;
		}

		public AppDbContextFactory(DbContextOptions options)
		{
			this.options = options;
		}

		public AppDbContext Create()
		{
			return new AppDbContext(options);
		}

		public string GetDatabaseConnection()
		{
			var fileStream = new FileStream("E:/Programmierung/Temp/Connections.ini", FileMode.Open, FileAccess.Read);
			string text;

			using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
			{
				text = streamReader.ReadToEnd();
			}

			return text.Split('#')[2];
		}
	}
}