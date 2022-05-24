using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Test_Loopguy.Content;

namespace Test_Loopguy
{
    internal class Character : MovingObject
    {

        protected enum Orientation
        {
            Zero,
            Up,
            Down,
            Left,
            Right,
        }

        //Made for defining the sound used for each type of weapon on each type of enemy, the sound is set in each one of the characters TakeDamage() method
        public enum DamageType
        {
            melee,
            laserGun,
            railGun,
            Hazard,
            Electricity,
        }

        //public AttackType currentAttack;

        protected Orientation primaryOrientation;
        protected Orientation secondaryOrientation;

        public AnimatedSprite sprite;

        protected const float pi = (float)Math.PI;
        protected const int meleeRange = 22;

        public Rectangle footprint;
        protected float traveledDistance = 35;
        protected Point footprintOffset;

        public int health;
        public int maxHealth;


        public Character(Vector2 position)
            : base(position)
        {

        }

        public void GetOrientation()
        {
            float absDirectionX = Math.Abs(direction.X);
            float absDirectionY = Math.Abs(direction.Y);

            //Orientation changes depending on direction
            if (direction.Y < 0 && absDirectionX < absDirectionY)
            {
                primaryOrientation = Orientation.Up;

                if (direction.X > 0.38f)
                    secondaryOrientation = Orientation.Right;
                else if (direction.X < -0.38f)
                    secondaryOrientation = Orientation.Left;
                else
                    secondaryOrientation = Orientation.Up;
            }
            else if (direction.Y > 0 && absDirectionX < absDirectionY)
            {
                primaryOrientation = Orientation.Down;

                if (direction.X > 0.38f)
                    secondaryOrientation = Orientation.Right;
                else if (direction.X < -0.38f)
                    secondaryOrientation = Orientation.Left;
                else
                    secondaryOrientation = Orientation.Down;
            }
            else if (direction.X < 0)
            {
                primaryOrientation = Orientation.Left;

                if (direction.Y < -0.38f)
                    secondaryOrientation = Orientation.Up;
                else if (direction.Y > 0.38f)
                    secondaryOrientation = Orientation.Down;
                else
                    secondaryOrientation = Orientation.Left;
            }
            else if (direction.X > 0)
            {
                primaryOrientation = Orientation.Right;

                if (direction.Y < -0.38f)
                    secondaryOrientation = Orientation.Up;
                else if (direction.Y > 0.38f)
                    secondaryOrientation = Orientation.Down;
                else
                    secondaryOrientation = Orientation.Right;
            }
        }

        public void AngleGetOrientation(float angle)
        {
            if (angle >= pi * 1.75f || angle < pi * 0.25f)
            {
                primaryOrientation = Orientation.Down;
                if (angle < pi * 0.25f && angle >= pi * 0.125f)
                    secondaryOrientation = Orientation.Right;
                else if (angle < pi * 1.875f && angle >= pi * 1.75f)
                    secondaryOrientation = Orientation.Left;
                else
                    secondaryOrientation = primaryOrientation;
            }
            else if (angle < pi * 0.75f && angle >= pi * 0.25f)
            {
                primaryOrientation = Orientation.Right;
                if (angle < pi * 0.275f)
                    secondaryOrientation = Orientation.Down;
                else if (angle >= pi * 0.625f)
                    secondaryOrientation = Orientation.Up;
                else
                    secondaryOrientation = primaryOrientation;
            }
            else if (angle < pi * 1.25f && angle >= pi * 0.75f)
            {
                primaryOrientation = Orientation.Up;
                if (angle < pi * 0.875f)
                    secondaryOrientation = Orientation.Right;
                else if (angle >= pi * 1.125f)
                    secondaryOrientation = Orientation.Left;
                else
                    secondaryOrientation = primaryOrientation;
            }
            else if (angle < pi * 1.75f && angle >= pi * 1.25f)
            {
                primaryOrientation = Orientation.Left;
                if (angle < pi * 1.375f)
                    secondaryOrientation = Orientation.Up;
                else if (angle >= pi * 1.625f)
                    secondaryOrientation = Orientation.Down;
                else
                    secondaryOrientation = primaryOrientation;
            }

        }

