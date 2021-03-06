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
        protected Vector2 position;
        protected Texture2D outline;
        protected Texture2D fill;
        protected Texture2D fill_bg;
        protected int value;
        protected int maxValue;
        protected Rectangle srcRectangle;
        protected Vector2 offset;
        protected int divisorValue;

        public HealthBar(int maxValue)
        {
            this.maxValue = maxValue;
            outline = TextureManager.healthbar_small_outline;
            fill = TextureManager.healthbar_small_fill;
            fill_bg = TextureManager.healthbar_small_fill_bg;
            srcRectangle = new Rectangle(1, 0, 9, 4);
            divisorValue = 9;
            //offset = new Vector2(1, 0);
        }

        public void SetCurrentValue(Vector2 pos, int value)
        {
            position = pos;
            this.value = value;
            srcRectangle.Width = (int)((value * divisorValue) / maxValue);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);


            spriteBatch.Draw(fill_bg, position, Color.White);

            spriteBatch.Draw(fill, position + offset, srcRectangle, Color.White);
            spriteBatch.Draw(outline, position, Color.White);

            //spriteBatch.End();
        }
    }

    public class DashBar : HealthBar
    {
        public DashBar(int maxValue) :base(maxValue)
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);

            if(value < maxValue)
            {

            
            spriteBatch.Draw(fill_bg, position, Color.CornflowerBlue);

            spriteBatch.Draw(fill, position + offset, srcRectangle, Color.AliceBlue);
            spriteBatch.Draw(outline, position, Color.White);
            }

            //spriteBatch.End();
        }
    }

    public class AmmoBar : HealthBar
    {
        public AmmoBar(int maxValue) : base(maxValue)
        {
            this.maxValue = maxValue;
            outline = TextureManager.ammobar_outline;
            fill = TextureManager.ammobar_fill;
            fill_bg = TextureManager.ammobar_fill_bg;
            srcRectangle = new Rectangle(4, 3, 68, 20);
            divisorValue = 68;
            offset = new Vector2(4, 3);
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);


            spriteBatch.Draw(fill_bg, position, Color.White);

            spriteBatch.Draw(fill, position + offset, srcRectangle, Color.White);
            spriteBatch.Draw(outline, position, Color.White);
            OutlinedText.DrawOutlinedText(spriteBatch, position + new Vector2(24, -4), TextureManager.smallestFont, value.ToString() + "/" + maxValue.ToString(), new Color(249, 196, 89), Color.White);

            //spriteBatch.End();
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
            outline = TextureManager.player_healthbar_outline;

            Color bgColor = new Color(52, 173, 50, 255);
            if(ProfileManager.ColorBlind)
            {
                spriteBatch.Draw(TextureManager.player_healthbar_fill_bg_alt, position, Color.White);
                bgColor = new Color(71, 124, 177, 255);
                outline = TextureManager.player_healthbar_outline_alt;
            }
            else
            {
                spriteBatch.Draw(fill_bg, position, Color.White);
            }

            

            if(maxValue != value)
            {
                spriteBatch.Draw(fill, position + new Vector2(80 - ((Math.Abs(value - maxValue) * 16)), 0), srcRectangle, Color.White);
            }
            
            spriteBatch.Draw(outline, position, Color.White);
            spriteBatch.Draw(inline, position, Color.White);

            OutlinedText.DrawOutlinedText(spriteBatch, position + new Vector2(28,-1), TextureManager.smallestFont, value.ToString() + "/" + maxValue.ToString(), bgColor, Color.White);

        }

    }
}
