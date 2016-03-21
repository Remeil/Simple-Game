using RogueSharp;

namespace SimpleGame.Models.EnemyAI
{
    public interface IAi
    {
        void Act(IMap map, Point playerLocation, IEntityManager manager, PathFinder pathfinder);
    }
}
