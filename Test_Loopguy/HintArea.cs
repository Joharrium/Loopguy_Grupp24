using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Test_Loopguy.Content;

namespace Test_Loopguy
{
    public enum InputIcon
    {
        X, A, Y, B, Select, Start, LT, RT, LB, RB, None
    }
    class HintArea
    {
        public Rectangle area;
        string text;
        InputIcon icon;
        Texture2D texture = TextureManager.control_atlas;
        public bool active;
        Color color = new Color(255, 255, 255, 0);
        Color bgColor = new Color(0, 0, 0, 0);
        private readonly byte colorIncrement = 15;

        public HintArea(Rectangle area, string text, InputIcon icon)
        {
            this.area = area;
            this.text = text;
            this.icon = icon;
        }

        public void Update()
        {
            if(area.Contains(EntityManager.player.centerPosition))
            {
                active = true;
                FadeIn();
            }
            else
            {
                active = false;
                FadeOut();
            }
        }

        private void FadeIn()
        {
            if(color.A < 255)
            {
                if(color.A + colorIncrement > 255)
                {
                    color.A = 255;
                    bgColor.A = 255;
                }
                else
                {
                    color.A += colorIncrement;
                    bgColor.A += colorIncrement;
                }
                
            }
            
        }

        private void FadeOut()
        {
            if (color.A > 0)
            {
                if (color.A - colorIncrement < 0)
                {
                    color.A = 0;
                    bgColor.A = 0;
                }
                else
                {
                    color.A -= colorIncrement;
                    bgColor.A -= colorIncrement;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, bool multipleDraw)
        {
            
            if(active || color.A > 0)
            {
                int offset = (int)TextureManager.smallestFont.MeasureString(text).X;
                Vector2 drawPoint = new Vector2((Game1.windowX / 2) - (offset / 2), Game1.windowY * 0.7F);
                if (multipleDraw)
                {
                    drawPoint = new Vector2((Game1.windowX / 2) - (offset / 2), Game1.windowY * 0.8F);
                }
                if(icon == InputIcon.None)
                {
                    drawPoint.X -= 22;
                }
                
                
                spriteBatch.Draw(texture, drawPoint - new Vector2(20 * ((int)InputReader.controllerMode + 1), 2), new Rectangle(32 * (int)InputReader.controllerMode, (int)icon * 32, 32 * ((int)InputReader.controllerMode + 1), 32), color);

                OutlinedText.DrawOutlinedText(spriteBatch, drawPoint + new Vector2(22, 0), TextureManager.smallestFont, text, bgColor, color);
            }
            
        }
    }
}
