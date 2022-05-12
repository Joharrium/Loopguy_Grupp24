using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test_Loopguy
{
    class Slider : Component
    {
        Texture2D outline, fill;
        int maxValue;
        int currentValue;

        public int Value
        {
            get { return currentValue; }
        }

        Rectangle sourceRectangle = new Rectangle(0, 0, 300, 19);
        Vector2 position;

        public override bool isHovering { get; set; }

        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X - 25, (int)position.Y + 2, outline.Width, outline.Height);
            }
        }


        public Slider(Vector2 position, int maxValue)
        {
            this.position = position - new Vector2(30, 0);
            this.maxValue = maxValue;
            currentValue = maxValue / 2;
            fill = TextureManager.slider_fill;
            outline = TextureManager.slider_container;
        }

        public override void Update(GameTime gameTime)
        {
            if(isHovering)
            {
                DoMovement();
            }
        }

        public void DoMovement()
        {
            if (InputReader.MovementLeft())
            {
                CheckValue(currentValue - 1);
            }
            if (InputReader.MovementRight())
            {
                CheckValue(currentValue + 1);
            }
        }

        private void CheckValue(int newValue)
        {
            if(newValue < 0)
            {
                currentValue = 0;
            }
            else if(newValue > maxValue)
            {
                currentValue = maxValue;
            }
            else
            {
                currentValue = newValue;
            }
            sourceRectangle.Width = (int)((currentValue * 300) / maxValue);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if(isHovering)
            {
                spriteBatch.Draw(TextureManager.UI_selectedMenuBox, Rectangle, Color.White);
                spriteBatch.Draw(outline, position, Color.White);
                spriteBatch.Draw(fill, position, sourceRectangle, Color.White);
                
            }
            else
            {
                spriteBatch.Draw(outline, position, Color.DarkGray);
                spriteBatch.Draw(fill, position, sourceRectangle, Color.DarkGray);
            }


            

        }
    }
}
