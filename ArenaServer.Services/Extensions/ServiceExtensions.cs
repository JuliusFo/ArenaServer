using ArenaServer.Data.Common.Models;
using ArenaServer.Data.Transfer;
using System.Collections.Generic;

namespace ArenaServer.Services.Extensions
{
    public static class ServiceExtensions
    {
        public static FightParticipant ToFightParticipantRandomPokemon(this TransferTwitchuser user)
        {
            return new FightParticipant()
            {
                User = user,
                Pokemon = new List<TransferPokemon>() { user.GetRandomPokemon() }
            };
        }

        public static FightParticipant ToFightParticipantSelectedFightTeam(this TransferTwitchuser user)
        {
            return new FightParticipant()
            {
                User = user,
                Pokemon = user.GetSelectedTeam()
            };
        }
    }
}