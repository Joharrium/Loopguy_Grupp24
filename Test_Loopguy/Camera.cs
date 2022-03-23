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
    }
}
