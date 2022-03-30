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
            
            transform = Matrix.CreateTranslation(MathHelper.Clamp(-position.X + Game1.windowX / 2, -LevelManager.GetBounds().Width, 0), MathHelper.Clamp(-position.Y + Game1.windowY / 2, -LevelManager.GetBounds().Height, 0), 0);
        }
    }
}
