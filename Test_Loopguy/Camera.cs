using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    public class Camera
    {
        private Matrix transform;
        public Vector2 position;
        public Vector2 clampedPosition;
        public Vector2 oldNewPos;
        private Viewport view;

        public bool xClamped;
        public bool yClamped;

        public float speedFactor;

        public bool stabilize;

        public Matrix Transform
        {
            get { return transform; }
        }

        public Camera(Viewport view)
        {
            this.view = view;

            speedFactor = 5;
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
            
            transform = Matrix.CreateTranslation(MathHelper.Clamp(-position.X + Game1.windowX / 2, -LevelManager.GetBounds().Width, 0), MathHelper.Clamp(-position.Y + Game1.windowY / 2, -LevelManager.GetBounds().Height, 0), 0);
        }

        public void SmoothPosition(Vector2 newPos, float deltaTime)
        {
            float distance = Vector2.Distance(newPos, position);

            Vector2 direction;
            direction.X = newPos.X - position.X;
            direction.Y = newPos.Y - position.Y;

            Vector2 velocity;

            //if (distance < 10)
            //{
            //    Vector2.Normalize(direction);
            //    velocity = direction * 0.1f;
            //}
            velocity = direction * speedFactor * deltaTime;
            position += velocity;

            //position.X = MathHelper.Clamp(-position.X + Game1.windowX / 2, -LevelManager.GetBounds().Width, 0);
            //position.Y = MathHelper.Clamp(-position.Y + Game1.windowY / 2, -LevelManager.GetBounds().Height, 0);

            if (newPos == oldNewPos || stabilize)
            {
                position.X = (int)Math.Round(position.X);
                position.Y = (int)Math.Round(position.Y);
            }
            //position.X = (int)Math.Round(position.X);
            //position.Y = (int)Math.Round(position.Y);
            //casting position float to int fixes weird moving of background in relation to player sprite,
            //but causes player to shake when runnin :( 

            //transform = Matrix.CreateTranslation(-position.X + Game1.windowX / 2, -position.Y + Game1.windowY / 2, 0);

            //OBS This clamping stuff messes up calculating the mouse postition in Game1 very badly. How to fix???
            if(clampedPosition.X == MathHelper.Clamp(-position.X + Game1.windowX / 2, -LevelManager.GetBounds().Width, 0))
            {
                xClamped = true;
            }
            else
            {
                xClamped = false;
            }

            if (clampedPosition.Y == MathHelper.Clamp(-position.Y + Game1.windowX / 2, -LevelManager.GetBounds().Height, 0))
            {
                yClamped = true;
            }
            else
            {
                yClamped = false;
            }

            clampedPosition.X = MathHelper.Clamp(-position.X + Game1.windowX / 2, -LevelManager.GetBounds().Width, 0);
            clampedPosition.Y = MathHelper.Clamp(-position.Y + Game1.windowY / 2, -LevelManager.GetBounds().Height, 0);

            

            transform = Matrix.CreateTranslation(clampedPosition.X, clampedPosition.Y, 0);

            //transform = Matrix.CreateTranslation(MathHelper.Clamp(-position.X + Game1.windowX / 2, -LevelManager.GetBounds().Width, 0), MathHelper.Clamp(-position.Y + Game1.windowY / 2, -LevelManager.GetBounds().Height, 0), 0);

            oldNewPos = newPos;
        }
    }
}
