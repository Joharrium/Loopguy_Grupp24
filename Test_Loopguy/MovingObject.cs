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
        protected Point frameSize;

        public MovingObject (Vector2 position)
            : base(position)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            hitBox = new Rectangle((int)position.X, (int)position.Y, frameSize.X, frameSize.Y);
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
