using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    public class Camera
    {
        private Matrix transform;
        public Vector2 position;
        private Viewport view;

        public float speedFactor;

        public Matrix Transform
        {
            get { return transform; }
        }

        public Camera(Viewport view)
        {
            this.view = view;

            speedFactor = 0.05f;
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
            transform = Matrix.CreateTranslation(-position.X + Game1.windowX / 2, -position.Y + Game1.windowY / 2, 0);
        }

        public void SmoothPosition(Vector2 newPos)
        {
            float distance = Vector2.Distance(newPos, position);

            //if (distance < 2)
            //{
            //    speedFactor = 0.5f;
            //}

            Vector2 direction;
            direction.X = newPos.X - position.X;
            direction.Y = newPos.Y - position.Y;

            position += direction * speedFactor;

            //position.X = (int)Math.Round(position.X);
            //position.Y = (int)Math.Round(position.Y);
            //casting position float to int fixes weird moving of background in relation to player sprite, but causes camera shake :(
            transform = Matrix.CreateTranslation(-position.X + Game1.windowX / 2, -position.Y + Game1.windowY / 2, 0);
        }
    }
}
