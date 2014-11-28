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
    }
}
