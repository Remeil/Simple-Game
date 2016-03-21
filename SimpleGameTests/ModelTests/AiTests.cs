using Moq;
using NUnit.Framework;
using RogueSharp;
using SimpleGame.Models;
using SimpleGame.Models.EnemyAI;

namespace SimpleGameTests.ModelTests
{
    [TestFixture]
    public class AiTests
    {
        private IMap _map;
        private BaseEntity _entity;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _map = new BorderOnlyMapCreationStrategy<Map>(5, 5).CreateMap();
            _entity = new BaseEntity();
        }

        [SetUp]
        public void TestSetUp()
        {
            
        }

        [Test]
        public void Sentry_DoesntKnowWherePlayerIs_StandsStill()
        {
            //Arrange
            _entity.Location = new Point(3, 3);
            var startingPoint = new Point(3, 3);
            var entity = new Sentry(false, _map);
            var entityManager = new Mock<IEntityManager>();
            //Act
            entity.Act(new Point(4,4), entityManager.Object);
            //Assert
            Assert.AreEqual(startingPoint, _entity.Location);
        }
    }
}
