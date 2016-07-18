using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DiffNetExperiment
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D backgroundTexture;

        Rectangle viewportBounds;

        Player player;

        private Enemy[] enemies = new Enemy[2];

        Random randomNumberGenerator = new Random();

        private List<Vector2> possibleEnemyStartLocations = new List<Vector2>();

        public GameState CurrentState { get; private set; } = GameState.Ready;

        private ReadyMenu readyMenu;

        private GameOverMenu gameOverMenu;

        public void StartGame()
        {
            player.Reset();
            foreach(var e in enemies)
            {
                e.Reset();
            }
            enemies[0].Start(GetRelativePos(10,10), (float)randomNumberGenerator.NextDouble() * MathHelper.TwoPi, 5f);
            enemies[1].Start(GetRelativePos(90,90), (float)randomNumberGenerator.NextDouble() * MathHelper.TwoPi, 5f);

            CurrentState = GameState.Playing;
            
        }

        private Vector2 GetRelativePos(int widthPercent, int heightPercent)
        {
            var viewport = GraphicsDevice.Viewport;
            return new Vector2(viewport.Width * (widthPercent / 100f), viewport.Height * (heightPercent / 100f));
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 1024;
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
            player = new Player(this);
            Components.Add(player);

            enemies[0] = new Enemy(this, 1);
            enemies[1] = new Enemy(this, 1);

            Components.Add(enemies[0]);
            Components.Add(enemies[1]);

            readyMenu = new ReadyMenu(this);
            Components.Add(readyMenu);

           gameOverMenu = new GameOverMenu(this);
            Components.Add(gameOverMenu);

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
            backgroundTexture = Content.Load<Texture2D>("Background");

            viewportBounds = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

        }

        public void SetGameOver(bool isWin)
        {
            CurrentState = isWin ? GameState.GameOverWin : GameState.GameOverFail;
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var playerOneGamePadState = GamePad.GetState(PlayerIndex.One);
            if (playerOneGamePadState.Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

           // CheckForCollision();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //TODO: Learn the options
            spriteBatch.Begin();

            //TODO: What is the Color Mask for?
            spriteBatch.Draw(backgroundTexture, viewportBounds, Color.White);
            
            spriteBatch.End();
            


            base.Draw(gameTime);
        }
    }
}
