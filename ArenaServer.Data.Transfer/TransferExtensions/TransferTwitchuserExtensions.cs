using System;
using System.Collections.Generic;
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

        public static List<TransferPokemon> GetRandomTeam(this TransferTwitchuser transferTwitchuser)
        {
            return transferTwitchuser.CatchedPokemonList.Where(p => p.AmountCatched > 0).Take(6).Select(p => p.Pokemon).ToList();
        }

        public static List<TransferPokemon> GetSelectedTeam(this TransferTwitchuser transferTwitchuser)
        {
            var selected_pkm = transferTwitchuser.CatchedPokemonList.Where(p => p.AmountOnFightingTeam > 0).Select(c => c.Pokemon).ToList();

            //TODO: Hier muss noch logik rein:
            //1. Versuchen einzelne Pokemon zu nehmen
            //2. Max 6
            //3. Wenn 1. nicht klappt dann auffüllen wo Amount > 1

            return selected_pkm;
        }

        public static bool HasFullFightingTeam(this TransferTwitchuser transferTwitchuser)
        {
            return transferTwitchuser.CatchedPokemonList.Select(c => c.AmountCatched).Sum() > 6;
        }
    }
}