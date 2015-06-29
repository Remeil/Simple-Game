using System;
using System.Linq;
using RogueSharp;
using RogueSharp.Random;

namespace SimpleGame.Engine
{
    public static class MapExtensions
    {
        private static IRandom Random { get; set; }

        static MapExtensions()
        {
            Random = new DotNetRandom();
        }

        public static Cell GetRandomWalkableCell(this IMap map)
        {
            bool walkable = map.GetAllCells().Any(cell => cell.IsWalkable);
            if (!walkable)
            {
                return null;
            }
            while (true)
            {
                int x = Random.Next(map.Width - 1);
                int y = Random.Next(map.Height - 1);
                if (map.IsWalkable(x, y))
                {
                    return map.GetCell(x, y);
                }
            }
        }
    }
}
