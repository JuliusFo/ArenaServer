using System;
using System.Collections.Generic;
using System.Linq;

namespace ArenaServer.Data.Transfer
{
    public static class TransferPokemonExtensions
    {
        public static TransferPokemon GetRandomPokemon(this List<TransferPokemon> transferPokemonList)
        {
            Random rndGen = new Random();

            if (transferPokemonList.Any())
            {
                return transferPokemonList[rndGen.Next(0, transferPokemonList.Count)];
            }
            else
            {
                return null;
            }
        }

        public static TransferPokemon Copy(this TransferPokemon original)
        {
            var result = new TransferPokemon()
            {
                ATK = original.ATK,
                Description = original.Description,
                HP = original.HP,
                Name = original.Name,
                Rarity = original.Rarity,
                Type = original.Type,
                ID = original.ID

            };

            return result;
        }
    }
}