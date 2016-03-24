using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;
using SimpleGame.Engine;
using SimpleGame.Helpers;
using Point = RogueSharp.Point;

namespace SimpleGame.Models.Entities
{
    public class Player : BaseEntity
    {

        public Player()
        {

        }

        public Player(IMap map)
        {
            Map = map;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float multiplier = Scale * Sprite.Width;
            spriteBatch.Draw(Sprite, new Vector2(Location.X * multiplier + 16, Location.Y * multiplier + 16),
              null, null, null, 0.0f, new Vector2(Scale, Scale),
              Color.White, SpriteEffects.None, 0.5f);
        }

        public bool HandleInput(InputState inputState, EntityManager entities)
        {
            return HandleInput(inputState, Map, entities);
        }

        public bool HandleInput(InputState inputState, IMap map, EntityManager entities)
        {
            if (inputState.IsLeft(PlayerIndex.One))
            {
                return MoveOrAttack(map, entities, new Point(Location.X - 1, Location.Y));
            }
            else if (inputState.IsRight(null))
            {
                return MoveOrAttack(map, entities, new Point(Location.X + 1, Location.Y));
            }
            else if (inputState.IsUp(null))
            {
                return MoveOrAttack(map, entities, new Point(Location.X, Location.Y - 1));
            }
            else if (inputState.IsDown(null))
            {
                return MoveOrAttack(map, entities, new Point(Location.X, Location.Y + 1));
            }
            return false;
        }
    }
}
