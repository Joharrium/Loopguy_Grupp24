using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Test_Loopguy
{
    internal class AnimSprite
    {
        Texture2D sheet;

        public Point size;
        Point currentFrame;
        Vector2 position;

        Rectangle frame;

        int timeSinceLastFrame;

        public AnimSprite(Texture2D sheet, Point size)
        {
            this.sheet = sheet;
            this.size = size;

            currentFrame = new Point();
        }

        public Vector2 Position { get; set; }

        public void Update(GameTime gameTime)
        {
            timeSinceLastFrame += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void Play(int row, int stopX, int frameRate)
        {
            currentFrame.Y = row;

            if (frameRate == 0)
            {
                currentFrame.X = stopX;
            }
            else
            {

                if (timeSinceLastFrame >= frameRate)
                {
                    timeSinceLastFrame = 0;
                    currentFrame.X++;
                    if (currentFrame.X >= stopX)
                    {
                        currentFrame.X = 0;
                    }
                }
            }
        }
        public void Frame(int X, int Y)
        {
            currentFrame.X = X;
            currentFrame.Y = Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            frame = new Rectangle(currentFrame.X * size.X, currentFrame.Y * size.Y, size.X, size.Y);

            position.X = (int)Math.Round(Position.X);
            position.Y = (int)Math.Round(Position.Y);
            spriteBatch.Draw(sheet, position, frame, Color.White);
        }
    }
}
