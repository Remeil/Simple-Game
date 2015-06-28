using System;
using NUnit.Framework;
using RogueSharp.Random;
using SimpleGame.Engine;
using SimpleGame.Entities;

namespace SimpleGameTests.EngineTests
{
    [TestFixture]
    public class CombatTests
    {
        public BaseEntity DamageTaker { get; set; }
        public BaseEntity DamageDealer { get; set; }

        [TestFixtureSetUp]
        public void Setup()
        {
            DamageDealer = new BaseEntity
            {
                WeaponDamage = 10,
                Stats = new StatBlock()
            };
            DamageTaker = new BaseEntity
            {
                Stats = new StatBlock()
            };
        }

        [TestCase(5, 10)]
        [TestCase(3, 7)]
        [TestCase(14, 6)]
        public void TakeDamage_GivenBothEntitiesAndDamage_EntityTakesDamage(decimal damage, decimal health)
        {
            //Arrange
            DamageTaker.Stats.CurrentHp = health;

            //Act
            var expectedHealth = DamageTaker.Stats.CurrentHp - damage;
            DamageTaker.TakeDamage(damage, DamageDealer.Name);


            //Assert
            Assert.AreEqual(expectedHealth, DamageTaker.Stats.CurrentHp);
        }


        [TestCase(15, 10)]
        [TestCase(7, 7)]
        [TestCase(6.52, 6.51)]
        public void TakeDamage_GivenLethalDamage_TargetIsDead(decimal damage, decimal health)
        {
            //Arrange
            DamageTaker.Stats.CurrentHp = health;

            //Act
            DamageTaker.TakeDamage(damage, DamageDealer.Name);

            //Assert
            Assert.IsFalse(DamageTaker.IsAlive);
        }

        [TestCase(10, 100)]
        [TestCase(20, 80)]
        [TestCase(100, 120)]
        public void CalculateDamage_GivenNoDamageBlock_DamageIsCorrect(int attackValue, int damageMod)
        {
            //Arrange
            DamageDealer.WeaponDamage = 10;
            DamageDealer.Stats.BaseAttackPower = attackValue;
            DamageTaker.ArmorBlock = 0;
            CombatMethods.Random = new KnownSeriesRandom(new[] { damageMod });

            //Act
            var expectedValue = DamageDealer.WeaponDamage * (1 + (decimal)DamageDealer.Stats.AttackPower / 30) * ((decimal)damageMod / 100);
            var actual = DamageDealer.CalculateDamageOn(DamageTaker);

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        [TestCase(10, 5, 100)]
        [TestCase(20, 5, 100)]
        [TestCase(40, 20, 120)]
        public void CalculateDamage_GivenDamageBlock_DamageIsCorrect(int weaponDamage, int damageBlock, int damageMod)
        {
            //Arrange
            DamageDealer.WeaponDamage = weaponDamage;
            DamageTaker.ArmorBlock = damageBlock;
            CombatMethods.Random = new KnownSeriesRandom(new [] { damageMod });

            //Act
            var expectedAttack = DamageDealer.WeaponDamage * (1 + (decimal)DamageDealer.Stats.AttackPower / 30) * ((decimal)damageMod / 100);
            var expectedBlock = DamageTaker.ArmorBlock * (1 + (decimal)DamageTaker.Stats.DefensePower / 30);
            var expectedValue = expectedAttack - expectedBlock;
            var actual = DamageDealer.CalculateDamageOn(DamageTaker);

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        [TestCase(0)]
        [TestCase(30)]
        [TestCase(90)]
        [TestCase(1000000)]
        public void CalculateDamage_GivenDamageBlocksAllDamage_DamageIsMinimumValue(int weaponDamage)
        {
            //Arrange
            DamageDealer.WeaponDamage = weaponDamage;
            DamageTaker.ArmorBlock = weaponDamage;
            CombatMethods.Random = new KnownSeriesRandom(new[] { 100 });

            //Act
            var expectedValue = weaponDamage*.05;
            var actual = DamageDealer.CalculateDamageOn(DamageTaker);

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        [TestCase(0, 0)]
        [TestCase(30, 0)]
        [TestCase(0, 30)]
        [TestCase(30, 30)]
        public void CalculateDamage_GivenVaryingAttackAndDefensePowers_DamageIsCorrect(int attackPower, int defensePower)
        {
            //Arrange
            DamageDealer.WeaponDamage = 10;
            DamageTaker.ArmorBlock = 5;
            CombatMethods.Random = new KnownSeriesRandom(new [] { 100 });
            DamageDealer.Stats.BaseAttackPower = attackPower;
            DamageTaker.Stats.BaseDefensePower = defensePower;

            //Act
            var expectedAttack = 10 + (10*attackPower/30);
            var expectedDefense = 5 + (5*defensePower/30);
            var expectedValue = Math.Max(expectedAttack-expectedDefense, expectedAttack*.05);
            var actual = DamageDealer.CalculateDamageOn(DamageTaker);

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }
    }
}
