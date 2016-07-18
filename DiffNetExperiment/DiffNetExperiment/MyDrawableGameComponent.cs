using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffNetExperiment
{
    public abstract class MyDrawableGameComponent : DrawableGameComponent
    {
        protected MyDrawableGameComponent(Game game, string textureName) : base(game)
        {
            this.textureName = textureName;
        }

        protected SpriteBatch SpriteBatch;
        protected Texture2D Texture;

        protected Vector2 Location = new Vector2();
        protected Vector2 Offset;
        protected Vector2 Scale;
        protected float MoveIncrement = 3f;
        protected float Rotation;
        protected float RotationIncrement = MathHelper.Pi / 32f;
        private bool debugEnabled = false;

        private string textureName;
        protected Random RandomNumberGenerator = new Random();

        private SpriteBatch debugSpriteBatch;
        private Texture2D debugTexture;

        public bool IsActive { get; protected set; }


        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            debugSpriteBatch = new SpriteBatch(GraphicsDevice);
            Texture = Game.Content.Load<Texture2D>(textureName);
            debugTexture = Game.Content.Load<Texture2D>("bounds");
            Offset = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
            UpdateBounds();


            base.LoadContent();
        }

        private void UpdateBounds()
        {

            var size = new Point((int)(Texture.Width * Scale.X), (int)(Texture.Height * Scale.Y));
            var position = new Point((int)(Location.X - (size.X / 2)),
                (int)(Location.Y - (size.Y / 2)));
            Bounds = new Rectangle(position, size);
        }

        public Rectangle Bounds { get; private set; }

        public override void Update(GameTime gameTime)
        {
            UpdateBounds();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if(debugEnabled)
            {
                debugSpriteBatch.Begin();
                debugSpriteBatch.Draw(debugTexture, Bounds, Color.White);
                debugSpriteBatch.End();
            }
            base.Draw(gameTime);
        }

        /// <summary>
        /// Does this object collide with anything 
        /// </summary>
        /// <returns>the object it collides with or null</returns>
        protected T CollidesWith<T>() where T : MyDrawableGameComponent
        {
            var components = Game.Components;
            foreach(var c in components)
            {                            
                if (c == this) continue;
                if (c.GetType() != typeof(T)) continue;
                var collidable = c as MyDrawableGameComponent;
                if (collidable == null) continue;
                if (!collidable.IsActive) continue;
                if (collidable.Bounds.Intersects(this.Bounds)) return c as T;
            }
            return null;
        }

    }
}
