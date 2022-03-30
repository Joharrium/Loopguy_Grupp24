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

        //Wtf
        public Vector2 cameraPosition;
        Vector2 gunDirection;
        Vector2 prevDirection;

        float aimAngle;
        const float pi = (float)Math.PI;

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
                cameraPosition = centerPosition + gunDirection * 50;
            }
            else
            {
                if (direction == Vector2.Zero)
                    cameraPosition = centerPosition + prevDirection * 30;
                else
                    cameraPosition = centerPosition + direction * 30;

                gunDirection = Vector2.Zero;
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
            if (aimAngle > pi * 1.75f || aimAngle < pi * 0.25f)
                sprite.Frame(1, 4);
            else if (aimAngle < pi * 0.75f)
                sprite.Frame(3, 4);
            else if (aimAngle < pi * 1.25f)
                sprite.Frame(0, 4);
            else
                sprite.Frame(2, 4);

            if (!InputReader.MovingLeftStick())
            {
                aimAngle = (float)Helper.GetAngle(centerPosition, Game1.mousePos);
            }
            else
            {
                aimAngle = InputReader.LeftStickAngle();
            }

            gunDirection = new Vector2((float)Math.Sin(aimAngle), (float)Math.Cos(aimAngle));

            for (int i = 0; i < 560; i++)
            {
                Vector2 aimPoint = new Vector2(centerPosition.X + i * gunDirection.X, centerPosition.Y + i * gunDirection.Y);
                spriteBatch.Draw(TexMGR.pinkPixel, aimPoint, Helper.RandomTransparency(random, 0, 90));
            }
        }

        public override void Movement(float deltaTime)
        {
            direction.Y = 0;
            direction.X = 0;

            if (InputReader.MovingLeftStick())
            {
                speed = InputReader.LeftStickLength() * 100;
                direction.X = InputReader.padState.ThumbSticks.Left.X;
                direction.Y = -InputReader.padState.ThumbSticks.Left.Y;
            }
            else
            {
                speed = 100;
                if (InputReader.MovementLeft())
                    direction.X -= 1;
                if (InputReader.MovementRight())
                    direction.X += 1;
                if (InputReader.MovementUp())
                    direction.Y -= 1;
                if (InputReader.MovementDown())
                    direction.Y += 1;
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
                    sprite.Frame(0, 4);
                else if (dirint == 2)
                    sprite.Frame(1, 4);
                else if (dirint == 3)
                    sprite.Frame(2, 4);
                else
                    sprite.Frame(3, 4);
            }

            //This normalizes the direction Vector so that movement is consistent in all directions. If it normalizes a Vector of 0,0 it gets fucky though
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
                prevDirection = direction;
            }

            position += direction * speed * deltaTime;

            Vector2 velocity = direction * speed;
            float playerVelocityShort = velocity.Length();

            float absDirShort = (float)Math.Round(absDirection, 2);
            playerInfoString = absDirShort.ToString() + " || " + frameRate.ToString() + " || " + playerVelocityShort.ToString();
        }
    }
}
