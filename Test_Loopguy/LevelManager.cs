using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    static public class LevelManager
    {
        private static Level currentLevel;

        //have list of levels?
        public static void Update(GameTime gameTime)
        {
            currentLevel.Update(gameTime);
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            currentLevel.Draw(spriteBatch);
        }

        public static Level LoadLevel(int id)
        {


            Level level = new Level(id, BoundsLoad(id), ObjectLoad(id), TileLoad(id));
            //obviously shouldn't return null when done

            currentLevel = level;
            return level;
        }

        private static Rectangle BoundsLoad(int id)
        {
            Rectangle bounds = new Rectangle(0, 0, 0, 0);
            List<string> lines = new List<string>();
            foreach (string line in System.IO.File.ReadLines(string.Format(@"maps\level{0}\bounds.txt", id)))
            {
                lines.Add(line);
            }

            string[] splitter = lines[0].Split(',');
            bounds.Width = Int32.Parse(splitter[0]);
            bounds.Height = Int32.Parse(splitter[1]);
            return bounds;
        }

        
        private static Tile[,] TileLoad(int id)
        {
            List<string> terrainStrings = new List<string>();
            StreamReader terrainReader = new StreamReader(String.Format(@"maps\level{0}\tilemap.txt", id));

            

            while (!terrainReader.EndOfStream)
            {
                terrainStrings.Add(terrainReader.ReadLine());
            }
            terrainReader.Close();

            Tile[,] tiles = new Tile[terrainStrings[0].Length, terrainStrings.Count];

            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    Vector2 tempPos = new Vector2((i * 16), (j * 16));
                    if (terrainStrings[j][i] == 'g')
                    {
                        tiles[i, j] = new GrassTile(tempPos);
                    }
                }
            }

            return tiles;
        }
        

        
        private static List<LevelObject> ObjectLoad(int id)
        {
            List<LevelObject> levelObjects = new List<LevelObject>();
            levelObjects.Add(new Box(new Vector2(56, 56)));

            return levelObjects;
        }
        

        /*
        private static List<Enemy> EnemyLoad(int id)
        {
            
        }
        */
    }
}