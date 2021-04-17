using ArenaServer.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ArenaServer.Services
{
	public class SettingsService
	{
		#region Fields

		private readonly AppDbContext db;

		#endregion

		#region Constructor

		public SettingsService(AppDbContext db)
		{
			this.db = db;
		}

		#endregion

		#region Properties



		#endregion

		#region Methods

		#region Global



		#endregion

		#region Boss

		public async Task<int> GetIntegerSetting(string settingName)
		{
			var result = await db.SdSettings.Where(s => s.Name == settingName).Select(r => r.Value).FirstOrDefaultAsync();

			return int.Parse(result);
		}

		public async Task<string> GetTextSetting(string settingName)
		{
			var result = await db.SdSettings.Where(s => s.Name == settingName).Select(r => r.Value).FirstOrDefaultAsync();

			return result;
		}

		public async Task<bool> GetBoolSetting(string settingName)
		{
			var result = await db.SdSettings.Where(s => s.Name == settingName).Select(r => r.Value).FirstOrDefaultAsync();

			return bool.Parse(result);
		}

		public async Task<float> GetFloatSetting(string settingName)
		{
			var result = await db.SdSettings.Where(s => s.Name == settingName).Select(r => r.Value).FirstOrDefaultAsync();

			return float.Parse(result);
		}

		#endregion

		#endregion
	}
}