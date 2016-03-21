using RogueSharp;

namespace SimpleGame.Models.EnemyAI
{
    public interface IAi
    {
        void Act(Point playerLocation, IEntityManager manager);
    }
}
