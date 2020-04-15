using ArenaServer.Data.Common.Models;
using ArenaServer.Data.Transfer;
using System;
using System.Threading.Tasks;

namespace ArenaServer.Services
{
    public class UserfightRound
    {
        #region Fields

        private readonly DateTimeOffset createdDt;
        private readonly Random randomGenerator;
        private readonly UserService userService;
        private readonly bool isSelectedFight;
        private readonly bool withExchange;

        #endregion

        #region Constructor

        public UserfightRound(UserService userService, TransferTwitchuser attacker, TransferTwitchuser defender, bool withExchange, bool isSelectedFight = false)
        {
            this.Attacker = attacker;
            this.Defender = defender;

            this.createdDt = DateTimeOffset.Now;
            this.randomGenerator = new Random();
            this.userService = userService;
            this.isSelectedFight = isSelectedFight;
            this.withExchange = withExchange;
        }

        #endregion

        #region Properties

        public TransferTwitchuser Attacker { get; set; }

        public TransferTwitchuser Defender { get; set; }

        public DateTimeOffset CreatedDt
        {
            get
            {
                return createdDt;
            }
        }

        #endregion

        #region Methods

        public async Task<FightResult> Fight()
        {
            FightParticipant attacker;
            FightParticipant defender;

            if (isSelectedFight)
            {
                attacker = Attacker.ToFightParticipantSelectedFightTeam();
                defender = Defender.ToFightParticipantSelectedFightTeam();
            }
            else
            {
                attacker = Attacker.ToFightParticipantRandomFightTeam();
                defender = Defender.ToFightParticipantRandomFightTeam();
            }

            //Determinate the Pokemon the users gonna exchange if the lose.
            TransferPokemon exchangePokemonATK = attacker.Pokemon[randomGenerator.Next(0, 6)];
            TransferPokemon exchangePokemonDEF = defender.Pokemon[randomGenerator.Next(0, 6)];

            Fight fight = new Fight();
            FightOptions fightOptions = new FightOptions(attacker, defender, true);

            var result = fight.CalculateUnlimited(fightOptions);

            if (result.Winner.Equals(Attacker))
            {
                if(withExchange) await ExchangePokemon(true, exchangePokemonDEF);
                return new FightResult(result.Winner, exchangePokemonDEF);
            }
            else
            {
                if (withExchange) await ExchangePokemon(false, exchangePokemonATK);
                return new FightResult(result.Winner, exchangePokemonATK);
            }
        }

        private async Task ExchangePokemon(bool attackerWon, TransferPokemon exchange)
        {
            if (attackerWon)
            {
                await userService.RemovePokemon(Defender.Id, exchange);
                await userService.AddPokemon(Attacker.Id, exchange, false);
            }
            else
            {
                await userService.RemovePokemon(Attacker.Id, exchange);
                await userService.AddPokemon(Defender.Id, exchange, false);
            }
        }

        #endregion
    }
}