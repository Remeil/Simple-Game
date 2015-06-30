using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RogueSharp;
using SimpleGame.Entities;

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
            Entity = new BaseEntity
            {
                X = 3,
                Y = 3
            };
        }

        [SetUp]
        public void Setup()
        {
            
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
            var actual = Entity.Move(x, y, Map);

            //Assert
            return actual;
        }
    }
}
