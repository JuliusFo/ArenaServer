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
            var result = new List<TransferPokemon>();
            int maxLoop = 100;
            Random rnd = new Random();

            var entryList = transferTwitchuser.CatchedPokemonList.Where(c => c.AmountCatched > 0).ToList();

            for (int i = 0; i < maxLoop && result.Count != 6; i++)
            {
                var entry = entryList[rnd.Next(0, entryList.Count)];
                var resultEntry = result.Where(r => r == entry.Pokemon).ToList();

                if (resultEntry.Any())
                {
                    if(resultEntry.Count() < entry.AmountCatched)
                    {
                        result.Add(entry.Pokemon.Copy());
                    }
                }
                else
                {
                    result.Add(entry.Pokemon.Copy());
                }
            }
            return result;
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
            return transferTwitchuser.CatchedPokemonList.Select(c => c.AmountCatched).Sum() >= 6;
        }
    }
}