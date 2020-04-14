using ArenaServer.Data.Common.Models;
using ArenaServer.Data.Transfer;
using ArenaServer.Services.Models;
using System;
using System.Linq;

namespace ArenaServer.Services
{
    public class Fight
    {
        #region Fields

        private readonly Random randomGenerator;

        #endregion

        #region Constructor

        public Fight()
        {
            this.randomGenerator = new Random();
        }

        #endregion

        #region Properties



        #endregion

        #region Methods

        public FightResult CalculateUnlimited(FightOptions input)
        {
            var challenger = input.Challenger;
            var defender = input.Defender;

            var exchange_challenger = challenger.Pokemon.GetRandomPokemon().Copy();
            var exchange_defender = defender.Pokemon.GetRandomPokemon().Copy();

            //Fight until there are no Pokemon left.
            while (true)
            {
                //Step 1 - Check if the trainers still got Pokemon left
                if (defender.Pokemon.FirstOrDefault() == null)
                {
                    return new FightResult(challenger.User, input.Kz_WithPokemonExchange ? exchange_defender : null);
                }
                if (challenger.Pokemon.FirstOrDefault() == null)
                {
                    return new FightResult(defender.User, input.Kz_WithPokemonExchange ? exchange_challenger : null);
                }

                //Step 2 - Calculate next fight round
                var result = CalulateOneVSOne(input);

                if (result.Winner.User.Equals(defender.User))
                {
                    challenger.Pokemon.RemoveAt(0);
                }
                else
                {
                    defender.Pokemon.RemoveAt(0);
                }
            }
        }

        private FightRoundResult CalulateOneVSOne(FightOptions input)
        {
            bool challengerAttackFirst = randomGenerator.Next(0, 2) == 0;
            var challenger = input.Challenger;
            var defender = input.Defender;

            while (((challenger.Pokemon.FirstOrDefault()?.HP ?? 0) > 0) && (defender.Pokemon.FirstOrDefault()?.HP ?? 0) > 0)
            {
                var defender_pokemon = defender.Pokemon[0];
                var attacker_pokemon = challenger.Pokemon[0];

                if (challengerAttackFirst)
                {
                    var damage = input.ParticipantBonus + attacker_pokemon.ATK * PokemonService.GetTypeAdvantageMultiplikator(attacker_pokemon.Type, defender_pokemon.Type) + randomGenerator.Next(-2, 3);
                    defender_pokemon.HP -= damage;

                    if (defender_pokemon.HP <= 0) return new FightRoundResult(challenger);

                    damage = defender_pokemon.ATK * PokemonService.GetTypeAdvantageMultiplikator(defender_pokemon.Type, attacker_pokemon.Type) + randomGenerator.Next(-2, 3);
                    attacker_pokemon.HP -= damage;

                    if (attacker_pokemon.HP <= 0) return new FightRoundResult(defender);
                }
                else
                {
                    var damage = defender_pokemon.ATK * PokemonService.GetTypeAdvantageMultiplikator(defender_pokemon.Type, attacker_pokemon.Type) + randomGenerator.Next(-2, 3);
                    attacker_pokemon.HP -= damage;

                    if (attacker_pokemon.HP <= 0) return new FightRoundResult(defender);

                    damage = attacker_pokemon.ATK * PokemonService.GetTypeAdvantageMultiplikator(attacker_pokemon.Type, defender_pokemon.Type) + randomGenerator.Next(-2, 3);
                    defender_pokemon.HP -= damage;

                    if (defender_pokemon.HP <= 0) return new FightRoundResult(challenger);
                }
            }

            return null;
        }

        #endregion
    }
}