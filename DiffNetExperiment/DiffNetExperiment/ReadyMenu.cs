using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffNetExperiment
{
    public class ReadyMenu : DrawableGameComponent
    {
        public ReadyMenu(Game game) : base(game)
        {
        }

        SpriteFont spriteFontH1;
        SpriteFont spriteFontH2;
        SpriteBatch spriteBatch;

        Vector2 h1Pos;
        Vector2 h2Pos;

        string h1Text = "DiffWars";
        string h2Text = "Press Enter (or tap) to begin";


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFontH1 = Game.Content.Load<SpriteFont>("H1");
            spriteFontH2 = Game.Content.Load<SpriteFont>("H2");

            h1Pos = h1Text.GetTextPosition(GraphicsDevice.Viewport, spriteFontH1, 40);
            h2Pos = h2Text.GetTextPosition(GraphicsDevice.Viewport, spriteFontH2, 60);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if ((Game as Game1)?.CurrentState == GameState.Ready)
            {
                var touchState = TouchPanel.GetState();
                var keyboardState = Keyboard.GetState();
                if(keyboardState.IsKeyDown(Keys.Enter) || touchState.Count > 0)
                {
                    (Game as Game1).StartGame();
                }

            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if((Game as Game1)?.CurrentState == GameState.Ready)
            {
                spriteBatch.Begin();

                spriteBatch.DrawString(spriteFontH1, h1Text, h1Pos,Color.White);
                spriteBatch.DrawString(spriteFontH2, h2Text, h2Pos, Color.White);

                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
