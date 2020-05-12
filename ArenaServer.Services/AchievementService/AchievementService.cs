using ArenaServer.Data;
using ArenaServer.Data.Transfer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaServer.Services
{
    public class AchievementService
    {
        #region Fields

        private readonly AppDbContext db;

        #endregion

        #region Constructor

        public AchievementService(AppDbContext db)
        {
            this.db = db;
        }

        #endregion

        #region Properties



        #endregion

        #region Methods

        public async Task<TransferTwitchuser> CreateVirtualNPC(string achievementName)
        {
            TransferTwitchuser virtualNPC;

            var sdAchievement = await db.SdAchievement.Where(a => a.NPCName == achievementName).FirstOrDefaultAsync();
            var transferPokemonList = sdAchievement.SdAchievementPokemon.Select(a => a.SdPokemon).Select(p => p.ConvertSdPokemonToTransfer());

            virtualNPC = new TransferTwitchuser()
            {
                DisplayName = sdAchievement.NPCName,
                KzLogEnabled = false,
                Id = "-1NPC",
                LastUserFight = DateTime.Now,
                CatchedPokemonList = transferPokemonList.Select(t => t.ConvertToCatchedPokemon()).ToList()
            };

            return virtualNPC;
        }

        #endregion
    }
}