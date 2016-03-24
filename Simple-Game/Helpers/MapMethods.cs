using System.Collections.Generic;
using System.Linq;
using EmptyKeys.UserInterface.Generated;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;
using RogueSharp.Random;
using SimpleGame.Models;
using SimpleGame.Models.Entities;
using Point = RogueSharp.Point;

namespace SimpleGame.Helpers
{
    public static class MapExtensions
    {
        private static IRandom Random { get; set; }

        static MapExtensions()
        {
            Random = new DotNetRandom();
        }

        public static Cell GetRandomWalkableCell(this IMap map)
        {
            bool walkable = map.GetAllCells().Any(cell => cell.IsWalkable);
            if (!walkable)
            {
                return null;
            }
            while (true)
            {
                int x = Random.Next(map.Width - 1);
                int y = Random.Next(map.Height - 1);
                if (map.IsWalkable(x, y))
                {
                    return map.GetCell(x, y);
                }
            }
        }

        public static bool IsInFov(this IMap map, Point point)
        {
            return map.IsInFov(point.X, point.Y);
        }

        public static bool IsExplored(this IMap map, Point point)
        {
            return map.IsExplored(point.X, point.Y);
        }

        public static bool IsTransparent(this IMap map, Point point)
        {
            return map.IsTransparent(point.X, point.Y);
        }

        public static bool IsWalkable(this IMap map, Point point)
        {
            return map.IsWalkable(point.X, point.Y);
        }

        public static void DrawUi(this UserInterface userInterface, GameTime gameTime, StatBlock uiModel)
        {
            userInterface.DataContext = uiModel;
            userInterface.Draw(gameTime.ElapsedGameTime.Milliseconds);
        }

        public static void DrawMap(this IMap map, SpriteBatch spriteBatch, Dictionary<string, Texture2D> textures, int sizeOfSprites, float scale)
        {
            foreach (Cell cell in map.GetAllCells())
            {
                //calulate curr position based on sprite size and scale
                var position = new Vector2(cell.X * sizeOfSprites * scale + 16, cell.Y * sizeOfSprites * scale + 16);

                //not currently visible to player
                if (!cell.IsExplored)
                {
                    continue;
                }

                //cell has been explored but is not in FOV
                var tint = Color.White;
                if (!cell.IsInFov)
                {
                    tint = Color.DarkGray;
                }

                //floor tile
                if (cell.IsWalkable)
                {
                    spriteBatch.Draw(textures["floor"], position,
                        null, null, null, 0.0f, new Vector2(scale, scale),
                        tint, SpriteEffects.None, 0.8f);
                }

                //wall
                else
                {
                    spriteBatch.Draw(textures["wall"], position,
                        null, null, null, 0.0f, new Vector2(scale, scale),
                        tint, SpriteEffects.None, 0.8f);
                }
            }
        }

        public static void UpdatePlayerFieldOfView(this IMap map, BaseEntity player)
        {
            map.ComputeFov(player.Location.X, player.Location.Y, player.LightRadius, true);
            foreach (Cell cell in map.GetAllCells())
            {
                if (map.IsInFov(cell.X, cell.Y))
                {
                    map.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }
    }
}
