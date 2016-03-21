using System.Linq;
using RogueSharp;

namespace SimpleGame.Models.EnemyAI
{
    public class Sentry : Enemy
    {
        private bool _alerted;
        private const int VisionRange = 12;
        private Point _investigationLocation;

        public Sentry(bool alerted, IMap map, Point investigationLocation = null)
            : base(map)
        {
            _alerted = alerted;
            _investigationLocation = investigationLocation;
        }

        public override void Act(Point playerLocation, IEntityManager manager)
        {
            bool isInView = false;
            Map.ComputeFov(this.Location.X, this.Location.Y, VisionRange, false);
            if (Map.IsInFov(playerLocation.X, playerLocation.Y))
            {
                _alerted = true;
                isInView = true;
                _investigationLocation = playerLocation;
            }

            if (isInView)
            {
                ChasePlayer(playerLocation, manager);
            }
            else if (_alerted)
            {
                ChasePlayer(_investigationLocation, manager);
            }
            else
            {
                return;
            }
        }

        private void ChasePlayer(Point playerLocation, IEntityManager entities)
        {
            var nextSquare = Pathfinder.ShortestPath(Map.GetCell(playerLocation.X, playerLocation.Y), Map.GetCell(playerLocation.X, playerLocation.Y)).FirstOrDefault();
            if (nextSquare == null)
            {
                return;
            }
            this.MoveOrAttack(Map, entities, new Point(nextSquare.X, nextSquare.Y));
        }
    }
}
