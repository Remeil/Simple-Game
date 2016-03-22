using RogueSharp;
using SimpleGame.Models;

namespace SimpleGame.EnemyAI
{
    public interface IAi
    {
        void Act(Point playerLocation, IEntityManager manager);
    }
}
