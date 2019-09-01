using System;
using System.Linq;

namespace ArenaServer.Data.Transfer
{
    public static class TransferTwitchuserExtensions
    {
        public static TransferPokemon GetRandomPokemon(this TransferTwitchuser transferTwitchuser)
        {
            Random rndGen = new Random();

            if (transferTwitchuser.CatchedPokemonList.Any())
            {
                return transferTwitchuser.CatchedPokemonList[rndGen.Next(0, transferTwitchuser.CatchedPokemonList.Count)].Pokemon;
            }
            else
            {
                return null;
            }
        }
    }
}