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
            float distance = Vector2.Distance(newPos, position);
            float speedFactor = 0.05f;

            Vector2 direction;
            direction.X = newPos.X - position.X;
            direction.Y = newPos.Y - position.Y;

            position += direction * speedFactor;


            transform = Matrix.CreateTranslation(-(int)position.X + Game1.windowX / 2, -(int)position.Y + Game1.windowY / 2, 0);
        }
    }
}
