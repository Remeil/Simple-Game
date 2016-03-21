using System.Linq;
using RogueSharp;

namespace SimpleGame.Models.EnemyAI
{
    public class Sentry : IAi
    {
        private bool _alerted;
        private readonly BaseEntity _entity;
        private const int VisionRange = 12;
        private Point _investigationLocation;

        public Sentry(bool alerted, BaseEntity entity, Point investigationLocation = null)
        {
            _alerted = alerted;
            _entity = entity;
            _investigationLocation = investigationLocation;
        }

        public void Act(IMap map, Point playerLocation, IEntityManager manager, PathFinder pathfinder)
        {
            bool isInView = false;
            map.ComputeFov(playerLocation.X, playerLocation.Y, VisionRange, false);
            if (map.IsInFov(playerLocation.X, playerLocation.Y))
            {
                _alerted = true;
                isInView = true;
                _investigationLocation = playerLocation;
            }

            if (isInView)
            {
                ChasePlayer(playerLocation, map, manager, pathfinder);
            }
            else if (_alerted)
            {
                ChasePlayer(_investigationLocation, map, manager, pathfinder);
            }
            else
            {
                return;
            }
        }

        private void ChasePlayer(Point playerLocation, IMap map, IEntityManager entities, PathFinder pathfinder)
        {
            var nextSquare = pathfinder.ShortestPath(map.GetCell(playerLocation.X, playerLocation.Y), map.GetCell(playerLocation.X, playerLocation.Y)).FirstOrDefault();
            if (nextSquare == null)
            {
                return;
            }
            _entity.MoveOrAttack(map, entities, new Point(nextSquare.X, nextSquare.Y));
        }
    }
}
