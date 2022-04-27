using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test_Loopguy
{
    class PlayerManager
    {

        public static Player player;
        static List<Enemy> enemyList;

        public static void PlayerInitialization()
        {

            player = new Player(new Vector2(64, 64));

        }



        public static void Update(GameTime gameTime)
        {
            player.Update(gameTime);

 

        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            player.Draw(spriteBatch);

        }

    }
}
