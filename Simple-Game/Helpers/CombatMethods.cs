using System;
using RogueSharp.Random;
using SimpleGame.Enumerators;
using SimpleGame.Models.Entities;

namespace SimpleGame.Helpers
{
    public static class CombatMethods
    {
        public static IRandom Random { get; set; }

        static CombatMethods()
        {
            Random = new DotNetRandom();
        }

        public static string MakeMeleeAttack(this BaseEntity attacker, BaseEntity defender)
        {
            attacker.AdjustTimer(1000);
            if (attacker.TryToHit(defender, CombatType.Melee))
            {
                return defender.TakeDamage(attacker.CalculateDamageOn(defender), attacker.Name);
            }
            else
            {
                return attacker.Name + " missed an attack on " + defender.Name;
            }
        }

        public static decimal CalculateDamageOn(this BaseEntity attacker, BaseEntity defender)
        {
            var randomModifier = Random.Next(80, 120);
            var attackDamage = attacker.WeaponDamage * (1 + attacker.Stats.AttackPower / 30m) * (randomModifier / 100m);
            var damageBlock = defender.ArmorBlock * (1 + defender.Stats.DefensePower / 30m);
            var dealtDamage = Math.Truncate((attackDamage - damageBlock) * 100) / 100;
            var minimumDamage = Math.Truncate(attackDamage * .05m * 100) / 100;
            return Math.Max(dealtDamage, minimumDamage);
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

        public static bool OnFriendlyTeam(this BaseEntity attacker, BaseEntity defender)
        {
            switch (attacker.EntityTeam)
            {
                case EntityTeam.Player:
                    switch (defender.EntityTeam)
                    {
                        case EntityTeam.Enemy:
                        case EntityTeam.EnemyHostileTowardOthers:
                        case EntityTeam.NeutralCanBeHit:
                            return false;

                        case EntityTeam.Player:
                        case EntityTeam.NeutralCantBeHit:
                            return true;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case EntityTeam.Enemy:
                    switch (defender.EntityTeam)
                    {
                        case EntityTeam.Player:
                        case EntityTeam.EnemyHostileTowardOthers:
                            return false;

                        case EntityTeam.Enemy:
                        case EntityTeam.NeutralCanBeHit:
                        case EntityTeam.NeutralCantBeHit:
                            return true;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case EntityTeam.NeutralCanBeHit:
                    switch (defender.EntityTeam)
                    {
                        case EntityTeam.EnemyHostileTowardOthers:
                            return false;

                        case EntityTeam.Enemy:
                        case EntityTeam.NeutralCanBeHit:
                        case EntityTeam.NeutralCantBeHit:
                        case EntityTeam.Player:
                            return true;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case EntityTeam.NeutralCantBeHit:
                    switch (defender.EntityTeam)
                    {
                        case EntityTeam.EnemyHostileTowardOthers:
                            return false;

                        case EntityTeam.Enemy:
                        case EntityTeam.NeutralCanBeHit:
                        case EntityTeam.NeutralCantBeHit:
                        case EntityTeam.Player:
                            return true;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case EntityTeam.EnemyHostileTowardOthers:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
