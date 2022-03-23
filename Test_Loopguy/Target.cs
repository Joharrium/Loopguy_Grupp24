using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test_Loopguy
{
    public class Target : GameObject
    {
        public Target(Vector2 position)
        {
            this.position = position;
            texture = TexMGR.target;
        }

        public override void Update(GameTime gameTime)
        {
            hitBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            centerPosition = new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
