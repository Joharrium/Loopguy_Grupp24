using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test_Loopguy
{
    class EntityManager
    {

        public static Player player;
        static List<Enemy> enemyList;

        public static void PlayerInitialization()
        {
            //player = (Player)movingObject;

            player = new Player(new Vector2(64, 64));

        }

        public static void EnemyInitialization()
        {
            enemyList = new List<Enemy>();
        }


        public static void Update(GameTime gameTime)
        {
            player.Update(gameTime);

            //foreach (Enemy e in enemyList)
            //{
            //    e.Update(gameTime);
            //}


        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            player.Draw(spriteBatch);

            //foreach (Enemy e in enemyList)
            //{
            //    e.Draw(spriteBatch);
            //}


        }

    }
}
