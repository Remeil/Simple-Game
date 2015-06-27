using System;
using RogueSharp.Random;
using SimpleGame.Entities;

namespace SimpleGame.Engine
{
    public static class CombatMethods
    {
        public static IRandom Random { get; set; }

        static CombatMethods()
        {
            Random = new DotNetRandom();
        }

        public static decimal CalculateDamageOn(this BaseEntity attacker, BaseEntity defender)
        {
            var randomModifier = Random.Next(80, 120);
            var attackDamage = attacker.WeaponDamage*(1 + (decimal) attacker.Stats.AttackPower/30)*((decimal)randomModifier / 100);
            var damageBlock = defender.ArmorBlock*(1 + (decimal) defender.Stats.DefensePower/30);
            return Math.Max(attackDamage - damageBlock, attackDamage * (decimal).05);
        }
    }
}
