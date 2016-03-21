using System;
using System.Collections.Generic;
using System.Threading;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Generated;
using EmptyKeys.UserInterface.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;
using SimpleGame.Models;
using SimpleGame.Models.EnemyAI;
using Point = RogueSharp.Point;

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
        private Dictionary<string, Texture2D> _textures; 
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
            IMapCreationStrategy<Map> mapCreationStrategy = new RandomRoomsMapCreationStrategy<Map>(48, 28, 25, 10, 3);
            _map = Map.Create(mapCreationStrategy);
            _inputState = new InputState();
            _entityManager = new EntityManager();
            _textures = new Dictionary<string, Texture2D>();

            base.Initialize();
        }

        private void GraphicsPreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            _nativeScreenWidth = graphics.PreferredBackBufferWidth;
            _nativeScreenHeight = graphics.PreferredBackBufferHeight;

            // Or any other resolution
            graphics.PreferredBackBufferWidth = _nativeScreenWidth;
            graphics.PreferredBackBufferHeight = _nativeScreenHeight;
            graphics.PreferMultiSampling = true;
            graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
        }

        private void GraphicsDeviceCreated(object sender, EventArgs e)
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
            _userInterface.Background = new SolidColorBrush(ColorW.Black);

            // TODO: use this.Content to load your game content here
            _textures["wall"] = Content.Load<Texture2D>("Wall.png");
            _textures["floor"] = Content.Load<Texture2D>("Floor.png");

            Cell startingLoc = _map.GetRandomWalkableCell();
            _player = new Player
            {
                Location = new Point(startingLoc.X, startingLoc.Y),
                Scale = 0.5f,
                Sprite = Content.Load<Texture2D>("Player.png"),
                WeaponDamage = 10,
                ArmorBlock = 4,
                Stats = new StatBlock(),
                Timer = 0,
                Name = "Player"
            };

            Cell otherStartingLoc = _map.GetRandomWalkableCell();
            var enemy = new Sentry(false, _map)
            {
                Location = new Point(otherStartingLoc.X, otherStartingLoc.Y),
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
            _map.UpdatePlayerFieldOfView(_player);
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
                    _map.UpdatePlayerFieldOfView(_player);
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
                    entity.Act(_player.Location, _entityManager);
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
            _userInterface.DrawUi(gameTime, _player.Stats);

            const int sizeOfSprites = 32;
            const float scale = .5f;

            _map.UpdatePlayerFieldOfView(_player);
            _map.DrawMap(_spriteBatch, _textures, sizeOfSprites, scale);

            HandleEntities();

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void HandleEntities()
        {
            var entitiesToAdd = new List<BaseEntity>();
            //Draw all the sprites
            foreach (var entity in _entityManager.Entities)
            {
                if (entity.IsVisible(_map) && entity.IsAlive)
                {
                    entity.Draw(_spriteBatch);
                }
                else
                {
                    if (entity is Player && !entity.IsAlive)
                    {
                        Console.WriteLine("GG RITO");
                        Thread.Sleep(2000);
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
        }
    }
}