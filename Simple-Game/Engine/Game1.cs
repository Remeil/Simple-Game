using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;
using SimpleGame.Models;

namespace SimpleGame.Engine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private InputState _inputState;
        private Texture2D _wall;
        private Texture2D _floor;
        private IMap _map;
        private Player _player;
        private Enemy _enemy;
        private EntityManager _entityManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IMapCreationStrategy<Map> mapCreationStrategy = new RandomRoomsMapCreationStrategy<Map>(50, 30, 100, 7, 3);
            _map = Map.Create(mapCreationStrategy);
            _inputState = new InputState();
            _entityManager = new EntityManager();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _wall = this.Content.Load<Texture2D>("Wall.png");
            _floor = this.Content.Load<Texture2D>("Floor.png");

            Cell startingLoc = _map.GetRandomWalkableCell();
            _player = new Player
            {
                X = startingLoc.X,
                Y = startingLoc.Y,
                Scale = 0.5f,
                Sprite = Content.Load<Texture2D>("Player.png"),
                WeaponDamage = 10,
                ArmorBlock = 4,
                Stats = new StatBlock(),
                Timer = 0,
                Name = "Player"
            };

            Cell otherStartingLoc = _map.GetRandomWalkableCell();
            _enemy = new Enemy (_map)
            {
                X = otherStartingLoc.X,
                Y = otherStartingLoc.Y,
                Scale = 0.5f,
                Sprite = Content.Load<Texture2D>("Enemy.png"),
                WeaponDamage = 8,
                ArmorBlock = 2,
                Stats = new StatBlock(),
                Timer = 5000,
                Name = "Big Bad"
            };

            _entityManager.Entities.Add(_player);
            _entityManager.Entities.Add(_enemy);

            //Set starting FOV
            UpdatePlayerFieldOfView();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (_inputState.IsExitGame(PlayerIndex.One))
            {
                Exit();
            }

            _inputState.Update();

            var nextEntity = _entityManager.GetNextEntity();
            if (nextEntity == _player)
            {
                if (_player.HandleInput(_inputState, _map))
                {
                    UpdatePlayerFieldOfView();
                    nextEntity.Timer += 800;
                    _entityManager.UpdateTimers();
                    _entityManager.Debug();
                }
            }
            else if (nextEntity == _enemy)
            {
                _enemy.ChasePlayer(_player, _map);
                nextEntity.Timer += 1000;
                _entityManager.UpdateTimers();
                _entityManager.Debug();
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.TransparentBlack);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            const int sizeOfSprites = 32;
            const float scale = .5f;
            //iterate through each cell
            foreach (Cell cell in _map.GetAllCells())
            {
                //calulate curr position based on sprite size and scale
                var position = new Vector2(cell.X * sizeOfSprites * scale, cell.Y * sizeOfSprites * scale);

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
                    spriteBatch.Draw(_floor, position,
                      null, null, null, 0.0f, new Vector2(scale, scale),
                      tint, SpriteEffects.None, 0.8f);
                }

                //wall
                else
                {
                    spriteBatch.Draw(_wall, position,
                       null, null, null, 0.0f, new Vector2(scale, scale),
                       tint, SpriteEffects.None, 0.8f);
                }
            }

            //draw the player sprite
            _player.Draw(spriteBatch);

            if (_enemy.IsVisible(_map))
            {
                _enemy.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void UpdatePlayerFieldOfView()
        {
            _map.ComputeFov(_player.X, _player.Y, 10, true);
            foreach (Cell cell in _map.GetAllCells())
            {
                if (_map.IsInFov(cell.X, cell.Y))
                {
                    _map.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }
    }
}
