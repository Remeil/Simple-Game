using NUnit.Framework;
using SimpleGame.Enumerators;
using SimpleGame.Models;

namespace SimpleGameTests.EngineTests
{
    [TestFixture]
    public class EntityTests
    {
        private BaseEntity entity;

        [TestFixtureSetUp]
        public void Init()
        {
            entity = new BaseEntity();
        }

        [SetUp]
        public void Setup()
        {
            entity.Stats = new StatBlock();
        }

        [Test]
        public void Levelup_GivenThreeStats_IncreaseThem()
        {
            //Arrange
            //Act
            var expectedAttack = entity.Stats.BaseAttackPower + 3;
            var expectedDefense = entity.Stats.BaseDefensePower + 2;
            var expectedSpeed = entity.Stats.BaseSpeed + 1;
            entity.LevelUp(Stat.Attack, Stat.Defense, Stat.Speed);

            //Assert
            Assert.AreEqual(expectedAttack, entity.Stats.BaseAttackPower);
            Assert.AreEqual(expectedDefense, entity.Stats.BaseDefensePower);
            Assert.AreEqual(expectedSpeed, entity.Stats.BaseSpeed);
        }

        [TestCase(1, ExpectedResult = 75)]
        public long HandleDeath_GivenEntityDies_GiveKillerExperience(int level)
        {
            //Arrange
            var dyingEntity = new BaseEntity
            {
                Stats = new StatBlock {CurrentHp = 0, Level = level}
            };

            //Act
            dyingEntity.HandleDeath(entity);

            //Assert
            return entity.Stats.Experience;
        }

    }
}
