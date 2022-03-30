using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    class Camera
    {
        private Matrix transform;
        public Vector2 position;
        private Viewport view;

        public Matrix Transform
        {
            get { return transform; }
        }

        public Camera(Viewport view)
        {
            this.view = view;
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
            transform = Matrix.CreateTranslation(-position.X + Game1.windowX / 2, -position.Y + Game1.windowY / 2, 0);
        }

        public void SmoothPosition(Vector2 newPos)
        {
            float speed;
            float distance = Vector2.Distance(newPos, position);

            if (distance < 3)
            {
                speed = 0;
            }
            else if (distance < 10)
            {
                speed = 1;
            }
            else
            {
                speed = 2;
            }

            Vector2 direction;
            direction.X = newPos.X - position.X;
            direction.Y = newPos.Y - position.Y;

            direction.Normalize();
            position += direction * speed;

            transform = Matrix.CreateTranslation(-position.X + Game1.windowX / 2, -position.Y + Game1.windowY / 2, 0);
        }
    }
}
