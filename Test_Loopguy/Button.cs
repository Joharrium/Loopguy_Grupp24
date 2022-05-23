using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test_Loopguy
{
    class Button : Component
    {

        MouseState currentMouse;
        MouseState previousMouse;

        SpriteFont font;
        Texture2D texture;

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color TextColor { get; set; }

        public Vector2 Position { get; set; }

        public string Text { get; set; }

        public override bool isHovering { get; set; }
        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X - 25, (int)Position.Y + 2, texture.Width, texture.Height);
            }
        }


        public Button(Texture2D aTexture, SpriteFont aFont, Vector2 aPosition)
        {
            font = aFont;
            texture = aTexture;
            Position = aPosition;


        }

        public override void Update(GameTime gameTime)
        {
            Vector2 windowMousePos = new Vector2(InputReader.mouseState.X / Game1.windowScale, InputReader.mouseState.Y / Game1.windowScale);

            //isHovering = false;

            if (Rectangle.Contains(windowMousePos))
            {
                //isHovering = true;

                if (InputReader.mouseState.LeftButton == ButtonState.Released && InputReader.oldMouseState.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());

                    //if (Click != null)
                    //{
                    //    Click(this, new EventArgs());
                    //}
                }
            }
            //else
            //{
            //    isHovering = false;
            //}


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color = Color.Transparent;
            TextColor = Color.White;

            if (isHovering)
            {
                color = Color.White;
                TextColor = Color.Black;
            }


            spriteBatch.Draw(texture, Rectangle, color);

            if (!string.IsNullOrEmpty(Text))
            {
                //var x = (Rectangle.X + (Rectangle.Width / 2)) - (font.MeasureString(Text).X / 2);
                //var y = (Rectangle.Y + (Rectangle.Width / 2)) - (font.MeasureString(Text).Y / 2);

                if(!isHovering)
                {
                    OutlinedText.DrawOutlinedText(spriteBatch, Position, font, Text, Color.Black, TextColor);
                }
                else
                {
                    spriteBatch.DrawString(font, Text, Position, TextColor);
                }
                

                

            }
        }
    }
}
