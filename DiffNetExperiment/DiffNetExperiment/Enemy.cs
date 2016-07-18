using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiffNetExperiment
{
    public class Enemy : MyDrawableGameComponent
    {
        public Enemy(Game game, int currentLayer) : base(game, "Target")
        {
            if(currentLayer != maxLayer)
            {
                for (int i = 0; i < fragmentCount; i++)
                {
                    var fragment = new Enemy(game, currentLayer + 1);
                    Fragments.Add(fragment);
                    Game.Components.Add(fragment);
                }
                
            }
            CurrentLayer = currentLayer;

            Scale = new Vector2(0.8f / currentLayer, 0.8f / currentLayer);

        }

        private List<Enemy> Fragments = new List<Enemy>();


        const int maxLayer = 2;
        const int fragmentCount = 2;

        
        public int CurrentLayer { get; private set; }

        public bool HasAliveFragments
        {
            get
            {
                return IsActive || Fragments.Any(r => r.HasAliveFragments); 
            }
        }

        public void Start(Vector2 startLocation, float direction, float speed)
        {
            IsActive = true;
            Location = startLocation;
            Rotation = direction;
            MoveIncrement = speed; 
        }

        public void Reset()
        {
            IsActive = false;
            foreach(var f in Fragments)
            {
                f.Reset();
            }
        }

        public void FragmentOrDestroy()
        {
            IsActive = false;
            if (CurrentLayer != maxLayer)
            {
                foreach (var f in Fragments)
                {
                    f.Start(Location, (float)RandomNumberGenerator.NextDouble() * MathHelper.TwoPi, MoveIncrement + 1);
                }
            }
        }


        public override void Update(GameTime gameTime)
        {
            if (IsActive && (Game as Game1).CurrentState == GameState.Playing)
            {
                Vector2 direction = new Vector2((float)Math.Cos(Rotation),
                                    (float)Math.Sin(Rotation));
                direction.Normalize();
                Location += direction * MoveIncrement;
                Location = Location.ModifyLocationBasedOnViewport(GraphicsDevice.Viewport);

                var playerCollision = CollidesWith<Player>();
                if(playerCollision != null)
                {
                    (Game as Game1).SetGameOver(false);
                }
            }
            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            if (IsActive && (Game as Game1).CurrentState == GameState.Playing)
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
