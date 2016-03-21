using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;
using SimpleGame.Engine;
using SimpleGame.Enumerators;
using SimpleGame.Models.EnemyAI;
using Point = RogueSharp.Point;

namespace SimpleGame.Models
{
    public class BaseEntity
    {
        public Point Location { get; set; }
        public float Scale { get; set; }
        public Texture2D Sprite { get; set; }
        public PathFinder Pathfinder { get; set; }
        public IMap Map { get; set; }
        public string Name { get; set; }
        public StatBlock Stats { get; set; }
        public decimal WeaponDamage { get; set; }
        public decimal ArmorBlock { get; set; }
        public int Timer { get; set; }
        public EntityTeam EntityTeam { get; set; }
        public int LightRadius { get; set; }

        public BaseEntity()
        {
            Location = new Point();
            LightRadius = 12;
        }

        public bool IsAlive
        {
            get { return Stats.CurrentHp > 0; }
        }

        public void TakeDamage(decimal damage, string name)
        {
            Stats.CurrentHp -= damage;
            Console.WriteLine(name + " has dealt " + damage + " damage to " + Name);
        }

        public virtual bool Move(Point coord, IMap map)
        {
            if (map.IsWalkable(coord.X, coord.Y))
            {
                var xDist = Math.Abs(coord.X - Location.X);
                var yDist = Math.Abs(coord.Y - Location.Y);
                if (xDist <= 1 && yDist <= 1 && xDist + yDist < 2)
                {
                    Location = coord;
                    return true;
                }
            }
            return false;
        }

        public bool MoveOrAttack(IMap map, IEntityManager entities, Point loc)
        {
            var otherEntity = entities.GetEntityInSquare(loc);
            if (otherEntity != null)
            {
                this.MakeMeleeAttack(otherEntity);
                if (!otherEntity.IsAlive && otherEntity is Enemy)
                {
                    entities.RemoveEntity(otherEntity);
                    otherEntity.HandleDeath(this);
                    var square = map.GetRandomWalkableCell();
                    entities.Entities.Add(new Sentry(false, map) { Location = new Point(square.X, square.Y), Sprite = otherEntity.Sprite, Scale = otherEntity.Scale, Name = "Big Bad" });
                }
                return true;
            }
            else
            {
                return Move(loc, map);
            }
        }

        public bool IsVisible(IMap map)
        {
            return map.IsInFov(Location.X, Location.Y);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            float multiplier = Scale * Sprite.Width;
            spriteBatch.Draw(Sprite, new Vector2(Location.X * multiplier + 16, Location.Y * multiplier + 16),
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
            var levelDifference = this.Stats.Level - killer.Stats.Level;
            var baseExperience = this.Stats.Level * 75;
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
