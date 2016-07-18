using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffNetExperiment
{
    public static class ViewportExtensions
    {
        public static Vector2 ModifyLocationBasedOnViewport(this Vector2 source, Viewport viewport)
        {
            var returnVector = new Vector2(source.X, source.Y);

            if (returnVector.X < 0)
            {
                returnVector.X = viewport.Width;
            }
            else if (returnVector.X > viewport.Width)
            {
                returnVector.X = 0;
            }

            if (returnVector.Y < 0)
            {
                returnVector.Y = viewport.Height;
            }
            else if (returnVector.Y > viewport.Height)
            {
                returnVector.Y = 0;
            }

            return returnVector;
        }

        public static Vector2 GetTextPosition(this string text, Viewport viewport, SpriteFont font, int verticalPercent)
        {
            var textBounds = font.MeasureString(text);
            var y = viewport.Height * (verticalPercent / 100f);
            var viewportWidth = viewport.Width;
            var x = (viewportWidth / 2f) - (textBounds.X / 2f);

            return new Vector2(x, y);
        }
    }
}
