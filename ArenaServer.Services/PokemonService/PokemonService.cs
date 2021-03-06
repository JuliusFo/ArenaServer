﻿using ArenaServer.Data;
using ArenaServer.Data.Models;
using ArenaServer.Data.Transfer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

		public async Task<TransferPokemon> GetTransferPokemonFromName(string pokemonName)
		{
			if (string.IsNullOrWhiteSpace(pokemonName)) return null;

			var pokemon = await db.SdPokemon.Where(p => p.Name == pokemonName).Select(e => new SdPokemon()
			{
				SdPokemon_Id = e.SdPokemon_Id,
				ATK = e.ATK,
				Description = e.Description,
				HP = e.HP,
				Name = e.Name,
				Rarity = e.Rarity,
				Type = e.Type
			}).FirstOrDefaultAsync();

			if (pokemon == null)
			{
				return null;
			}
			else
			{
				return pokemon.ConvertSdPokemonToTransfer();
			}
		}

		public decimal? GetSdPokemonIdFromName(string pokemonName)
		{
			return db.SdPokemon.Where(p => p.Name == pokemonName).FirstOrDefault()?.SdPokemon_Id;
		}

		public async Task<TransferPokemon> GetRandomPokemon()
		{
			Random rnd = new Random();

			var pkm = await db.SdPokemon.Select(e => new SdPokemon()
			{
				SdPokemon_Id = e.SdPokemon_Id,
				ATK = e.ATK,
				Description = e.Description,
				HP = e.HP,
				Name = e.Name,
				Rarity = e.Rarity,
				Type = e.Type
			}).ToListAsync();

			return pkm[rnd.Next(0, pkm.Count)].ConvertSdPokemonToTransfer();
		}

		public async Task<TransferPokemon> GetRandomPokemonWithRarity(PokemonRarity rarity)
		{
			Random rnd = new Random();

			var pokemonWithRarity = await db.SdPokemon.Where(p => p.Rarity == rarity).Select(e => new SdPokemon()
			{
				SdPokemon_Id = e.SdPokemon_Id,
				ATK = e.ATK,
				Description = e.Description,
				HP = e.HP,
				Name = e.Name,
				Rarity = e.Rarity,
				Type = e.Type
			}).ToListAsync();

			if (pokemonWithRarity.Any())
			{
				return pokemonWithRarity[rnd.Next(0, pokemonWithRarity.Count)].ConvertSdPokemonToTransfer();
			}
			else
			{
				return await GetRandomPokemon();
			}
		}

		public async Task<TransferPokemon> GetRandomPokemonWithParticipantCount(int participantCount)
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
				return await GetRandomPokemonWithRarity(PokemonRarity.Legendary);
			}
			else if (percent < (legendaryChance + ultrarareChance))
			{
				return await GetRandomPokemonWithRarity(PokemonRarity.Ultrarare);
			}
			else if (percent < (legendaryChance + ultrarareChance + rareChance))
			{
				return await GetRandomPokemonWithRarity(PokemonRarity.Rare);
			}
			else if (percent < (legendaryChance + ultrarareChance + rareChance + commonChance))
			{
				return await GetRandomPokemonWithRarity(PokemonRarity.Common);
			}

			return await GetRandomPokemon();
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
					default: return 1;
				}
			}

			return 1;
		}

		#endregion
	}
}