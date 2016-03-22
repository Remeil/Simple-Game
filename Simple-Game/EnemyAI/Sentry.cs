using System.Linq;
using RogueSharp;
using SimpleGame.Models;

namespace SimpleGame.EnemyAI
{
    public class Sentry : Enemy
    {
        private bool _alerted;
        private Point _investigationLocation;

        public Sentry(bool alerted, IMap map, Point investigationLocation = null)
            : base(map)
        {
            _alerted = alerted;
            _investigationLocation = investigationLocation;
            LightRadius = 8;
        }

        public override void Act(Point playerLocation, IEntityManager manager)
        {
            bool isInView = false;
            Map.ComputeFov(this.Location.X, this.Location.Y, LightRadius, false);
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
            var nextSquare = Pathfinder.ShortestPath(Map.GetCell(Location.X, Location.Y), Map.GetCell(playerLocation.X, playerLocation.Y)).FirstOrDefault();
            if (nextSquare == null)
            {
                return;
            }
            this.MoveOrAttack(Map, entities, new Point(nextSquare.X, nextSquare.Y));
        }
    }
}
