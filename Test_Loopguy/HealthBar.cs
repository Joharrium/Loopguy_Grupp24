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
            outline = TextureManager.healthbar_small_outline;
            fill = TextureManager.healthbar_small_fill;
            fill_bg = TextureManager.healthbar_small_fill_bg;
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

    public class PlayerHealthBar
    {
        Vector2 position = new Vector2(4,4);
        Texture2D outline;
        Texture2D inline;
        Texture2D fill;
        Texture2D fill_bg;
        int value;
        int maxValue;
        Rectangle srcRectangle;

        public PlayerHealthBar(int maxValue)
        {
            this.maxValue = maxValue;
            value = maxValue;
            outline = TextureManager.player_healthbar_outline;
            inline = TextureManager.player_healthbar_inline;
            fill = TextureManager.player_healthbar_fill;
            fill_bg = TextureManager.player_healthbar_fill_bg;
            srcRectangle = new Rectangle(0, 0, 2, 30);
        }

        public void UpdateBar(int value)
        {
            this.value = value;
            srcRectangle = new Rectangle(0 , 0, 16 * (maxValue - this.value) + 8    , 30);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            
            spriteBatch.Draw(fill_bg, position, Color.White);
            if(maxValue != value)
            {
                spriteBatch.Draw(fill, position + new Vector2(80 - ((Math.Abs(value - maxValue) * 16)), 0), srcRectangle, Color.White);
            }
            
            spriteBatch.Draw(outline, position, Color.White);
            spriteBatch.Draw(inline, position, Color.White);
        }

    }
}
