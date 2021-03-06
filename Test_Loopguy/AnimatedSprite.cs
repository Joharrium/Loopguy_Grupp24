using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Test_Loopguy
{
    public class AnimatedSprite
    {
        Texture2D sheet;
        SpriteEffects effects;

        public Point size;
        public Point currentFrame;

        Vector2 position;

        Rectangle frame;

        public int timeSinceLastFrame;

        public bool flipHorizontally, flipVertically;

        public AnimatedSprite(Texture2D sheet, Point size)
        {
            this.sheet = sheet;
            this.size = size;

            currentFrame = new Point(0, 0);
            effects = SpriteEffects.None;
        }

        public Vector2 Position { get; set; }

        public void Update(GameTime gameTime)
        {
            timeSinceLastFrame += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (!flipHorizontally && !flipVertically)
                effects = SpriteEffects.None;
            else if(flipHorizontally && flipVertically)
            {
                effects = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
            }
            else
            {
                if (flipHorizontally)
                    effects = SpriteEffects.FlipHorizontally;
                if (flipVertically)
                    effects = SpriteEffects.FlipVertically;
            }

        }

        public void Play(int row, int stopX, int frameTime)
        {
            currentFrame.Y = row;

            if (timeSinceLastFrame >= frameTime)
            {
                timeSinceLastFrame = 0;
                currentFrame.X++;
                if (currentFrame.X >= stopX)
                {
                    currentFrame.X = 0;
                }
            }
        }
        public bool PlayOnce(int row, int stopX, int frameTime)
        {
            currentFrame.Y = row;

            if (timeSinceLastFrame >= frameTime)
            {
                timeSinceLastFrame = 0;
                currentFrame.X++;
                if (currentFrame.X >= stopX)
                {
                    currentFrame.X = 0;
                    return false;
                }
            }

            return true;
        }
        public void Frame(int X, int Y)
        {
            currentFrame.X = X;
            currentFrame.Y = Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            frame = new Rectangle(currentFrame.X * size.X, currentFrame.Y * size.Y, size.X, size.Y);

            position = Position;

            //position.X = (int)Math.Round(Position.X);
            //position.Y = (int)Math.Round(Position.Y);

            spriteBatch.Draw(sheet, position, frame, Color.White, 0, Vector2.Zero, 1, effects, 0);
        }
        public void Draw(SpriteBatch spriteBatch, bool grayedOut)
        {
            frame = new Rectangle(currentFrame.X * size.X, currentFrame.Y * size.Y, size.X, size.Y);

            position = Position;

            Color color = Color.White;
            if (grayedOut)
            {
                color = new Color(80, 80, 80, 150);
            }

            //position.X = (int)Math.Round(Position.X);
            //position.Y = (int)Math.Round(Position.Y);

            spriteBatch.Draw(sheet, position, frame, color, 0, Vector2.Zero, 1, effects, 0);
        }
        public void DrawRotation(SpriteBatch spriteBatch, float angle)
        {
            frame = new Rectangle(currentFrame.X * size.X, currentFrame.Y * size.Y, size.X, size.Y);
            Vector2 origin = new Vector2(size.X / 2, size.Y / 2);

            position.X = Position.X + size.X / 2;
            position.Y = Position.Y + size.Y / 2;

            spriteBatch.Draw(sheet, position, frame, Color.White, -angle, origin, 1, SpriteEffects.None, 0);
        }
        public void DrawRotation(SpriteBatch spriteBatch, float angle, Vector2 origin)
        {
            frame = new Rectangle(currentFrame.X * size.X, currentFrame.Y * size.Y, size.X, size.Y);

            position.X = Position.X + size.X / 2;
            position.Y = Position.Y + size.Y / 2;

            spriteBatch.Draw(sheet, position, frame, Color.White, -angle, origin, 1, SpriteEffects.None, 0);
        }
        public void DrawElsewhere(SpriteBatch spriteBatch, Vector2 otherPos, byte alpha)
        {
            frame = new Rectangle(currentFrame.X * size.X, currentFrame.Y * size.Y, size.X, size.Y);

            spriteBatch.Draw(sheet, otherPos, frame, new Color(alpha, alpha, alpha, alpha));
        }

        public void ResetAnimation()
        {
            currentFrame.X = 0;
            timeSinceLastFrame = 0;
        }

    }
}
