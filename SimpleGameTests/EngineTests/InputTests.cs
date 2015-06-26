using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RogueSharp;
using SimpleGame.Engine;

namespace SimpleGameTests.EngineTests
{
    [TestClass]
    public class InputTests
    {
        [TestInitialize]
        public void InitTests()
        {

        }

        [TestMethod]
        public void GivenMap_GetRandomWalkableCell_IsWalkableAndInMap()
        {
            IMap map = new Map(10, 20);

            var cell = map.GetRandomWalkableCell();

            Assert.IsTrue(cell.IsWalkable);
            Assert.IsTrue(cell.X <= 10 && cell.X >= 0);
            Assert.IsTrue(cell.Y <= 20 && cell.Y >= 0);
        }
    }
}
