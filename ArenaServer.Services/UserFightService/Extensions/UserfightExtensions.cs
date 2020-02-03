using ArenaServer.Data.Common.Models;
using ArenaServer.Data.Transfer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArenaServer.Services
{
    public static class UserfightExtensions
    {
        public static FightParticipant ToFightParticipantSelectedFightTeam(this TransferTwitchuser user)
        {
            return new FightParticipant()
            {
                User = user,
                Pokemon = user.GetSelectedTeam()
            };
        }

        public static FightParticipant ToFightParticipantRandomFightTeam(this TransferTwitchuser user)
        {
            return new FightParticipant()
            {
                User = user,
                Pokemon = user.GetRandomTeam()
            };
        }

        public static FightParticipant ToFightParticipantRandomPokemon(this TransferTwitchuser user)
        {
            return new FightParticipant()
            {
                User = user,
                Pokemon = new List<TransferPokemon>() { user.GetRandomPokemon() }
            };
        }
    }
}
