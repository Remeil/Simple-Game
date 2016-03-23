using System.Collections.Generic;
using RogueSharp;
using SimpleGame.Models.Entities;

namespace SimpleGame.Models.Interfaces
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