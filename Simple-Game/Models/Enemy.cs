using System.Linq;
using RogueSharp;
using SimpleGame.Models.EnemyAI;

namespace SimpleGame.Models
{
    public class Enemy : BaseEntity
    {
        public readonly IAi Ai;

        public Enemy(IMap map, IAi ai)
        {
            Ai = ai;
            Stats = new StatBlock(0, 0, 0, 0, 0, 0, 0, 0);
            WeaponDamage = 8;
            ArmorBlock = 2;
            Map = map;
            Pathfinder = new PathFinder(map);
        }

        public void HandleTurn(Player player, IMap map, EntityManager entities)
        {
            Ai.Act(map, player.Location, entities, Pathfinder);
        }

        public void GoToPoint(int destX, int destY)
        {
            var nextSquare = Pathfinder.ShortestPath(Map.GetCell(Location.X, Location.Y), Map.GetCell(destX, destY)).First();
            Location.X = nextSquare.X;
            Location.Y = nextSquare.Y;
        }
    }
}
