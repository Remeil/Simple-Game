using System.Linq;
using RogueSharp;
using SimpleGame.EnemyAI;
using SimpleGame.Enumerators;

namespace SimpleGame.Models
{
    public abstract class Enemy : BaseEntity, IAi
    {
        protected Enemy(IMap map)
        {
            Stats = new StatBlock(0, 0, 0, 0, 0, 0, 0, 0);
            WeaponDamage = 8;
            ArmorBlock = 2;
            Map = map;
            Pathfinder = new PathFinder(map);
            EntityTeam = EntityTeam.Enemy;
        }

        public void GoToPoint(int destX, int destY)
        {
            var nextSquare = Pathfinder.ShortestPath(Map.GetCell(Location.X, Location.Y), Map.GetCell(destX, destY)).First();
            Location.X = nextSquare.X;
            Location.Y = nextSquare.Y;
        }

        public abstract void Act(Point playerLocation, IEntityManager manager);
    }
}
