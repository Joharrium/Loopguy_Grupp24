using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    internal class Player : AnimatedMovingObject
    {

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

            sprite.Position = position;
            sprite.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

        public override void Movement(float deltaTime)
        {

            if (InputReader.MovementLeft())
            {
                if (InputReader.MovementUp())
                {//LEFT + UP
                    direction.Y = -1 * diagonalMultiplier;
                    direction.X = -1 * diagonalMultiplier;
                }
                else if (InputReader.MovementDown())
                {//LEFT + DOWN
                    direction.Y = 1 * diagonalMultiplier;
                    direction.X = -1 * diagonalMultiplier;
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
                    direction.Y = -1 * diagonalMultiplier;
                    direction.X = 1 * diagonalMultiplier;
                }
                else if (InputReader.MovementDown())
                {//RIGHT + DOWN
                    direction.Y = 1 * diagonalMultiplier;
                    direction.X = 1 * diagonalMultiplier;
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

            float absPlayerDirection = Math.Abs(direction.X) + Math.Abs(direction.Y);
            float absPlayerDirectionX = Math.Abs(direction.X);
            float absPlayerDirectionY = Math.Abs(direction.Y);


            //Changes frame rate depending on direction vector

            float absoluteDirection = (absPlayerDirection);
            if (absoluteDirection > 1)
                absoluteDirection = 1;
            else if (absoluteDirection != 0 && absoluteDirection < 0.2f)
                absoluteDirection = 0.2f;

            int frameRate = 0;

            if (absoluteDirection != 0)
            {
                frameRate = (int)(50 / absoluteDirection);
            }


            //Visual changes depending on direction
            if (direction.Y < 0 && absPlayerDirectionX < absPlayerDirectionY)
            {
                sprite.Play(0, 12, frameRate);
                dirint = 1;
            }
            else if (direction.Y > 0 && absPlayerDirectionX < absPlayerDirectionY)
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

            position += direction * speed * deltaTime;

            //Player velocity is higher when going diagonally even though it is theoretically corrected. Is this inherent to pixel stuff maybe?
            //If I change directionMultiplier to 0.5 instead of 0.707 the velocity stays the same but it feels slow!
            float playerVelocityShort = (float)Math.Round((Math.Abs(direction.X) * speed) + (Math.Abs(direction.Y) * speed));

            float absDirShort = (float)Math.Round((double)absoluteDirection, 2);
            playerInfoString = absDirShort.ToString() + " || " + frameRate.ToString() + " || " + playerVelocityShort.ToString();
        }
    }
}
