using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiffNetExperiment
{
    public class Player : MyDrawableGameComponent
    {
        public Player(Game game) : base(game, "Player")
        {
            for (int i = 0; i < 10; i++)
            {
                var bullet = new Bullet(Game);
                bullets.Add(bullet);
                Game.Components.Add(bullet);
            }
            MoveIncrement = 4f;
            Scale = new Vector2(0.2f, 0.2f);
            IsActive = true;
        }

      
        KeyboardState oldKeyboardState;
        

        private List<Bullet> bullets = new List<Bullet>();

        private SoundEffect _pew;

        public override void Initialize()
        {
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _pew = Game.Content.Load<SoundEffect>("Pew");
            var viewportBounds = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            Location = new Vector2(viewportBounds.Width / 2f, viewportBounds.Height / 2f);
            Offset = new Vector2(Texture.Width / 4f, Texture.Height / 2f);
            Scale = new Vector2(0.2f, 0.2f);
        }

        public void Reset()
        {
            var viewportBounds = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            Location = new Vector2(viewportBounds.Width / 2f, viewportBounds.Height / 2f);
            Offset = new Vector2(Texture.Width / 4f, Texture.Height / 2f);
            Scale = new Vector2(0.2f, 0.2f);
            foreach(var b in bullets)
            {
                b.Reset();
            }
        }


        public override void Update(GameTime gameTime)
        {
            if (IsActive && (Game as Game1).CurrentState == GameState.Playing)
            {
                var keyboardState = Keyboard.GetState();
                var playerOneGamePadState = GamePad.GetState(PlayerIndex.One);

                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    Rotation -= RotationIncrement;
                }

                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    Rotation += RotationIncrement;
                }

                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    Vector2 direction = new Vector2((float)Math.Cos(Rotation),
                                        (float)Math.Sin(Rotation));
                    direction.Normalize();
                    Location += direction * MoveIncrement;
                    Location = Location.ModifyLocationBasedOnViewport(GraphicsDevice.Viewport);
                }

                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    Vector2 direction = new Vector2((float)Math.Cos(Rotation),
                                        (float)Math.Sin(Rotation));
                    direction.Normalize();
                    Location -= direction * MoveIncrement;
                    Location = Location.ModifyLocationBasedOnViewport(GraphicsDevice.Viewport);
                }

                if ((keyboardState.IsKeyUp(Keys.Space) && oldKeyboardState != null 
                    && oldKeyboardState.IsKeyDown(Keys.Space)) ||
                    (keyboardState.IsKeyUp(Keys.M) && oldKeyboardState != null
                    && oldKeyboardState.IsKeyDown(Keys.M)))
                {
                    var bullet = bullets.FirstOrDefault(r => !r.IsActive);
                    if (bullet != null)
                    {
                        var instance = _pew.CreateInstance();
                        instance.Pitch = (float)((RandomNumberGenerator.NextDouble() * 0.5) + 0.4);
                        instance.Play();
                        bullet.Fire(Rotation, Location);
                    }
                }

                var thumbStickX = playerOneGamePadState.ThumbSticks.Left.X;
                var thumbStickY = -playerOneGamePadState.ThumbSticks.Left.Y;
                if (thumbStickX != 0f || thumbStickY != 0f)
                {
                    Rotation = (float)Math.Atan2(thumbStickY,
                        thumbStickX);

                    var direction = new Vector2(thumbStickX, thumbStickY);
                    direction.Normalize();
                    Location += direction * MoveIncrement;
                    Location = Location.ModifyLocationBasedOnViewport(GraphicsDevice.Viewport);
                }


                oldKeyboardState = keyboardState;

                //Check for win condition
                var enemies = Game.Components.Any(r=>r is Enemy && (r as Enemy).HasAliveFragments);
                if(!enemies)
                {
                    (Game as Game1).SetGameOver(true);
                }
            }

            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            if ((Game as Game1).CurrentState == GameState.Playing)
            {
                SpriteBatch.Begin();

                SpriteBatch.Draw(Texture, position: Location, origin: Offset,
                    rotation: Rotation,
                    //TODO: Part of talk show rescaling
                    scale: Scale);


                SpriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
