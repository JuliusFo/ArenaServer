using ArenaServer.Data.Models;
using ArenaServer.Data.Transfer;

namespace ArenaServer.Services
{
    public static class PokemonServiceExtensions
    {
        public static TransferPokemon ConvertSdPokemonToTransfer(this SdPokemon entity)
        {
            return new TransferPokemon()
            {
                Name = entity.Name,
                Description = entity.Description,
                HP = entity.HP,
                Rarity = entity.Rarity,
                Type = entity.Type,
                ATK = entity.ATK,
                ID = entity.SdPokemon_Id
            };
        }

        public static TransferCatchedPokemon ConvertToCatchedPokemon(this TransferPokemon item)
        {
            return new TransferCatchedPokemon()
            {
                AmountCatched = 1,
                AmountOnFightingTeam = 1,
                Pokemon = item
            };
        }
    }
}