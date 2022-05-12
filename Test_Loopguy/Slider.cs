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
        bool incrementalMovement;
        string text;

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
                return new Rectangle((int)position.X, (int)position.Y + 20, outline.Width, outline.Height);
            }
        }


        public Slider(Vector2 position, int maxValue, string text,  bool incremental)
        {
            this.position = position - new Vector2(30, 0);
            this.maxValue = maxValue;
            currentValue = maxValue / 2;
            fill = TextureManager.slider_fill;
            outline = TextureManager.slider_container;
            sourceRectangle.Width = 150;
            this.incrementalMovement = incremental;
            this.text = text;
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
            if ((InputReader.MovementLeft() && !incrementalMovement) || (InputReader.MovementLeftNonContinous() && incrementalMovement))
            {
                CheckValue(currentValue - 1);
            }
           if ((InputReader.MovementRight() && !incrementalMovement) || (InputReader.MovementRightNonContinous() && incrementalMovement))
            {
                CheckValue(currentValue + 1);
            }
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                CheckValue((int)((InputReader.mouseState.Position.X / Game1.windowScale) - position.X) * maxValue / 300);
                if((int)((InputReader.mouseState.Position.X / Game1.windowScale) - position.X) > 295)
                {
                    CheckValue(maxValue);
                }
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
            if(sourceRectangle.Width > 295)
            {
                currentValue = maxValue;
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if(isHovering)
            {
                spriteBatch.Draw(TextureManager.UI_selectedMenuBox, Rectangle, Color.White);
                spriteBatch.Draw(outline, position + new Vector2(0, 20), Color.White);
                spriteBatch.Draw(fill, position + new Vector2(0, 20), sourceRectangle, Color.White);
                
            }
            else
            {
                spriteBatch.Draw(outline, position + new Vector2(0,20), Color.DarkGray);
                spriteBatch.Draw(fill, position + new Vector2(0, 20), sourceRectangle, Color.DarkGray);
            }

            spriteBatch.DrawString(TextureManager.UI_menuFont, text, position, Color.White);

            

        }
    }
}
