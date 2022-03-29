using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    internal class Player : AnimatedMovingObject
    {
        Random random = new Random();

        const float diagonalMultiplier = 0.707f;

        int dirint;

        public string playerInfoString;

        public Player(Vector2 position)
            : base(position)
        {
            sprite = new AnimSprite(TexMGR.playerSheet, new Point(32, 32));

            speed = 100;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (InputReader.Aim())
            {

            }
            else
            {
                Movement(deltaTime);
            }

            sprite.Position = position;
            sprite.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (InputReader.Aim())
            {
                DrawShot(spriteBatch);
            }
            sprite.Draw(spriteBatch);
        }

        public void DrawShot(SpriteBatch spriteBatch)
        {
            float angle;

            if (!InputReader.MovingLeftStick())
            {
                angle = (float)Helper.GetAngle(centerPosition, Game1.mousePos);
            }
            else
            {
                angle = InputReader.LeftStickAngle();
            }

            Vector2 gunDirection = new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));

            for (int i = 0; i < 560; i++)
            {
                Vector2 aimPoint = new Vector2(centerPosition.X + i * gunDirection.X, centerPosition.Y + i * gunDirection.Y);
                spriteBatch.Draw(TexMGR.cyanPixel, aimPoint, Helper.RandomTransparency(random));
            }
        }

        public override void Movement(float deltaTime)
        {

            if (InputReader.MovementLeft())
            {
                if (InputReader.MovementUp())
                {//LEFT + UP
                    direction.Y = -1;
                    direction.X = -1;
                }
                else if (InputReader.MovementDown())
                {//LEFT + DOWN
                    direction.Y = 1;
                    direction.X = -1;
                }
                else
                {//LEFT
                    direction.Y = 0;
                    direction.X = -1;
                }
            }
            else if (InputReader.MovementRight())
            {
                if (InputReader.MovementUp())
                {//RIGHT + UP
                    direction.Y = -1;
                    direction.X = 1;
                }
                else if (InputReader.MovementDown())
                {//RIGHT + DOWN
                    direction.Y = 1;
                    direction.X = 1;
                }
                else
                {//RIGHT
                    direction.Y = 0;
                    direction.X = 1;
                }
            }
            else if (InputReader.MovementUp())
            {//UP
                direction.Y = -1;
                direction.X = 0;
            }
            else if (InputReader.MovementDown())
            {//DOWN
                direction.Y = 1;
                direction.X = 0;
            }
            else
            {//Analog Stick movement
                direction.X = InputReader.padState.ThumbSticks.Left.X;
                direction.Y = -InputReader.padState.ThumbSticks.Left.Y;
            }

            float absDirection = Math.Abs(direction.X) + Math.Abs(direction.Y);
            float absDirectionX = Math.Abs(direction.X);
            float absDirectionY = Math.Abs(direction.Y);


            //Changes frame rate depending on direction vector

            if (absDirection > 1)
                absDirection = 1;
            else if (absDirection != 0 && absDirection < 0.2f)
                absDirection = 0.2f;

            int frameRate = 0;

            if (absDirection != 0)
            {
                frameRate = (int)(50 / absDirection);
            }


            //Visual changes depending on direction
            if (direction.Y < 0 && absDirectionX < absDirectionY)
            {
                sprite.Play(0, 12, frameRate);
                dirint = 1;
            }
            else if (direction.Y > 0 && absDirectionX < absDirectionY)
            {
                sprite.Play(1, 11, frameRate);
                dirint = 2;
            }
            else if (direction.X < 0)
            {
                sprite.Play(2, 11, frameRate);
                dirint = 3;
            }
            else if (direction.X > 0)
            {
                sprite.Play(3, 11, frameRate);
                dirint = 4;
            }
            else
            {
                if (dirint == 1)
                    sprite.Play(4, 0, 0);
                else if (dirint == 2)
                    sprite.Play(5, 0, 0);
                else if (dirint == 3)
                    sprite.Play(6, 0, 0);
                else
                    sprite.Play(7, 0, 0);
            }

            //This normalizes the direction Vector so that movement is consistent in all directions. If it normalizes a Vector of 0,0 it gets fucky though
            if(direction != Vector2.Zero)
                direction.Normalize();

            if(TileManager.CheckCollision(centerPosition + direction * speed * deltaTime + new Vector2(0, 12)))
            {
                position += direction * speed * deltaTime;
            }
            

            

            float playerVelocityShort = (float)Math.Round((Math.Abs(direction.X) * speed) + (Math.Abs(direction.Y) * speed));

            float absDirShort = (float)Math.Round((double)absDirection, 2);
            playerInfoString = absDirShort.ToString() + " || " + frameRate.ToString() + " || " + playerVelocityShort.ToString();
        }
    }
}