        public void CheckMovement(float deltaTime)
        {
            //calculating future position
            Vector2 futurePosCalc = position + direction * speed * deltaTime;
            Rectangle futureFootPrint = new Rectangle((int)futurePosCalc.X + footprintOffset.X, (int)futurePosCalc.Y + footprintOffset.Y, footprint.Width, footprint.Height);

            bool blockX = false;
            bool blockY = false;
            if(!LevelEditor.editingMode)
            {
                if (LevelManager.LevelObjectCollision(futureFootPrint, 0))
                {
                    //if first check returns a collision, check the same collision twice but only in x and y to see which direction causes collision
                    if (LevelManager.LevelObjectCollision(new Rectangle((int)futurePosCalc.X + footprintOffset.X, (int)position.Y + footprintOffset.Y, footprint.Width, footprint.Height), 0))
                    {
                        blockX = true;
                    }
                    if (LevelManager.LevelObjectCollision(new Rectangle((int)position.X + footprintOffset.X, (int)futurePosCalc.Y + footprintOffset.Y, footprint.Width, footprint.Height), 0))
                    {
                        blockY = true;
                    }

                    //lets the character move if at least one block failed
                    if (!blockX && blockY)
                    {
                        traveledDistance += Math.Abs(position.X - futurePosCalc.X);
                        position.X = futurePosCalc.X;

                    }


                    if (!blockY && blockX)
                    {
                        traveledDistance += Math.Abs(position.Y - futurePosCalc.Y);
                        position.Y = futurePosCalc.Y;
                    }
                }
                //lets character move normally
                else
                {
                    Vector2 delta = position - futurePosCalc;
                    traveledDistance += Math.Abs(delta.Length());
                    position += direction * speed * deltaTime;
                }
            }
            else
            {
                Vector2 delta = position - futurePosCalc;
                traveledDistance += Math.Abs(delta.Length());
                position += direction * speed * deltaTime;
            }
        }

        public virtual void TakeDamage(int damage, DamageType soundType)
        {
            health -= damage;
           
            if (health <= 0)
            {
                LevelManager.Reset();
            }
     
        }

        public bool MeleeHit(GameObject obj)
        {
            foreach (Vector2 v in LevelManager.GetPointsOfObject(obj))
            {
                if (Vector2.Distance(centerPosition, v) <= meleeRange)
                {

                    float angle = (float)Helper.GetAngle(centerPosition, v, 0);

                    if (primaryOrientation == Orientation.Down)
                    {
                        if (secondaryOrientation == Orientation.Down)
                        {
                            if (angle >= pi * 1.75f || angle < pi * 0.25f)
                                return true;
                        }
                        else if (secondaryOrientation == Orientation.Left)
                        {
                            if (angle >= pi * 1.5f)
                                return true;
                        }
                        else if (secondaryOrientation == Orientation.Right)
                        {
                            if (angle < pi * 0.5f)
                                return true;
                        }
                    }
                    else if (primaryOrientation == Orientation.Right)
                    {
                        if (secondaryOrientation == Orientation.Right)
                        {
                            if (angle >= pi * 0.25f && angle < pi * 0.75f)
                                return true;
                        }
                        else if (secondaryOrientation == Orientation.Up)
                        {
                            if (angle >= pi * 0.5f && angle < pi)
                                return true;
                        }
                        else if (secondaryOrientation == Orientation.Down)
                        {
                            if (angle < pi * 0.5f)
                                return true;
                        }
                    }
                    else if (primaryOrientation == Orientation.Up)
                    {
                        if (secondaryOrientation == Orientation.Up)
                        {
                            if (angle >= pi * 0.75f && angle < pi * 1.25f)
                                return true;
                        }
                        else if (secondaryOrientation == Orientation.Left)
                        {
                            if (angle >= pi && angle < pi * 1.5f)
                                return true;
                        }
                        else if (secondaryOrientation == Orientation.Right)
                        {
                            if (angle >= pi * 0.5f && angle < pi)
                                return true;
                        }

                    }
                    else
                    { //LEFT
                        if (secondaryOrientation == Orientation.Left)
                        {
                            if (angle >= pi * 1.25f && angle < pi * 1.75f)
                                return true;
                        }
                        else if (secondaryOrientation == Orientation.Up)
                        {
                            if (angle >= pi && angle < pi * 1.5f)
                                return true;
                        }
                        else if (secondaryOrientation == Orientation.Down)
                        {
                            if (angle >= pi * 1.5f)
                                return true;
                        }
                    }
                }
            }

            return false;
        }

