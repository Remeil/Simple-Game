using System;
using NUnit.Framework;
using RogueSharp.Random;
using SimpleGame.Engine;
using SimpleGame.Enumerators;
using SimpleGame.Models;

namespace SimpleGameTests.EngineTests
{
    [TestFixture]
    public class CombatTests
    {
        public BaseEntity DamageTaker { get; set; }
        public BaseEntity DamageDealer { get; set; }

        [TestFixtureSetUp]
        public void Init()
        {
            DamageDealer = new BaseEntity
            {
                Stats = new StatBlock()
            };
            DamageTaker = new BaseEntity
            {
                Stats = new StatBlock()
            };
        }

        [SetUp]
        public void Setup()
        {
            DamageDealer.WeaponDamage = 10;
            DamageDealer.Stats.BaseAttackPower = 0;
            DamageDealer.Stats.BaseAccuracy = 0;

            DamageTaker.ArmorBlock = 5;
            DamageTaker.Stats.BaseDefensePower = 0;
            DamageTaker.Stats.BaseDodge = 0;

            CombatMethods.Random = new KnownSeriesRandom(new[] { 100 });
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


        [TestCase(15, 10, ExpectedResult = false)]
        [TestCase(7, 7, ExpectedResult = false)]
        [TestCase(6.52, 6.51, ExpectedResult = false)]
        [TestCase(10, 15, ExpectedResult = true)]
        [TestCase(0, .01, ExpectedResult = true)]
        public bool TakeDamage_GivenDamage_TargetIsDeadIfHealthIsZero(decimal damage, decimal health)
        {
            //Arrange
            DamageTaker.Stats.CurrentHp = health;

            //Act
            DamageTaker.TakeDamage(damage, DamageDealer.Name);

            //Assert
            return DamageTaker.IsAlive;
        }

        [TestCase(10)]
        [TestCase(20)]
        [TestCase(100)]
        public void CalculateDamage_GivenNoDamageBlock_DamageIsCorrect(int attackValue)
        {
            //Arrange
            DamageDealer.Stats.BaseAttackPower = attackValue;
            DamageTaker.ArmorBlock = 0;

            //Act
            var expectedValue = DamageDealer.WeaponDamage * (1 + (decimal)DamageDealer.Stats.AttackPower / 30);
            var actual = DamageDealer.CalculateDamageOn(DamageTaker);

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        [TestCase(10, 5)]
        [TestCase(20, 5)]
        [TestCase(40, 20)]
        public void CalculateDamage_GivenDamageBlock_DamageIsCorrect(int weaponDamage, int damageBlock)
        {
            //Arrange
            DamageDealer.WeaponDamage = weaponDamage;
            DamageTaker.ArmorBlock = damageBlock;

            //Act
            var expectedAttack = DamageDealer.WeaponDamage * (1 + (decimal)DamageDealer.Stats.AttackPower / 30);
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

        [TestCase(80)]
        [TestCase(100)]
        [TestCase(120)]
        public void CalculateDamage_GivenVaryingDamageModifiers_DamageIsCorrect(int damageMod)
        {
            //Only 80 - 120 are valid values for this test.
            Assert.IsTrue(damageMod >= 80 && damageMod <= 120);

            //Arrange
            CombatMethods.Random = new KnownSeriesRandom(damageMod);

            //Act
            var expectedAttack = 10 * (damageMod / 100.0);
            const int expectedDefense = 5;
            var expectedValue = Math.Max(expectedAttack - expectedDefense, expectedAttack*.05);
            var actual = DamageDealer.CalculateDamageOn(DamageTaker);

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        [TestCase(25, 5000)]
        [TestCase(50, 2500)]
        [TestCase(100, 4900)]
        [TestCase(99, 10000)]
        [TestCase(0, 100)]
        [TestCase(50, 5000)]
        public void TryToHitMelee_GivenVaryingDodgeChances_HitsAreCorrect(int dodge, int roll)
        {
            //Arrange
            CombatMethods.Random = new KnownSeriesRandom(roll);
            DamageTaker.Stats.BaseDodge = dodge;

            //Act
            //We expect to get hit if we fail the dodge roll
            var expected = DamageTaker.Stats.DodgeChance < (decimal)roll / 10000;
            var actual = DamageDealer.TryToHit(DamageTaker, CombatType.Melee);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [TestCase(25, 5000)]
        [TestCase(50, 2500)]
        [TestCase(100, 4900)]
        [TestCase(99, 10000)]
        [TestCase(0, 100)]
        [TestCase(50, 5000)]
        public void TryToHitMelee_GivenVaryingAccuracyChances_HitsAreCorrect(int accuracy, int roll)
        {
            //Arrange
            CombatMethods.Random = new KnownSeriesRandom(1, roll);
            DamageDealer.Stats.BaseAccuracy = accuracy;

            //We only make an accuracy check if we dodge successfully.
            DamageTaker.Stats.BaseDodge = 100; 

            //Act
            var expected = DamageDealer.Stats.AccuracyChance >= (decimal)roll / 10000;
            var actual = DamageDealer.TryToHit(DamageTaker, CombatType.Melee);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, 50, new[] { 2500, 4000 })]
        [TestCase(40, 50, new[] { 2500, 4000 })]
        [TestCase(50, 25, new[] { 2500, 5000 })]
        [TestCase(100, 100, new[] { 4800, 3200 })]
        [TestCase(100, 0, new[] { 6700, 2200 })]
        [TestCase(50, 50, new[] { 5000, 5000 })]
        [TestCase(0, 0, new[] { 10000, 10000 })]
        public void TryToHitMelee_GivenVaryingAccuracyAndDodgeChances_HitsAreCorrect(int dodge, int accuracy, int[] rolls)
        {
            //Arrange
            CombatMethods.Random = new KnownSeriesRandom(rolls);
            DamageDealer.Stats.BaseAccuracy = accuracy;
            DamageTaker.Stats.BaseDodge = dodge;

            //Act
            var expected = DamageTaker.Stats.DodgeChance < (decimal)rolls[0] / 10000 || DamageDealer.Stats.AccuracyChance >= (decimal)rolls[1] / 10000;
            var actual = DamageDealer.TryToHit(DamageTaker, CombatType.Melee);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, 10000)]
        [TestCase(100, 500)]
        [TestCase(50, 5000)]
        [TestCase(30, 2500)]
        public void TryToHitRanged_GivenVaryingAccuracyAndNoDodge_HitsAreCorrect(int accuracy, int roll)
        {
            //Arrange
            CombatMethods.Random = new KnownSeriesRandom(roll);
            DamageDealer.Stats.BaseAccuracy = accuracy;

            //Act
            var expected = DamageDealer.Stats.AccuracyChance >= (decimal)roll / 10000;
            var actual = DamageDealer.TryToHit(DamageTaker, CombatType.Ranged);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TryToHitRanged_GivenNoAccuracy_AttackDoesntHaveAChance()
        {
            //Arrange
            
            //Act
            var actual = DamageDealer.TryToHit(DamageTaker, CombatType.Ranged);

            //Assert
            Assert.IsFalse(actual);
        }

        [TestCase(100, 0, new[] { 1, 1 }, ExpectedResult = true)]
        [TestCase(100, 100, new[] { 1, 1 }, ExpectedResult = false)]
        [TestCase(0, 100, new[] { 1, 1 }, ExpectedResult = false)]
        [TestCase(50, 50, new[] { 5000, 5000 }, ExpectedResult = false)]
        [TestCase(80, 20, new[] { 7500, 100 }, ExpectedResult = false)]
        [TestCase(80, 20, new[] { 100, 7500 }, ExpectedResult = true)]
        [TestCase(0, 0, new[] { 1, 1 }, ExpectedResult = false)]
        [TestCase(50, 50, new[] { 4500, 5000 }, ExpectedResult = true)]
        [TestCase(50, 50, new[] { 4900, 5000 }, ExpectedResult = false)]
        public bool TryToHitRanged_GivenAccuracyAndDodge_HitsAreCorrect(int accuracy, int dodge, int[] rolls)
        {
            //Arrange
            CombatMethods.Random = new KnownSeriesRandom(rolls);
            DamageDealer.Stats.BaseAccuracy = accuracy;
            DamageTaker.Stats.BaseDodge = dodge;

            //Act
            var actual = DamageDealer.TryToHit(DamageTaker, CombatType.Ranged);

            //Assert
            return actual;
        }

        [TestCase(EntityTeam.Player, EntityTeam.Enemy, ExpectedResult = false)]
        [TestCase(EntityTeam.Enemy, EntityTeam.Enemy, ExpectedResult = true)]
        [TestCase(EntityTeam.Enemy, EntityTeam.EnemyHostileTowardOthers, ExpectedResult = false)]
        [TestCase(EntityTeam.EnemyHostileTowardOthers, EntityTeam.EnemyHostileTowardOthers, ExpectedResult = false)]
        [TestCase(EntityTeam.Player, EntityTeam.NeutralCantBeHit, ExpectedResult = true)]
        [TestCase(EntityTeam.Player, EntityTeam.NeutralCanBeHit, ExpectedResult = false)]
        [TestCase(EntityTeam.NeutralCanBeHit, EntityTeam.NeutralCanBeHit, ExpectedResult = true)]
        [TestCase(EntityTeam.NeutralCantBeHit, EntityTeam.NeutralCanBeHit, ExpectedResult = true)]
        [TestCase(EntityTeam.NeutralCanBeHit, EntityTeam.NeutralCantBeHit, ExpectedResult = true)]
        public bool OnFriendlyTeam_VariousTeams_DeterminesFriendlinessCorrectly(EntityTeam team1, EntityTeam team2)
        {
            //Arrange
            DamageDealer.EntityTeam = team1;
            DamageTaker.EntityTeam = team2;

            //Act
            var actual = DamageDealer.OnFriendlyTeam(DamageTaker);

            //Assert
            return actual;
        }
    }
}