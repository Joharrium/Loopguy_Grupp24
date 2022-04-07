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
        AnimSprite meleeSprite;

        Random random = new Random();

        //Wtf
        public Vector2 cameraPosition;
        Vector2 gunDirection;
        Vector2 prevDirection;

        public bool usedGate;

        float aimAngle;
        const float pi = (float)Math.PI;

        int dirInt;

        public string playerInfoString;

        bool attacking;

        public Player(Vector2 position)
            : base(position)
        {
            sprite = new AnimSprite(TexMGR.playerSheet, new Point(32, 32));
            gunSprite = new AnimSprite(TexMGR.gunSheet, new Point(32, 32));
            meleeSprite = new AnimSprite(TexMGR.meleeFx, new Point(48, 48));

            speed = 100;

            dirInt = 2;

            attacking = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (attacking)
            {
                Melee(deltaTime);
            }
            else
            {
                if (InputReader.Aim())
                {
                    cameraPosition = centerPosition + gunDirection * 50;
                    Game1.camera.stabilize = true;
                }
                else
                {
                    Game1.camera.stabilize = false;

                    if (direction == Vector2.Zero)
                        cameraPosition = centerPosition + prevDirection * 30;
                    else
                        cameraPosition = centerPosition + direction * 30;

                    gunDirection = Vector2.Zero;
                    Movement(deltaTime);

                    if (InputReader.Melee() && !attacking)
                    {
                        meleeSprite.currentFrame.X = 0;
                        meleeSprite.timeSinceLastFrame = 0;
                        sprite.currentFrame.X = 0;
                        sprite.timeSinceLastFrame = 0;

                        attacking = true;
                    }
                }
            }

            sprite.Position = position;
            gunSprite.Position = position;
            meleeSprite.Position = new Vector2(position.X - 8, position.Y - 8);
            meleeSprite.Update(gameTime);
            sprite.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (attacking)
            {
                sprite.Draw(spriteBatch);
                meleeSprite.Draw(spriteBatch);
            }
            else
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
                    if (attacking)
                        meleeSprite.Draw(spriteBatch);
                    sprite.Draw(spriteBatch);

                    if (InputReader.Aim())
                    {
                        DrawAim(spriteBatch);
                        DrawGun(spriteBatch);
                    }
                }
            }
        }

        public void Melee(float deltaTime)
        {
            int rowInt = dirInt - 1; //Wow dude??
            int frameTime = 50;

            if (dirInt == 1)
            {//UP
                sprite.Play(6, 4, frameTime);
                direction.X = 0;
                direction.Y = -1;
            }
            else if (dirInt == 2)
            {//DOWN
                sprite.Play(7, 4, frameTime);
                direction.X = 0;
                direction.Y = 1;
            }
            else if (dirInt == 3)
            {//LEFT
                sprite.Play(8, 4, frameTime);
                direction.X = -1;
                direction.Y = 0;
            }
            else
            {//RIGHT
                sprite.Play(9, 4, frameTime);
                direction.X = 1;
                direction.Y = 0;
            }

            Vector2 futurepos = centerPosition + direction * speed * deltaTime + new Vector2(0, 12);

            if (LevelManager.LevelObjectCollision(futurepos) || LevelManager.WallCollision(futurepos))
            {

            }
            else
            {
                position += direction * speed / 2 * deltaTime;
            }
            

            attacking = meleeSprite.PlayOnce(rowInt, 4, frameTime);

        }

        public void Dash(float deltaTime)
        {
            for
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

            int frameTime = 0;

            if (absDirection != 0)
            {
                frameTime = (int)(50 / absDirection);
            }

            //Visual changes depending on direction
            if (direction.Y < 0 && absDirectionX < absDirectionY)
            {//UP
                sprite.Play(0, 12, frameTime);
                dirInt = 1;
            }
            else if (direction.Y > 0 && absDirectionX < absDirectionY)
            {//DOWN
                sprite.Play(1, 12, frameTime);
                dirInt = 2;
            }
            else if (direction.X < 0)
            {//LEFT
                sprite.Play(2, 12, frameTime);
                dirInt = 3;
            }
            else if (direction.X > 0)
            {//RIGHT
                sprite.Play(3, 12, frameTime);
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

            LevelManager.CheckGate(this);
            

            //if (WallManager.CheckCollision(centerPosition + direction * speed * deltaTime + new Vector2(0, 12)))
            {
                
            }
            //else
            {
                //position += direction * speed * deltaTime;
            }

            Vector2 velocity = direction * speed;
            float playerVelocityShort = velocity.Length();

            float absDirShort = (float)Math.Round(absDirection, 2);
            playerInfoString = absDirShort.ToString() + " || " + frameTime.ToString() + " || " + playerVelocityShort.ToString();
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
    }
}
