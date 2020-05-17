using ArenaServer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var setting_name = "BossPauseSecondsNotEnoughParticipants";
            var result = await db.SdSettings.Where(s => s.Name == setting_name).FirstOrDefaultAsync();

            return int.Parse(result.Value);
        }

        public async Task<string> GetTextSetting(string settingName)
        {
            var setting_name = "BossPauseSecondsNotEnoughParticipants";
            var result = await db.SdSettings.Where(s => s.Name == setting_name).FirstOrDefaultAsync();

            return result.Value;
        }

        public async Task<bool> GetBoolSetting(string settingName)
        {
            var setting_name = "BossPauseSecondsNotEnoughParticipants";
            var result = await db.SdSettings.Where(s => s.Name == setting_name).FirstOrDefaultAsync();

            return bool.Parse(result.Value);
        }

        public async Task<float> GetFloatSetting(string settingName)
        {
            var setting_name = "BossPauseSecondsNotEnoughParticipants";
            var result = await db.SdSettings.Where(s => s.Name == setting_name).FirstOrDefaultAsync();

            return float.Parse(result.Value);
        }

        #endregion

        #endregion
    }
}