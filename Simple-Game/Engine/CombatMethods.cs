using System;
using RogueSharp.Random;
using SimpleGame.Enumerators;
using SimpleGame.Models;

namespace SimpleGame.Engine
{
    public static class CombatMethods
    {
        public static IRandom Random { get; set; }

        static CombatMethods()
        {
            Random = new DotNetRandom();
        }

        public static void MakeMeleeAttack(this BaseEntity attacker, BaseEntity defender)
        {
            if (attacker.TryToHit(defender, CombatType.Melee))
            {
                defender.TakeDamage(attacker.CalculateDamageOn(defender), attacker.Name);
            }
            else
            {
                Console.WriteLine(attacker.Name + " missed an attack on " + defender.Name);
            }
        }

        public static decimal CalculateDamageOn(this BaseEntity attacker, BaseEntity defender)
        {
            var randomModifier = Random.Next(80, 120);
            var attackDamage = attacker.WeaponDamage * (1 + (decimal)attacker.Stats.AttackPower / 30) * ((decimal)randomModifier / 100);
            var damageBlock = defender.ArmorBlock * (1 + (decimal)defender.Stats.DefensePower / 30);
            return Math.Max(attackDamage - damageBlock, attackDamage * (decimal).05);
        }

        public static bool TryToHit(this BaseEntity attacker, BaseEntity defender, CombatType range)
        {
            switch (range)
            {
                case CombatType.Melee:
                    return TryToHitMelee(attacker, defender);

                case CombatType.Ranged:
                    return TryToHitRanged(attacker, defender);

                default:
                    throw new ArgumentOutOfRangeException("range");
            }
        }

        private static bool TryToHitRanged(BaseEntity attacker, BaseEntity defender)
        {
            var accuracy = attacker.Stats.AccuracyChance;
            var dodge = defender.Stats.DodgeChance;
            while (true)
            {
                var roll = Random.Next(1, 10000);
                if (accuracy >= (decimal) roll/10000)
                {
                    var roll2 = Random.Next(1, 10000);
                    if (dodge < (decimal) roll2/10000)
                    {
                        return true;
                    }
                    else
                    {
                        dodge *= (decimal).9;
                        accuracy *= (decimal) .9;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        private static bool TryToHitMelee(BaseEntity attacker, BaseEntity defender)
        {
            var roll = Random.Next(1, 10000);
            if (defender.Stats.DodgeChance >= (decimal) roll/10000)
            {
                var roll2 = Random.Next(1, 10000);
                return attacker.Stats.AccuracyChance >= (decimal) roll2/10000;
            }
            else
            {
                return true;
            }
        }
    }
}
