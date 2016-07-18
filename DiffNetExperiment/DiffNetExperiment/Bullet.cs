

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DiffNetExperiment
{
    public class Bullet : MyDrawableGameComponent
    {
        public Bullet(Game game) : base(game, "bullet")
        {
        }

        
        //Keep alive for x seconds
        TimeSpan maxTimeToLive = new TimeSpan(0, 0, 0, 3);
        TimeSpan lifeTime = new TimeSpan();

        public void Fire(float angle, Vector2 origin)
        {
            Rotation = angle;
            Location = origin;
            lifeTime = new TimeSpan();
            IsActive = true;
        }

        public void Reset()
        {
            IsActive = false;
        }

        public void Destroy()
        {
            IsActive = false;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            MoveIncrement = 10f;
            Scale = new Vector2(0.8f, 0.8f);


        }

        

        public override void Update(GameTime gameTime)
        {
            if(IsActive && (Game as Game1).CurrentState == GameState.Playing)
            {
                lifeTime += gameTime.ElapsedGameTime;
                if (lifeTime > maxTimeToLive)
                {
                    IsActive = false;
                }
                else
                {
                    Vector2 direction = new Vector2((float)Math.Cos(Rotation),
                                        (float)Math.Sin(Rotation));
                    direction.Normalize();
                    Location += direction * MoveIncrement;
                    Location = Location.ModifyLocationBasedOnViewport(GraphicsDevice.Viewport);

                }

                //Does collide with an enemy
                var enemy = CollidesWith<Enemy>();
                if(enemy != null)
                {
                    Destroy();
                    enemy.FragmentOrDestroy();
                }

            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if(IsActive && (Game as Game1).CurrentState == GameState.Playing)
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
