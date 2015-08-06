﻿using System.Linq;
using EmptyKeys.UserInterface.Generated;
using Microsoft.Xna.Framework;
using RogueSharp;
using RogueSharp.Random;
using SimpleGame.Models;

namespace SimpleGame.Engine
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

        public static void DrawUi(this UserInterface userInterface, GameTime gameTime, BaseEntity entity)
        {
            userInterface.DataContext = entity.Stats;
            userInterface.Draw(gameTime.ElapsedGameTime.Milliseconds);
        }
    }
}
