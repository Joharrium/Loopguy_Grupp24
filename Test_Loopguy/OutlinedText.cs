using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Test_Loopguy.Content;

namespace Test_Loopguy
{
    static class OutlinedText
    {
        public static void DrawOutlinedText(SpriteBatch spriteBatch, Vector2 position, SpriteFont font, string text)
        {
            spriteBatch.DrawString(font, text, position + new Vector2(1, 1), Color.Black);
            spriteBatch.DrawString(font, text, position + new Vector2(1, 0), Color.Black);
            spriteBatch.DrawString(font, text, position + new Vector2(1, -1), Color.Black);
            spriteBatch.DrawString(font, text, position + new Vector2(0, 1), Color.Black);
            spriteBatch.DrawString(font, text, position + new Vector2(0, -1), Color.Black);
            spriteBatch.DrawString(font, text, position + new Vector2(-1, 1), Color.Black);
            spriteBatch.DrawString(font, text, position + new Vector2(-1, 0), Color.Black);
            spriteBatch.DrawString(font, text, position + new Vector2(-1, -1), Color.Black);
            spriteBatch.DrawString(font, text, position + new Vector2(0, 0), Color.White);
        }
        public static void DrawOutlinedText(SpriteBatch spriteBatch, Vector2 position, SpriteFont font, string text, Color backgroundColor, Color fontColor)
        {
            spriteBatch.DrawString(font, text, position + new Vector2(1, 1), backgroundColor);
            spriteBatch.DrawString(font, text, position + new Vector2(1, 0), backgroundColor);
            spriteBatch.DrawString(font, text, position + new Vector2(1, -1), backgroundColor);
            spriteBatch.DrawString(font, text, position + new Vector2(0, 1), backgroundColor);
            spriteBatch.DrawString(font, text, position + new Vector2(0, -1), backgroundColor);
            spriteBatch.DrawString(font, text, position + new Vector2(-1, 1), backgroundColor);
            spriteBatch.DrawString(font, text, position + new Vector2(-1, 0), backgroundColor);
            spriteBatch.DrawString(font, text, position + new Vector2(-1, -1), backgroundColor);
            spriteBatch.DrawString(font, text, position + new Vector2(0, 0), fontColor);
        }
    }
}
