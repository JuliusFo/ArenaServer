using ArenaServer.Data.Common.Models;
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

        public FightParticipant CalulateOneVSOne(FightParticipant challenger, FightParticipant defender)
        {
            bool challengerAttackFirst = randomGenerator.Next(0, 2) == 0;

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

        public FightParticipant CalculateUnlimited(FightParticipant challenger, FightParticipant defender)
        {
            //Fight until there are no Pokemon left.
            while (true)
            {
                FightParticipant lastResult = null;

                //Step 1 - Check if the trainers still got Pokemon left
                if (defender.Pokemon.FirstOrDefault() == null)
                {
                    //ExchangePokemon(false, exchangePokemonATK);
                    return challenger;
                }
                if (challenger.Pokemon.FirstOrDefault() == null)
                {
                    //ExchangePokemon(true, exchangePokemonDEF);
                    return defender;
                }

                //Step 2 - Calculate next fight round
                lastResult = CalulateOneVSOne(challenger, defender);
                if (lastResult.Equals(defender))
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
