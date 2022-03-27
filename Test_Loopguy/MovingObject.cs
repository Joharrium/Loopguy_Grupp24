using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    internal class MovingObject : GameObject
    {

        protected Vector2 direction;
        protected float speed;

        public MovingObject (Vector2 position)
            : base(position)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            hitBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            centerPosition = new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);

        }

        public virtual void Movement(float deltaTime)
        {
            position += direction * speed * deltaTime;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
