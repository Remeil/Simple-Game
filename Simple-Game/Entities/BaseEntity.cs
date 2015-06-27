using System;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;

namespace SimpleGame.Entities
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

        public bool IsAlive
        {
            get { return Stats.CurrentHp > 0; }
        }

        public void TakeDamage(decimal damage, string name)
        {
            Stats.CurrentHp -= damage;
            Console.WriteLine(name + " has dealt " + damage + " damage to " + Name);
        }
    }
}
