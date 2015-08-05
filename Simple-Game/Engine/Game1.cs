using System;
using System.Collections.Generic;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Generated;
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
        private SpriteBatch _spriteBatch;
        private InputState _inputState;
        private Texture2D _wall;
        private Texture2D _floor;
        private IMap _map;
        private EntityManager _entityManager;
        private Player _player;
        private UserInterface _userInterface;
        private int _nativeScreenWidth;
        private int _nativeScreenHeight;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreparingDeviceSettings += GraphicsPreparingDeviceSettings;
            graphics.DeviceCreated += GraphicsDeviceCreated;
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
            //IMapCreationStrategy<Map> mapCreationStrategy = new RandomRoomsMapCreationStrategy<Map>(50, 30, 100, 7, 3);
            IMapCreationStrategy<Map> mapCreationStrategy = new BorderOnlyMapCreationStrategy<Map>(50, 30);
            _map = Map.Create(mapCreationStrategy);
            _inputState = new InputState();
            _entityManager = new EntityManager();

            base.Initialize();
        }

        void GraphicsPreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            _nativeScreenWidth = graphics.PreferredBackBufferWidth;
            _nativeScreenHeight = graphics.PreferredBackBufferHeight;

            // Or any other resolution
            graphics.PreferredBackBufferWidth = _nativeScreenWidth + 32;
            graphics.PreferredBackBufferHeight = _nativeScreenHeight + 32;
            graphics.PreferMultiSampling = true;
            graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
        }

        void GraphicsDeviceCreated(object sender, EventArgs e)
        {
            EmptyKeys.UserInterface.Engine engine = new MonoGameEngine(GraphicsDevice, _nativeScreenWidth, _nativeScreenHeight);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Create the user interface
            var font = Content.Load<SpriteFont>("UI/Segoe_UI_9_Regular");
            FontManager.DefaultFont = EmptyKeys.UserInterface.Engine.Instance.Renderer.CreateFont(font);
            _userInterface = new UserInterface(_nativeScreenWidth, _nativeScreenHeight);

            // TODO: use this.Content to load your game content here
            _wall = Content.Load<Texture2D>("Wall.png");
            _floor = Content.Load<Texture2D>("Floor.png");

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
            var enemy = new Enemy(_map)
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
            _entityManager.Entities.Add(enemy);

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
            if (!nextEntity.IsAlive)
            {
                _entityManager.RemoveEntity(nextEntity);
            }

            var player = nextEntity as Player;
            if (player != null)
            {
                var entity = player;
                if (entity.HandleInput(_inputState, _map, _entityManager))
                {
                    UpdatePlayerFieldOfView();
                    player.Timer += 800;
                    _entityManager.UpdateTimers();
                    _entityManager.Debug();
                }
            }
            else
            {
                var enemy = nextEntity as Enemy;
                if (enemy != null)
                {
                    var entity = enemy;
                    entity.HandleTurn(_player, _map, _entityManager);
                    enemy.Timer += 1000;
                    _entityManager.UpdateTimers();
                    _entityManager.Debug();
                }
            }

            //Update User interface
            _userInterface.UpdateInput(gameTime.ElapsedGameTime.Milliseconds);
            _userInterface.UpdateLayout(gameTime.ElapsedGameTime.Milliseconds);

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
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            //Draw UI
            _userInterface.DrawUi(gameTime, _player);

            const int sizeOfSprites = 32;
            const float scale = .5f;
            //iterate through each cell
            foreach (Cell cell in _map.GetAllCells())
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
                    _spriteBatch.Draw(_floor, position,
                      null, null, null, 0.0f, new Vector2(scale, scale),
                      tint, SpriteEffects.None, 0.8f);
                }

                //wall
                else
                {
                    _spriteBatch.Draw(_wall, position,
                       null, null, null, 0.0f, new Vector2(scale, scale),
                       tint, SpriteEffects.None, 0.8f);
                }
            }

            var entitiesToAdd = new List<BaseEntity>();
            var entitiesToRemove = new List<BaseEntity>();
            //Draw all the sprites
            foreach (var entity in _entityManager.Entities)
            {
                if (entity.IsVisible(_map) && entity.IsAlive)
                {
                    entity.Draw(_spriteBatch);
                }
                else if (!entity.IsAlive && entity is Enemy)
                {
                    var startingLoc = _map.GetRandomWalkableCell();
                    entitiesToAdd.Add(new Enemy(_map)
                        {
                            X = startingLoc.X,
                            Y = startingLoc.Y,
                            Scale = 0.5f,
                            Sprite = Content.Load<Texture2D>("Enemy.png"),
                            WeaponDamage = 8,
                            ArmorBlock = 2,
                            Stats = new StatBlock(),
                            Timer = 50000,
                            Name = "Big Bad"
                        });
                    entitiesToRemove.Add(entity);
                }
                else
                {
                    if (entity is Player)
                    {
                        Console.WriteLine("GG RITO");
                        Exit();
                    }
                }
            }

            foreach (var entity in entitiesToAdd)
            {
                _entityManager.Entities.Add(entity);
                if (entity.IsVisible(_map))
                {
                    entity.Draw(_spriteBatch);
                }
            }

            foreach (var entity in entitiesToRemove)
            {
                _entityManager.RemoveEntity(entity);
            }

            _spriteBatch.End();

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