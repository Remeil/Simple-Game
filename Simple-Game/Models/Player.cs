using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;
using SimpleGame.Engine;
using SimpleGame.Enumerators;

namespace SimpleGame.Models
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

        public void Draw(SpriteBatch spriteBatch)
        {
            float multiplier = Scale * Sprite.Width;
            spriteBatch.Draw(Sprite, new Vector2(X * multiplier, Y * multiplier),
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
                return MoveOrAttack(map, entities, X - 1, Y);
            }
            else if (inputState.IsRight(null))
            {
                return MoveOrAttack(map, entities, X + 1, Y);
            }
            else if (inputState.IsUp(null))
            {
                return MoveOrAttack(map, entities, X, Y - 1);
            }
            else if (inputState.IsDown(null))
            {
                return MoveOrAttack(map, entities, X, Y + 1);
            }
            return false;
        }
    }
}
