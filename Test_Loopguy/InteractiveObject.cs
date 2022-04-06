using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    public class Destructible : LevelObject
    {
        int health;
        public Destructible(Vector2 position) : base(position)
        {
            this.position = position;
        }
    }

    public class Switch : LevelObject
    {
        bool state;
        public Switch(Vector2 position) : base(position)
        {
            this.position = position;
        }
    }

    
}
