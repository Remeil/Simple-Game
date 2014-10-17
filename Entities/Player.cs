using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;
using SimpleGame.Engine;

namespace SimpleGame.Entities
{
    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public float Scale { get; set; }
        public Texture2D Sprite { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            float multiplier = Scale * Sprite.Width;
            spriteBatch.Draw(Sprite, new Vector2(X * multiplier, Y * multiplier),
              null, null, null, 0.0f, new Vector2(Scale, Scale),
              Color.White, SpriteEffects.None, 0.5f);
        }

        public bool HandleInput(InputState inputState, IMap map)
        {
            if (inputState.IsLeft(PlayerIndex.One))
            {
                if (map.IsWalkable(X-1, Y))
                {
                    X--;
                    return true;
                }
            }
            else if (inputState.IsRight(null))
            {
                if (map.IsWalkable(X + 1, Y))
                {
                    X++;
                    return true;
                }
            }
            else if (inputState.IsUp(null))
            {
                if (map.IsWalkable(X, Y - 1))
                {
                    Y--;
                    return true;
                }
            }
            else if (inputState.IsDown(null))
            {
                if (map.IsWalkable(X, Y + 1))
                {
                    Y++;
                    return true;
                }
            }
            return false;
        }

    }
}
