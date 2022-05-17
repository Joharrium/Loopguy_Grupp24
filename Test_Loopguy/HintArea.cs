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
        X, A, Y, B, Select, Start, LT, RT, LB, RB
    }
    class HintArea
    {
        public Rectangle area;
        string text;
        InputIcon icon;
        Texture2D texture = TextureManager.control_atlas;
        public bool active;

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
            }
            else
            {
                active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(active)
            {
                int offset = (int)TextureManager.smallestFont.MeasureString(text).X;
                Vector2 drawPoint = new Vector2((Game1.windowX / 2) - (offset / 2), Game1.windowY * 0.8F);
                spriteBatch.Draw(texture, drawPoint - new Vector2(20, 2), new Rectangle(0, (int)icon * 32, 32, 32), Color.White);
                spriteBatch.DrawString(TextureManager.smallestFont, text, drawPoint + new Vector2(22, 2), Color.Black);
                spriteBatch.DrawString(TextureManager.smallestFont, text, drawPoint + new Vector2(20, 0), Color.White);
            }
            
        }
    }
}
