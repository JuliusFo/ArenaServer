using ArenaServer.Data.Common.Models;
using ArenaServer.Data.Transfer;
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

        public FightParticipant CalulateOneVSOne(FightOptions input)
        {
            bool challengerAttackFirst = randomGenerator.Next(0, 2) == 0;
            var challenger = input.Challenger;
            var defender = input.Defender;

            while (((challenger.Pokemon.FirstOrDefault()?.HP ?? 0) > 0) && (defender.Pokemon.FirstOrDefault()?.HP ?? 0) > 0)
            {
                var defender_pokemon = defender.Pokemon.FirstOrDefault();
                var attacker_pokemon = defender.Pokemon.FirstOrDefault();

                if (challengerAttackFirst)
                {
                    var damage = attacker_pokemon.ATK * PokemonService.GetTypeAdvantageMultiplikator(attacker_pokemon.Type, defender_pokemon.Type) + randomGenerator.Next(-2, 3);
                    defender_pokemon.HP -= damage;

                    if (defender_pokemon.HP <= 0) return challenger;

                    damage = defender_pokemon.ATK * PokemonService.GetTypeAdvantageMultiplikator(defender_pokemon.Type, attacker_pokemon.Type) + randomGenerator.Next(-2, 3);
                    attacker_pokemon.HP -= damage;

                    if (attacker_pokemon.HP <= 0) return defender;
                }
                else
                {
                    var damage = defender_pokemon.ATK * PokemonService.GetTypeAdvantageMultiplikator(defender_pokemon.Type, attacker_pokemon.Type) + randomGenerator.Next(-2, 3);
                    attacker_pokemon.HP -= damage;

                    if (attacker_pokemon.HP <= 0) return defender;

                    damage = attacker_pokemon.ATK * PokemonService.GetTypeAdvantageMultiplikator(attacker_pokemon.Type, defender_pokemon.Type) + randomGenerator.Next(-2, 3);
                    defender_pokemon.HP -= damage;

                    if (defender_pokemon.HP <= 0) return challenger;
                }
            }

            return null;
        }

        public FightResult CalculateUnlimited(FightOptions input)
        {
            var challenger = input.Challenger;
            var defender = input.Defender;

            var exchange_challenger = challenger.Pokemon.GetRandomPokemon();
            var exchange_defender = defender.Pokemon.GetRandomPokemon();

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
                if (result.Equals(defender))
                {
                    challenger.Pokemon.RemoveAt(0);
                }
                else
                {
                    defender.Pokemon.RemoveAt(0);
                }
            }
        }

        #endregion
    }
}
