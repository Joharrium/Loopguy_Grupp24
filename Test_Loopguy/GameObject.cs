using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    public abstract class GameObject
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 centerPosition;
        public Rectangle hitBox;

        public GameObject()
        {
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
