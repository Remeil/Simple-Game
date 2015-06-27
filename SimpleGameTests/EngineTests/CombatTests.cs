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
            DamageDealer.Stats.BaseAttackPower = attackValue;
            CombatMethods.Random = new KnownSeriesRandom(new[] { damageMod });

            //Act
            var expectedValue = DamageDealer.WeaponDamage * ((decimal)DamageDealer.Stats.AttackPower / 30) * ((decimal)damageMod / 100);
            var actual = DamageDealer.CalculateDamageOn(DamageTaker);

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }
    }
}
