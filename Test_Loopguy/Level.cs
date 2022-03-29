using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    public class Level
    {
        int id;
        Rectangle cameraBounds;
        //array of tiles
        //list of game objects
        //list of enemies
        //list of corresponding entrances from different ids and their position

        public void Update()
        {
            //update game objects
            //update enemy ai

            //do any animations
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //draw tiles and objects and enemies, in the correct order
        }
    }
    public class Entrance
    {
        Vector2 position;
        int idFrom;
        int idTo;
    }
}