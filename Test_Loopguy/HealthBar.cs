using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Test_Loopguy.Content;

namespace Test_Loopguy
{
    public class HealthBar
    {
        Vector2 position;
        Texture2D outline;
        Texture2D fill;
        Texture2D fill_bg;
        int value;
        int maxValue;
        Rectangle srcRectangle;

        public HealthBar(int maxValue)
        {
            this.maxValue = maxValue;
            outline = TexMGR.healthbar_small_outline;
            fill = TexMGR.healthbar_small_fill;
            fill_bg = TexMGR.healthbar_small_fill_bg;
            srcRectangle = new Rectangle(1, 0, 9, 4);
        }

        public void SetCurrentValue(Vector2 pos, int value)
        {
            position = pos;
            this.value = value;
            srcRectangle.Width = (int)((value * 9) / maxValue);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(outline, position, Color.White);
            spriteBatch.Draw(fill_bg, position, Color.White);

            spriteBatch.Draw(fill, position + new Vector2(1,0), srcRectangle, Color.White);
        }
    }
}
