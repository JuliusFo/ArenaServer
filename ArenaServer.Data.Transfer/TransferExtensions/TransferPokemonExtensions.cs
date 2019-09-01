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
    }
}