        //I made this a method in Character instead of Player so that AndroidEnemy could use it, but I couldn't get it to work
        //without changing basically everything so AndroidEnemy has its own method for playing melee attacks instead :(
        public bool PlayEnergySwordMelee(float deltaTime, AnimatedSprite meleeSprite, int rowIntCharacterSprite, int frameTime, bool flipMelee)
        {
            int rowIntPlayer = rowIntCharacterSprite;
            int rowIntSword = (int)primaryOrientation - 1; //Wow dude??

            if (primaryOrientation == Orientation.Up)
            {//UP
                direction.X = 0;
                direction.Y = -1;

                meleeSprite.flipVertically = false;

                if(flipMelee)
                    sprite.flipHorizontally = meleeSprite.flipHorizontally = true;
                else
                    sprite.flipHorizontally = meleeSprite.flipHorizontally = false;

                if (secondaryOrientation == Orientation.Left)
                {
                    sprite.flipHorizontally = false;

                    if (flipMelee)
                    {
                        rowIntSword = 7;
                        rowIntPlayer = 9;
                        meleeSprite.flipVertically = true;
                        meleeSprite.flipHorizontally = true;
                    }
                    else
                    {
                        rowIntSword = 4;
                        rowIntPlayer = 8;
                    }
                    direction.X = -1;
                }
                else if (secondaryOrientation == Orientation.Right)
                {
                    sprite.flipHorizontally = false;

                    if (flipMelee)
                    {
                        rowIntSword = 5;
                        rowIntPlayer = 11;
                        meleeSprite.flipVertically = true;
                        meleeSprite.flipHorizontally = true;
                    }
                    else
                    {
                        rowIntSword = 6;
                        rowIntPlayer = 10;
                    }
                    direction.X = 1;
                }

            }
            else if (primaryOrientation == Orientation.Down)
            {//DOWN
                direction.X = 0;
                direction.Y = 1;

                meleeSprite.flipVertically = false;

                if (flipMelee)
                    sprite.flipHorizontally = meleeSprite.flipHorizontally = true;
                else
                    sprite.flipHorizontally = meleeSprite.flipHorizontally = false;

                if (secondaryOrientation == Orientation.Left)
                {
                    sprite.flipHorizontally = false;

                    if (flipMelee)
                    {
                        rowIntSword = 6;
                        rowIntPlayer = 9;
                        meleeSprite.flipVertically = true;
                        meleeSprite.flipHorizontally = true;
                    }
                    else
                    {
                        rowIntSword = 5;
                        rowIntPlayer = 8;
                    }
                    direction.X = -1;
                }
                else if (secondaryOrientation == Orientation.Right)
                {
                    sprite.flipHorizontally = false;

                    if (flipMelee)
                    {
                        rowIntSword = 4;
                        rowIntPlayer = 11;
                        meleeSprite.flipVertically = true;
                        meleeSprite.flipHorizontally = true;
                    }
                    else
                    {
                        rowIntSword = 7;
                        rowIntPlayer = 10;
                    }
                    direction.X = 1;
                }
            }
            else if (primaryOrientation == Orientation.Left)
            {//LEFT
                direction.X = -1;
                direction.Y = 0;

                meleeSprite.flipHorizontally = false;

                if (flipMelee)
                {
                    rowIntPlayer = 9;
                    meleeSprite.flipVertically = true;
                }
                else
                {
                    rowIntPlayer = 8;
                    meleeSprite.flipVertically = false;
                }

                if (secondaryOrientation == Orientation.Up)
                {
                    if (flipMelee)
                    {
                        rowIntSword = 7;
                        meleeSprite.flipVertically = true;
                        meleeSprite.flipHorizontally = true;
                    }
                    else
                    {
                        rowIntSword = 4;
                    }
                    direction.Y = -1;
                }
                else if (secondaryOrientation == Orientation.Down)
                {
                    if (flipMelee)
                    {
                        rowIntSword = 6;
                        meleeSprite.flipVertically = true;
                        meleeSprite.flipHorizontally = true;
                    }
                    else
                    {
                        rowIntSword = 5;
                    }
                    direction.Y = 1;
                }
            }
            else
            {//RIGHT
                direction.X = 1;
                direction.Y = 0;

                meleeSprite.flipHorizontally = false;

                if (flipMelee)
                {
                    rowIntPlayer = 11;
                    meleeSprite.flipVertically = true;
                }
                else
                {
                    rowIntPlayer = 10;
                    meleeSprite.flipVertically = false;
                }

                if (secondaryOrientation == Orientation.Up)
                {
                    if (flipMelee)
                    {
                        rowIntSword = 5;
                        meleeSprite.flipVertically = true;
                        meleeSprite.flipHorizontally = true;
                    }
                    else
                    {
                        rowIntSword = 6;
                    }
                    direction.Y = -1;
                }
                else if (secondaryOrientation == Orientation.Down)
                {
                    if (flipMelee)
                    {
                        rowIntSword = 4;
                        meleeSprite.flipVertically = true;
                        meleeSprite.flipHorizontally = true;
                    }
                    else
                    {
                        rowIntSword = 7;
                    }
                    direction.Y = 1;
                }
            }

            direction.Normalize();

            CheckMovement(deltaTime);

            sprite.Play(rowIntPlayer, 4, frameTime);

            //The PlayOnce method returns false when the animation is done playing!!!
            return meleeSprite.PlayOnce(rowIntSword, 4, frameTime);
        }

        public void DrawLasersight(SpriteBatch spriteBatch, Vector2 direction, Texture2D linePixel, Texture2D lineDot, Random rand)
        {
            
            //LINE COLLISION FOR LASER SIGHT OMG
            Line laserLine = new Line(centerPosition, new Vector2(centerPosition.X + 580 * direction.X, centerPosition.Y + 580 * direction.Y));

            LevelManager.LevelObjectCollision(laserLine, 9);

            Line newLaserLine = new Line(centerPosition, laserLine.intersectionPoint);
            int laserLength = (int)newLaserLine.Length();

            for (int i = 16; i < laserLength; i++)
            {
                Vector2 aimPoint = new Vector2(centerPosition.X + i * direction.X, centerPosition.Y + i * direction.Y);
                spriteBatch.Draw(linePixel, aimPoint, Helper.RandomTransparency(rand, 0, 90));
            }

            Vector2 dotPos = new Vector2(laserLine.intersectionPoint.X - 1 + (direction.X * 5), laserLine.intersectionPoint.Y - 1 + (direction.Y * 5));

            spriteBatch.Draw(lineDot, dotPos, Color.White);
        }
    }
}
