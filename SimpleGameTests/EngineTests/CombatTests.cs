using NUnit.Framework;
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
            DamageDealer = new BaseEntity();
            DamageTaker = new BaseEntity
            {
                Stats = new StatBlock
                {
                    BaseHp = 10,
                    CurrentHp = 10
                }
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
    }
}
