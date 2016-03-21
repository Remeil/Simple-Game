using System.Collections.Generic;
using NUnit.Framework;
using RogueSharp;
using SimpleGame.Models;

namespace SimpleGameTests
{
    [TestFixture]
    public class EntityManagerTests
    {
        public EntityManager EntityManager { get; set; }
        public BaseEntity Entity1 { get; set; }
        public BaseEntity Entity2 { get; set; }
        public BaseEntity Entity3 { get; set; }

        [TestFixtureSetUp]
        public void Init()
        {
            EntityManager = new EntityManager();
        }

        [SetUp]
        public void Setup()
        {
            Entity1 = new BaseEntity {Location = new Point(1, 1), Timer = 1000, Name = "First"};
            Entity2 = new BaseEntity {Location = new Point(2, 2), Timer = 1500, Name = "Middle" };
            Entity3 = new BaseEntity {Location = new Point(3, 3), Timer = 2000, Name = "Last"};
            EntityManager.Entities = new HashSet<BaseEntity>
            {
                Entity1, Entity2, Entity3
            };
        }

        [Test]
        public void GetNextEntity_ReturnsCorrectEntity()
        {
            //Arrange
            //Act
            var expected = Entity1;
            var actual = EntityManager.GetNextEntity();

            //Assert
            Assert.AreSame(expected, actual);
        }

        [Test]
        public void UpdateTimers_UpdatesEntitiesTimers()
        {
            //Arrange
            //Act
            const int expected1 = 0;
            const int expected2 = 500;
            const int expected3 = 1000;
            EntityManager.UpdateTimers();

            //Assert
            Assert.AreEqual(Entity1.Timer, expected1);
            Assert.AreEqual(Entity2.Timer, expected2);
            Assert.AreEqual(Entity3.Timer, expected3);
        }

        [Test]
        public void GetEntityInSquare_GivenEntityInSquare_ReturnsCorrectEntity()
        {
            //Arrange
            //Act
            var expected = Entity1;
            var actual = EntityManager.GetEntityInSquare(new Point(1, 1));

            //Assert
            Assert.AreSame(expected, actual);
        }

        [Test]
        public void GetEntityInSquare_NoEntityInSquare_ReturnsNull()
        {
            //Arrange
            //Act
            var actual = EntityManager.GetEntityInSquare(new Point(1, 3));

            //Assert
            Assert.IsNull(actual);
        }
    }
}
