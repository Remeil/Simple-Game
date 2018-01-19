using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MoreLinq;
using RogueSharp;
using SimpleGame.Models.Entities;
using SimpleGame.Models.Interfaces;

namespace SimpleGame.Models
{
    public class EntityManager : IEntityManager
    {
        public EntityManager()
        {
            Entities = new HashSet<BaseEntity>();
        }

        public ICollection<BaseEntity> Entities { get; set; }

        public void Debug()
        {
            foreach (var entity in Entities.OrderBy(x => x.Timer))
            {
                Console.WriteLine("Entity Name: " + entity.Name + " Timer: " + entity.Timer);
                Console.WriteLine("HP: " + entity.Stats.CurrentHp);
            }
            Console.WriteLine();

            Thread.Sleep(16);
        }

        public BaseEntity GetNextEntity()
        {
            return Entities.MinBy(entity => entity.Timer);
        }

        public BaseEntity GetEntityInSquare(Point loc)
        {
            return Entities.SingleOrDefault(entity => entity.Location == loc);
        }

        public void UpdateTimers()
        {
            var minimumTimerValue = Entities.Min(entity => entity.Timer);
            Entities.ForEach(entity => entity.Timer -= minimumTimerValue);
        }

        public void RemoveEntity(BaseEntity entity)
        {
            Entities.Remove(entity);
        }

    }
}
