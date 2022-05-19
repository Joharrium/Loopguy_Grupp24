﻿using System;
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

        protected AnimatedSprite sprite;

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
    }
}
