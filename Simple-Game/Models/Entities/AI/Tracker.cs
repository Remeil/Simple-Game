using System.Collections.Generic;
using System.Linq;
using RogueSharp;
using SimpleGame.Helpers;
using SimpleGame.Models.Interfaces;

namespace SimpleGame.Models.Entities.AI
{
    public class Tracker : Enemy
    {
        private bool _alerted;
        private Point _investigationPoint;
        private bool _tracking;
        private readonly List<Point> _lastMoves;

        public Tracker(IMap map) : base(map)
        {
            _alerted = false;
            _investigationPoint = null;
            _tracking = false;
            _lastMoves = new List<Point>(5);
            LightRadius = 6;
        }

        public override void Act(Point playerLocation, IEntityManager manager)
        {
            bool isInView = false;
            Map.ComputeFov(this.Location.X, this.Location.Y, LightRadius, false);
            if (Map.IsInFov(playerLocation))
            {
                _alerted = true;
                _investigationPoint = playerLocation;
                _tracking = false;
                isInView = true;
            }

            if (!isInView && !_tracking && _alerted)
            {
                var xDirection = _lastMoves.Last().X - _lastMoves.First().X;
                var yDirection = _lastMoves.Last().Y - _lastMoves.First().Y;
                _tracking = true;
                _investigationPoint = new Point(xDirection * 3 + Location.X, yDirection * 3 + Location.Y);

                var initialPoint = _investigationPoint;
                var searchRadius = 0;
                while (!Map.IsWalkable(_investigationPoint))
                {
                    searchRadius++;
                    var cellsInRange = Map.GetCellsInRadius(initialPoint.X, initialPoint.Y, searchRadius);

                    foreach (var cell in cellsInRange)
                    {
                        if (cell.IsWalkable)
                        {
                            _investigationPoint = new Point(cell.X, cell.Y);
                            break;
                        }
                    }
                }
            }

            if (_tracking && Location == _investigationPoint)
            {
                _tracking = false;
            }

            if (isInView || (_alerted && _tracking))
            {
                ChasePlayer(_investigationPoint, manager);
            }
        }

        private void ChasePlayer(Point playerLocation, IEntityManager entities)
        {
            var nextSquare = Pathfinder.ShortestPath(Map.GetCell(Location.X, Location.Y), Map.GetCell(playerLocation.X, playerLocation.Y)).FirstOrDefault();
            if (nextSquare == null)
            {
                return;
            }
            else
            {
                if (_lastMoves.Count == 5)
                {
                    _lastMoves.RemoveAt(0);
                }
                _lastMoves.Add(new Point(nextSquare.X, nextSquare.Y));
            }
            this.MoveOrAttack(Map, entities, new Point(nextSquare.X, nextSquare.Y));
        }
    }
}