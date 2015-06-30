using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;

namespace SimpleGame.Entities
{
    public class Enemy : BaseEntity
    {
        public Enemy()
        {
            
        }

        public Enemy(IMap map)
        {
            Map = map;
            PathFinder = new PathFinder(map);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float multiplier = Scale * Sprite.Width;
            spriteBatch.Draw(Sprite, new Vector2(X * multiplier, Y * multiplier),
              null, null, null, 0.0f, new Vector2(Scale, Scale),
              Color.White, SpriteEffects.None, 0.4f);
        }

        public void GoToPoint(int destX, int destY)
        {
            var nextSquare = PathFinder.ShortestPath(Map.GetCell(X, Y), Map.GetCell(destX, destY)).First();
            X = nextSquare.X;
            Y = nextSquare.Y;
        }

        public void ChasePlayer(Player player, IMap map)
        {
            var nextSquare = PathFinder.ShortestPath(map.GetCell(X, Y), map.GetCell(player.X, player.Y)).FirstOrDefault();
            if (nextSquare == null)
            {
                return;
            }
            Move(nextSquare.X, nextSquare.Y, map);
        }

        public bool IsVisible(IMap map)
        {
            return map.IsInFov(X, Y);
        }
    }
}
