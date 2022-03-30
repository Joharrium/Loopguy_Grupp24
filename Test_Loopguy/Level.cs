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
        Tile[,] tiles;
        List<LevelObject> levelObjects;
        //list of enemies
        //list of corresponding entrances from different ids and their position


        public Level (int id, Rectangle cameraBounds, List<LevelObject> levelObjects, Tile[,] tiles/*, List<Entrance> entrances*/)
        {
            this.id = id;
            this.cameraBounds = cameraBounds;
            this.levelObjects = levelObjects;
            this.tiles = tiles;
            
        }

        public void Update(GameTime gameTime)
        {
            //update game objects
            //update enemy ai

            //do any animations
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile t in tiles)
            {
                t.Draw(spriteBatch);
            }
            foreach (LevelObject lo in levelObjects)
            {
                lo.Draw(spriteBatch);
            }
            //draw tiles and objects and enemies, in the correct order
        }

        public bool LevelObjectCollision(Vector2 check)
        {
            foreach(LevelObject lo in levelObjects)
            {
                if(lo.hitBox.Contains(check))
                {
                    return true;
                }
            }
            return false;
        }

        public Rectangle GetBounds()
        {
            return cameraBounds;
        }

        public bool WallCollision(Vector2 check)
        {
            foreach (Tile w in tiles)
            {
                if(w is Wall)
                {
                    if (w.hitBox.Contains(check))
                    {
                        return true;
                    }
                }
                
            }
            return false;
        }

        public void AddObject(LevelObject levelObject)
        {
            levelObjects.Add(levelObject);
        }
    }
    public class Entrance
    {
        int id;
        Vector2 position;
        int idFrom;
        int idTo;
    }
}