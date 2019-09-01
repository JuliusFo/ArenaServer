using ArenaServer.Data;
using ArenaServer.Data.Models;
using ArenaServer.Data.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArenaServer.Services
{
    public class PokemonService
    {
        #region Fields

        private readonly AppDbContext db;

        #endregion

        #region Constructor

        public PokemonService(AppDbContext db)
        {
            this.db = db;
        }

        #endregion

        #region Properties



        #endregion

        #region Methods

        public TransferPokemon GetTransferPokemonFromName(string pokemonName)
        {
            if (string.IsNullOrWhiteSpace(pokemonName)) return null;

            var pokemon = db.SdPokemon.Where(p => p.Name == pokemonName).FirstOrDefault();

            if (pokemon == null)
            {
                return null;
            }
            else
            {
                return ConvertSdPokemonToTransfer(pokemon);
            }
        }

        public decimal? GetSdPokemonIdFromName(string pokemonName)
        {
            return db.SdPokemon.Where(p => p.Name == pokemonName).FirstOrDefault()?.SdPokemon_Id;
        }

        public TransferPokemon GetRandomPokemon()
        {
            Random rnd = new Random();

            return ConvertSdPokemonToTransfer(db.SdPokemon.Where(p => p.SdPokemon_Id == rnd.Next(1, 152)).First());
        }

        public TransferPokemon GetRandomPokemonWithRarity(PokemonRarity rarity)
        {
            Random rnd = new Random();

            var pokemonWithRarity = db.SdPokemon.Where(p => p.Rarity == rarity).ToList();

            return ConvertSdPokemonToTransfer(pokemonWithRarity[rnd.Next(0, pokemonWithRarity.Count)]);
        }

        public TransferPokemon GetRandomPokemonWithParticipantCount(int participantCount)
        {
            Dictionary<PokemonRarity, int> rarityChances = new Dictionary<PokemonRarity, int>();
            Random rnd = new Random();

            if (participantCount >= 15)
            {
                rarityChances.Add(PokemonRarity.Legendary, 8);
                rarityChances.Add(PokemonRarity.Ultrarare, 25);
                rarityChances.Add(PokemonRarity.Rare, 37);
                rarityChances.Add(PokemonRarity.Common, 30);
            }
            else
            {
                rarityChances.Add(PokemonRarity.Legendary, 5 + Convert.ToInt32(participantCount * 0.2));
                rarityChances.Add(PokemonRarity.Ultrarare, 20 + Convert.ToInt32(participantCount * 0.2));
                rarityChances.Add(PokemonRarity.Rare, 30 + Convert.ToInt32(participantCount * 0.2));
                rarityChances.Add(PokemonRarity.Common, 45 - 3 * (Convert.ToInt32(participantCount * 0.2)));
            }

            rarityChances.TryGetValue(PokemonRarity.Common, out int commonChance);
            rarityChances.TryGetValue(PokemonRarity.Rare, out int rareChance);
            rarityChances.TryGetValue(PokemonRarity.Ultrarare, out int ultrarareChance);
            rarityChances.TryGetValue(PokemonRarity.Legendary, out int legendaryChance);

            int percent = rnd.Next(1, 101);

            if (percent < legendaryChance)
            {
                return GetRandomPokemonWithRarity(PokemonRarity.Legendary);
            }
            else if (percent < (legendaryChance + ultrarareChance))
            {
                return GetRandomPokemonWithRarity(PokemonRarity.Ultrarare);
            }
            else if (percent < (legendaryChance + ultrarareChance + rareChance))
            {
                return GetRandomPokemonWithRarity(PokemonRarity.Rare);
            }
            else if (percent < (legendaryChance + ultrarareChance + rareChance + commonChance))
            {
                return GetRandomPokemonWithRarity(PokemonRarity.Common);
            }

            return GetRandomPokemon();
        }

        public static float GetTypeAdvantageMultiplikator(PokemonType Attacker, PokemonType Defender)
        {
            if (Attacker == PokemonType.Normal)
            {
                switch (Defender)
                {
                    case PokemonType.Rock: return 0.75f;
                    case PokemonType.Ghost: return 0.75f;
                    default: return 1;
                }
            }

            if (Attacker == PokemonType.Fight)
            {
                switch (Defender)
                {
                    case PokemonType.Normal: return 1.25f;
                    case PokemonType.Flying: return 0.75f;
                }
            }

            return 1;
        }

        #region Converting

        public TransferPokemon ConvertSdPokemonToTransfer(SdPokemon entity)
        {
            return new TransferPokemon()
            {
                Name = entity.Name,
                Description = entity.Description,
                HP = entity.HP,
                Rarity = entity.Rarity,
                Type = entity.Type
            };
        }

        #endregion

        #endregion
    }
}