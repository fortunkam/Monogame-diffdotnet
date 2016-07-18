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
    public class GameOverMenu : DrawableGameComponent
    {
        public GameOverMenu(Game game) : base(game)
        {
        }

        SpriteFont spriteFontH1;
        SpriteFont spriteFontH2;
        SpriteBatch spriteBatch;

        string h1Text = "Game over";
        string bottomText = "Press Enter (or tap) to begin";
        string gameOverWinText = "You destroyed everything";
        string gameOverLoseText = "You died";

        Vector2 h1Pos;
        Vector2 bottomPos;
        Vector2 middlePos;



        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFontH1 = Game.Content.Load<SpriteFont>("H1");
            spriteFontH2 = Game.Content.Load<SpriteFont>("H2");

            h1Pos = h1Text.GetTextPosition(GraphicsDevice.Viewport, spriteFontH1, 40);
            bottomPos = bottomText.GetTextPosition(GraphicsDevice.Viewport, spriteFontH2, 60);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if ((Game as Game1)?.CurrentState == GameState.GameOverFail || (Game as Game1)?.CurrentState == GameState.GameOverWin)
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
            if((Game as Game1)?.CurrentState == GameState.GameOverFail || (Game as Game1)?.CurrentState == GameState.GameOverWin)
            {
                spriteBatch.Begin();

                spriteBatch.DrawString(spriteFontH1, h1Text, h1Pos,Color.White);

                if ((Game as Game1)?.CurrentState == GameState.GameOverFail)
                {
                    spriteBatch.DrawString(spriteFontH2, gameOverLoseText, gameOverLoseText.GetTextPosition(GraphicsDevice.Viewport,spriteFontH2,50), Color.White);

                }

                if ((Game as Game1)?.CurrentState == GameState.GameOverWin)
                {
                    spriteBatch.DrawString(spriteFontH2, gameOverWinText, gameOverWinText.GetTextPosition(GraphicsDevice.Viewport, spriteFontH2, 50), Color.White);

                }



                spriteBatch.DrawString(spriteFontH2, bottomText, bottomPos, Color.White);

                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
