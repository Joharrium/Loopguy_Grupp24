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

        public Rectangle footprint;

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
    }
}
