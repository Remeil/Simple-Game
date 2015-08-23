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
                if (!otherEntity.IsAlive && otherEntity is Enemy)
                {
                    entities.RemoveEntity(otherEntity);
                    otherEntity.HandleDeath(this);
                    var square = map.GetRandomWalkableCell();
                    entities.Entities.Add(new Enemy(map) { X = square.X, Y = square.Y, Sprite = otherEntity.Sprite, Scale = otherEntity.Scale, Name = "Big Bad"});
                }
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

        public void LevelUp(Stat primary, Stat secondary, Stat tertiary)
        {
            AdjustBaseStats(primary, 3);
            AdjustBaseStats(secondary, 2);
            AdjustBaseStats(tertiary, 1);
        }

        private void AdjustBaseStats(Stat primary, int mod)
        {
            switch (primary)
            {
                case Stat.Hp:
                    Stats.BaseHp += mod;
                    break;
                case Stat.Mp:
                    Stats.BaseMp += mod;
                    break;
                case Stat.Attack:
                    Stats.BaseAttackPower += mod;
                    break;
                case Stat.Defense:
                    Stats.BaseDefensePower += mod;
                    break;
                case Stat.Magic:
                    Stats.BaseMp += mod;
                    break;
                case Stat.Accuracy:
                    Stats.BaseAccuracy += mod;
                    break;
                case Stat.Dodge:
                    Stats.BaseDodge += mod;
                    break;
                case Stat.Speed:
                    Stats.BaseSpeed += mod;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("primary");
            }
        }

        public void HandleDeath(BaseEntity killer)
        {
            var levelDifference = killer.Stats.Level - this.Stats.Level;
            var baseExperience = this.Stats.Level * 750;
            var grantedExperience = baseExperience * (Math.Pow(1.1,levelDifference));
            killer.GrantExperience((long) grantedExperience);
            Console.WriteLine(killer.Name + " killed " + Name + " for " + (long)grantedExperience + " exp.");
        }

        private void GrantExperience(long grantedExperience)
        {
            Stats.Experience += grantedExperience;
            var level = Stats.Level;
            var requiredExp = 500 * Math.Pow(level, 2) + 500*level;
            if (Stats.Experience > requiredExp)
            {
                //Actually level up
                LevelUp(Stat.Attack, Stat.Hp, Stat.Defense);
                Stats.Experience -= (long) requiredExp;
                Stats.Level += 1;
                Console.WriteLine("Level Up!");
            }
            requiredExp = 500 * Math.Pow(level, 2) + 500 * level;
            Console.WriteLine(Name + ": " + Stats.Experience + " / " + requiredExp);
        }
    }
}
