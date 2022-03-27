using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    internal class AnimatedMovingObject : MovingObject
    {
        protected AnimSprite sprite;

        public AnimatedMovingObject(Vector2 position)
            : base(position)
        {


        }

        public override void Update(GameTime gameTime)
        {
            hitBox = new Rectangle((int)position.X, (int)position.Y, sprite.size.X, sprite.size.Y);
            centerPosition = new Vector2(position.X + sprite.size.X / 2, position.Y + sprite.size.Y / 2);
        }
    }
}
