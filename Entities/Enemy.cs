using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;

namespace SimpleGame.Entities
{
    public class Enemy : BaseEntity
    {
        public void Draw(SpriteBatch spriteBatch)
        {
            float multiplier = Scale * Sprite.Width;
            spriteBatch.Draw(Sprite, new Vector2(X * multiplier, Y * multiplier),
              null, null, null, 0.0f, new Vector2(Scale, Scale),
              Color.White, SpriteEffects.None, 0.4f);
        }

        public void ChasePlayer(Player player, IMap map)
        {
            if (X < player.X && Y < player.Y && map.IsWalkable(X + 1, Y + 1))
            {
                X++;
                Y++;
            }
            else if (X < player.X && Y > player.Y && map.IsWalkable(X + 1, Y - 1))
            {
                X++;
                Y--;
            }
            else if (X > player.X && Y > player.Y && map.IsWalkable(X - 1, Y - 1))
            {
                X--;
                Y--;
            }
            else if (X > player.X && Y < player.Y && map.IsWalkable(X - 1, Y + 1))
            {
                X--;
                Y++;
            }
            else if (X < player.X && map.IsWalkable(X + 1, Y))
            {
                X++;
            }
            else if (X > player.X && map.IsWalkable(X - 1, Y))
            {
                X--;
            }
            else if (Y > player.Y && map.IsWalkable(X, Y - 1))
            {
                Y--;
            }
            else if (Y < player.Y && map.IsWalkable(X, Y + 1))
            {
                Y++;
            }
        }
    }
}
