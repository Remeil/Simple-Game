using Moq;
using NUnit.Framework;
using RogueSharp;
using SimpleGame.Models;
using SimpleGame.Models.Entities.AI;
using SimpleGame.Models.Interfaces;

namespace SimpleGameTests.ModelTests
{
    [TestFixture]
    public class AiTests
    {
        private IMap _map;
        private Mock<IEntityManager> _entityManager;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _map = new StringDeserializeMapCreationStrategy<Map>("#####\n#...#\n#####\n#...#\n#####").CreateMap();
            _entityManager = new Mock<IEntityManager>();
        }

        [SetUp]
        public void TestSetUp()
        {
            
        }

        [Test]
        public void Sentry_DoesntKnowWherePlayerIs_StandsStill()
        {
            //Arrange
            var expectedEnd = new Point(1, 1);
            var entity = new Sentry(false, _map) {Location = new Point(1, 1)};
            //Act
            entity.Act(new Point(3, 3), _entityManager.Object);
            //Assert
            Assert.AreEqual(expectedEnd, entity.Location);
        }

        [Test]
        public void Sentry_KnowsWherePlayerIs_PursuesThem()
        {
            //Arrange
            var expectedEnd = new Point(2, 1);
            var playerPoint = new Point(3, 1);
            var entity = new Sentry(true, _map, playerPoint) {Location = new Point(1, 1)};
            //Act
            entity.Act(playerPoint, _entityManager.Object);
            //Assert
            Assert.AreEqual(expectedEnd, entity.Location);
        }
    }
}