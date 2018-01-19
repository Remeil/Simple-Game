using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;
using SimpleGame.Models.Interfaces;
using SimpleGame.RogueSharpExtensions;

namespace SimpleGame.Models.Entities.AI
{
    public class Sentry : Enemy
    {
        private bool _alerted;
        private Point _investigationLocation;
        private Point _defendedLocation;

        public Sentry(bool alerted, IMap map, Point investigationLocation = null)
            : base(map)
        {
            _alerted = alerted;
            _investigationLocation = investigationLocation;
            LightRadius = 6;
            _defendedLocation = new Point(-1, -1);
        }

        public override void Act(Point playerLocation, IEntityManager manager)
        {
            if (_defendedLocation.X == -1 && _defendedLocation.Y == -1)
            {
                _defendedLocation = Location.Clone();
            }

            bool isInView = false;
            Map.ComputeFov(this.Location.X, this.Location.Y, LightRadius, false);
            if (Map.IsInFov(playerLocation.X, playerLocation.Y))
            {
                LightRadius = 8;
                _alerted = true;
                isInView = true;
                _investigationLocation = playerLocation;
            }

            if (isInView)
            {
                MoveToLocation(playerLocation, manager);
            }
            else if (_alerted && Location != _investigationLocation)
            {
                MoveToLocation(_investigationLocation, manager);
            }
            else if (_alerted && Location == _investigationLocation)
            {
                _investigationLocation = Point.Zero;
                MoveToLocation(_defendedLocation, manager);
            }
            else if (Location != _defendedLocation)
            {
                MoveToLocation(_defendedLocation, manager);
            }
            else
            {
                Wait();
            }
        }

        private void MoveToLocation(Point target, IEntityManager entities)
        {
            var nextSquare = Pathfinder.ShortestPath(Map.GetCell(Location.X, Location.Y), Map.GetCell(target.X, target.Y)).FirstOrDefault();
            if (nextSquare == null)
            {
                return;
            }
            MoveOrAttack(Map, entities, new Point(nextSquare.X, nextSquare.Y));
        }
    }
}
