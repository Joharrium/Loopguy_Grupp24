using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test_Loopguy
{
    public abstract class Component
    {
        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void Update(GameTime gameTime);

        public abstract Rectangle Rectangle { get;  }
        public abstract bool isHovering { get; set; }
    }
}
