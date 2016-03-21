using System.Collections.Generic;
using RogueSharp;

namespace SimpleGame.Models
{
    public interface IEntityManager
    {
        ICollection<BaseEntity> Entities { get; set; }
        void Debug();
        BaseEntity GetNextEntity();
        BaseEntity GetEntityInSquare(Point loc);
        void UpdateTimers();
        void RemoveEntity(BaseEntity entity);
    }
}