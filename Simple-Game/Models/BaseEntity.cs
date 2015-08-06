using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;
using SimpleGame.Engine;
using SimpleGame.Enumerators;

namespace SimpleGame.Models
{
    public class BaseEntity
    {
        public int X { get; set; }
        public int Y { get; set; }
        public float Scale { get; set; }
        public Texture2D Sprite { get; set; }
        public PathFinder PathFinder { get; set; }
        public IMap Map { get; set; }
        public string Name { get; set; }
        public StatBlock Stats { get; set; }
        public decimal WeaponDamage { get; set; }
        public decimal ArmorBlock { get; set; }
        public int Timer { get; set; }
        public EntityTeam EntityTeam { get; set; }

        public bool IsAlive
        {
            get { return Stats.CurrentHp > 0; }
        }

        public void TakeDamage(decimal damage, string name)
        {
            Stats.CurrentHp -= damage;
            Console.WriteLine(name + " has dealt " + damage + " damage to " + Name);
        }

        public virtual bool Move(int xCoord, int yCoord, IMap map)
        {
            if (map.IsWalkable(xCoord, yCoord))
            {
                var xDist = Math.Abs(xCoord - X);
                var yDist = Math.Abs(yCoord - Y);
                if (xDist <= 1 && yDist <= 1 && xDist + yDist < 2)
                {
                    X = xCoord;
                    Y = yCoord;
                    return true;
                }
            }
            return false;
        }

        protected bool MoveOrAttack(IMap map, EntityManager entities, int x, int y)
        {
            var otherEntity = entities.GetEntityInSquare(x, y);
            if (otherEntity != null)
            {
                this.MakeMeleeAttack(otherEntity);
                return true;
            }
            else
            {
                return Move(x, y, map);
            }
        }

        public bool IsVisible(IMap map)
        {
            return map.IsInFov(X, Y);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            float multiplier = Scale * Sprite.Width;
            spriteBatch.Draw(Sprite, new Vector2(X * multiplier + 16, Y * multiplier + 16),
              null, null, null, 0.0f, new Vector2(Scale, Scale),
              Color.White, SpriteEffects.None, 0.4f);
        }
    }
}
