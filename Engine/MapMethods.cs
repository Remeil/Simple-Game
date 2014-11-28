using RogueSharp;
using RogueSharp.Random;

namespace SimpleGame.Engine
{
    public static class MapExtensions
    {
        private static readonly IRandom Random = new DotNetRandom();

        public static Cell GetRandomWalkableCell(this IMap map)
        {
            while (true)
            {
                int x = Random.Next(49);
                int y = Random.Next(29);
                if (map.IsWalkable(x, y))
                {
                    return map.GetCell(x, y);
                }
            }
        }
    }
}
