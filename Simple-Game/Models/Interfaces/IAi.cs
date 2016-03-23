using RogueSharp;

namespace SimpleGame.Models.Interfaces
{
    public interface IAi
    {
        void Act(Point playerLocation, IEntityManager manager);
    }
}
