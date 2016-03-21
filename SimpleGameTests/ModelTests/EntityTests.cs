using NUnit.Framework;
using SimpleGame.Enumerators;
using SimpleGame.Models;

namespace SimpleGameTests.ModelTests
{
    [TestFixture]
    public class EntityTests
    {
        private BaseEntity _entity;

        [TestFixtureSetUp]
        public void Init()
        {
            _entity = new BaseEntity();
        }

        [SetUp]
        public void Setup()
        {
            _entity.Stats = new StatBlock();
        }

        [Test]
        public void Levelup_GivenThreeStats_IncreaseThem()
        {
            //Arrange
            //Act
            var expectedAttack = _entity.Stats.BaseAttackPower + 3;
            var expectedDefense = _entity.Stats.BaseDefensePower + 2;
            var expectedSpeed = _entity.Stats.BaseSpeed + 1;
            _entity.LevelUp(Stat.Attack, Stat.Defense, Stat.Speed);

            //Assert
            Assert.AreEqual(expectedAttack, _entity.Stats.BaseAttackPower);
            Assert.AreEqual(expectedDefense, _entity.Stats.BaseDefensePower);
            Assert.AreEqual(expectedSpeed, _entity.Stats.BaseSpeed);
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
            dyingEntity.HandleDeath(_entity);

            //Assert
            return _entity.Stats.Experience;
        }

    }
}
