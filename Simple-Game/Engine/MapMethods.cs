using System.Linq;
using RogueSharp;
using RogueSharp.Random;

namespace SimpleGame.Engine
{
    public static class MapExtensions
    {
        private static readonly IRandom Random = new DotNetRandom();

        public static Cell GetRandomWalkableCell(this IMap map)
        {
            bool walkable = map.GetAllCells().Any(cell => cell.IsWalkable);
            if (!walkable)
            {
                return null;
            }
            while (true)
            {
                int x = Random.Next(map.Width);
                int y = Random.Next(map.Height);
                if (map.IsWalkable(x, y))
                {
                    return map.GetCell(x, y);
                }
            }
        }
    }
}
