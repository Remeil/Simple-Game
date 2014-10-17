using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;
using RogueSharp.Random;
using SimpleGame.Entities;

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

            Cell startingLoc = MapExtensions.GetRandomWalkableCell(_map);
            _player = new Player
            {
                X = startingLoc.X,
                Y = startingLoc.Y,
                Scale = 0.5f,
                Sprite = Content.Load<Texture2D>("Player.png")
            };

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
            // TODO: Add your update logic here
            _inputState.Update();
            if (_inputState.IsLeft(null))
            {
                _player.X--;
            }
            else if (_inputState.IsRight(null))
            {
                _player.X++;
            }
            else if (_inputState.IsUp(null))
            {
                _player.Y--;
            }
            else if (_inputState.IsDown(null))
            {
                _player.Y++;
            }
            UpdatePlayerFieldOfView();

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

                //visible to player
                if (!cell.IsInFov)
                {
                    continue;
                }

                //floor tile
                if (cell.IsWalkable)
                {
                    spriteBatch.Draw(_floor, position,
                      null, null, null, 0.0f, new Vector2(scale, scale),
                      Color.White, SpriteEffects.None, 0.8f);
                }

                //wall
                else
                {
                    spriteBatch.Draw(_wall, position,
                       null, null, null, 0.0f, new Vector2(scale, scale),
                       Color.White, SpriteEffects.None, 0.8f);
                }
            }

            //draw the player sprite
            _player.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void UpdatePlayerFieldOfView()
        {
            _map.ComputeFov(_player.X, _player.Y, 10, true);
        }
    }
}
