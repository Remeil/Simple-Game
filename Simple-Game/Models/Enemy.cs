using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;

namespace SimpleGame.Models
{
    public class Enemy : BaseEntity
    {
        public Enemy()
        {
            
        }

        public Enemy(IMap map)
        {
            Map = map;
            PathFinder = new PathFinder(map);
        }

        public void HandleTurn(Player player, IMap map, EntityManager entities)
        {
            ChasePlayer(player, map, entities);
        }

        public void GoToPoint(int destX, int destY)
        {
            var nextSquare = PathFinder.ShortestPath(Map.GetCell(X, Y), Map.GetCell(destX, destY)).First();
            X = nextSquare.X;
            Y = nextSquare.Y;
        }

        private void ChasePlayer(Player player, IMap map, EntityManager entities)
        {
            var nextSquare = PathFinder.ShortestPath(map.GetCell(X, Y), map.GetCell(player.X, player.Y)).FirstOrDefault();
            if (nextSquare == null)
            {
                return;
            }
            MoveOrAttack(map, entities, nextSquare.X, nextSquare.Y);
        }
    }
}
