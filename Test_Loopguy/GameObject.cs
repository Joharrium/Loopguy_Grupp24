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
        public float drawDepth;

        public GameObject(Vector2 position)
        {
            this.position = position;
            texture = TextureManager.notex;

            centerPosition = new Vector2(position.X - texture.Width / 2, position.Y - texture.Height / 2);
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
