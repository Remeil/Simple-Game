using NUnit.Framework;
using RogueSharp;
using SimpleGame.Models;

namespace SimpleGameTests.EngineTests
{
    [TestFixture]
    public class MapTests
    {
        public IMap Map { get; set; }
        public BaseEntity Entity { get; set; }

        [TestFixtureSetUp]
        public void Init()
        {
            Map = new BorderOnlyMapCreationStrategy<Map>(5, 5).CreateMap();
            Entity = new BaseEntity();
        }

        [SetUp]
        public void Setup()
        {
            Entity.Location.X = 3;
            Entity.Location.Y = 3;
        }

        [TestCase(2, 2, ExpectedResult = false)]
        [TestCase(2, 3, ExpectedResult = true)]
        [TestCase(3, 2, ExpectedResult = true)]
        [TestCase(4, 3, ExpectedResult = false)]
        [TestCase(3, 4, ExpectedResult = false)]
        public bool Move_GivenVariousTiles_CorrectlyCalculatesMove(int x, int y)
        {
            //Arrange

            //Act
            var actual = Entity.Move(new Point(x, y), Map);

            //Assert
            return actual;
        }
    }
}
