using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    internal class Player : AnimatedMovingObject
    {
        AnimSprite gunSprite;

        Random random = new Random();

        //Wtf
        public Vector2 cameraPosition;
        Vector2 gunDirection;
        Vector2 prevDirection;

        float aimAngle;
        const float pi = (float)Math.PI;

        int dirInt;

        public string playerInfoString;

        public Player(Vector2 position)
            : base(position)
        {
            sprite = new AnimSprite(TexMGR.playerSheet, new Point(32, 32));
            gunSprite = new AnimSprite(TexMGR.gunSheet, new Point(32, 32));

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
            gunSprite.Position = position;
            sprite.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (dirInt == 1)
            { //if aiming up, draw player sprite on top

                if (InputReader.Aim())
                {
                    DrawAim(spriteBatch);
                    DrawGun(spriteBatch);
                }

                sprite.Draw(spriteBatch);
            }
            else
            { //if not aiming up, draw gun sprite on top
                sprite.Draw(spriteBatch);

                if (InputReader.Aim())
                {
                    DrawAim(spriteBatch);
                    DrawGun(spriteBatch);
                }
            }
        }

        public void DrawAim(SpriteBatch spriteBatch)
        {
            if (aimAngle > pi * 1.75f || aimAngle < pi * 0.25f)
            {//DOWN
                sprite.Frame(1, 5);
                gunSprite.Frame(1, 0);
                dirInt = 2;
            }
            else if (aimAngle < pi * 0.75f)
            {//RIGHT
                sprite.Frame(3, 5);
                gunSprite.Frame(3, 0);
                dirInt = 4;
            }
            else if (aimAngle < pi * 1.25f)
            {//UP
                sprite.Frame(0, 5);
                gunSprite.Frame(0, 0);
                dirInt = 1;
            }
            else
            {//LEFT
                sprite.Frame(2, 5);
                gunSprite.Frame(2, 0);
                dirInt = 3;
            }

            if (!InputReader.MovingLeftStick())
            {
                aimAngle = (float)Helper.GetAngle(centerPosition, Game1.mousePos, 0);
            }
            else
            {
                aimAngle = InputReader.LeftStickAngle(0);
            }

            gunDirection = new Vector2((float)Math.Sin(aimAngle), (float)Math.Cos(aimAngle));

            for (int i = 16; i < 580; i++)
            {
                Vector2 aimPoint = new Vector2(centerPosition.X + i * gunDirection.X, centerPosition.Y + i * gunDirection.Y);
                spriteBatch.Draw(TexMGR.cyanPixel, aimPoint, Helper.RandomTransparency(random, 0, 90));
            }
        }

        public void DrawGun(SpriteBatch spriteBatch)
        {
            float gunRotation;
            double angleOffset;

            //These are ordered in a way that makes perfect sense, shut up
            if (dirInt == 2)//down
                angleOffset = 0;
            else if (dirInt == 3)//right
                angleOffset = -1.5 * pi;
            else if (dirInt == 1)//up
                angleOffset = -1 * pi;
            else//left
                angleOffset = -0.5 * pi;

            if (!InputReader.MovingLeftStick())
                gunRotation = (float)Helper.GetAngle(centerPosition, Game1.mousePos, angleOffset);
            else
                gunRotation = InputReader.LeftStickAngle((float)angleOffset);

            gunSprite.DrawRotation(spriteBatch, gunRotation);
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
            {//UP
                sprite.Play(0, 12, frameRate);
                dirInt = 1;
            }
            else if (direction.Y > 0 && absDirectionX < absDirectionY)
            {//DOWN
                sprite.Play(1, 11, frameRate);
                dirInt = 2;
            }
            else if (direction.X < 0)
            {//LEFT
                sprite.Play(2, 11, frameRate);
                dirInt = 3;
            }
            else if (direction.X > 0)
            {//RIGHT
                sprite.Play(3, 11, frameRate);
                dirInt = 4;
            }
            else
            {
                if (dirInt == 1)
                    sprite.Frame(0, 4);
                else if (dirInt == 2)
                    sprite.Frame(1, 4);
                else if (dirInt == 3)
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
            Vector2 futurepos = centerPosition + direction * speed * deltaTime + new Vector2(0, 12);
            if (LevelManager.LevelObjectCollision(futurepos) || LevelManager.WallCollision(futurepos))
            {

            }
            else
            {
                position += direction * speed * deltaTime;
            }

            Vector2 velocity = direction * speed;
            float playerVelocityShort = velocity.Length();

            float absDirShort = (float)Math.Round(absDirection, 2);
            playerInfoString = absDirShort.ToString() + " || " + frameRate.ToString() + " || " + playerVelocityShort.ToString();
        }
    }
}
