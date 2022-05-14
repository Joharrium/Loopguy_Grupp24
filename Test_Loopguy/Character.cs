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
        protected Orientation primaryOrientation;
        protected Orientation secondaryOrientation;

        protected AnimatedSprite sprite;

        protected const float pi = (float)Math.PI;
        protected const int meleeRange = 22;

        public Rectangle footprint;
        protected float traveledDistance = 35;

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

        public void CheckMovement(float deltaTime)
        {
            Vector2 futurePosCalc = position + direction * speed * deltaTime;
            Rectangle futureFootPrint = new Rectangle((int)futurePosCalc.X + 12, (int)futurePosCalc.Y + 24, footprint.Width, footprint.Height);

            bool blockX = false;
            bool blockY = false;
            if (LevelManager.LevelObjectCollision(futureFootPrint, 0))
            {
                if (LevelManager.LevelObjectCollision(new Rectangle((int)futurePosCalc.X + 12, (int)position.Y + 24, footprint.Width, footprint.Height), 0))
                {
                    blockX = true;
                }
                if (LevelManager.LevelObjectCollision(new Rectangle((int)position.X + 12, (int)futurePosCalc.Y + 24, footprint.Width, footprint.Height), 0))
                {
                    blockY = true;
                }
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
            else
            {
                Vector2 delta = position - futurePosCalc;
                traveledDistance += Math.Abs(delta.Length());
                position += direction * speed * deltaTime;
            }
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            Audio.PlaySound(Audio.player_hit);
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
    }
}
