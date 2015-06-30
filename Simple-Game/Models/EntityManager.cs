using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace SimpleGame.Models
{
    public class EntityManager
    {
        public EntityManager()
        {
            Entities = new HashSet<BaseEntity>();
        }

        public ICollection<BaseEntity> Entities { get; set; }

        public BaseEntity GetNextEntity()
        {
            return Entities.MinBy(entity => entity.Timer);
        }

        public BaseEntity GetEntityInSquare(int x, int y)
        {
            return Entities.SingleOrDefault(entity => entity.X == x && entity.Y == y);
        }

        public void UpdateTimers()
        {
            var minimumTimerValue = Entities.Min(entity => entity.Timer);
            Entities.ForEach(entity => entity.Timer -= minimumTimerValue);
        }
    }
}
