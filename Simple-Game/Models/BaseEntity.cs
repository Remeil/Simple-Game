﻿using System;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;

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
    }
}
