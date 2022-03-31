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

        public static Rectangle GetBounds()
        {
            return currentLevel.GetBounds();
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
                    if (terrainStrings[j][i] == 'w')
                    {
                        tiles[i, j] = new BrickWall(tempPos);
                    }
                }
            }

            return tiles;
        }
        

        
        private static List<LevelObject> ObjectLoad(int id)
        {
            List<LevelObject> levelObjects = new List<LevelObject>();
            levelObjects.Add(new Box(new Vector2(56, 56)));

            List<string> lines = new List<string>();
            foreach (string line in System.IO.File.ReadLines(string.Format(@"maps\level{0}\objectmap.txt", id)))
            {
                lines.Add(line);
            }

            for (int i = 0; i<lines.Count; i++)
            {
                string[] splitter = lines[i].Split(',');
                string objectToFind = splitter[0];
                Vector2 objectPosition = new Vector2(0,0);
                objectPosition.X = Int32.Parse(splitter[1]);
                objectPosition.Y = Int32.Parse(splitter[2]);
                levelObjects.Add(ObjectCreator(objectToFind, objectPosition));
            }
            

            return levelObjects;
        }
        
        public static bool LevelObjectCollision(Vector2 position)
        {
            return currentLevel.LevelObjectCollision(position);
        }

        public static bool WallCollision(Vector2 position)
        {
            return currentLevel.WallCollision(position);
        }

        public static void ObjectAdd(LevelObject levelObject)
        {
            currentLevel.AddObject(levelObject);
        }

        public static void ObjectRemove(Vector2 pos)
        {
            currentLevel.RemoveObject(pos);
        }

        public static void TileEdit(TileSelection tile, Vector2 position)
        {
            currentLevel.TileEdit(tile, position);
        }

        public static void SetMapSize(int x, int y)
        {
            currentLevel.SetMapSize(x, y);
        }


        public static LevelObject ObjectCreator(string name, Vector2 pos)
        {
            switch (name)
            {
                case "Box":
                    return new Box(pos);

                case "ShrubSmall":
                    return new ShrubSmall(pos);

                case "TreeBig":
                    return new TreeBig(pos);

                case "BoxOpen":
                    return new BoxOpen(pos);

                case "Barrel":
                    return new Barrel(pos);

                case "TreeSmall":
                    return new TreeSmall(pos);

                case "ShrubBig":
                    return new ShrubBig(pos);

                default:
                    return null;
                    break;
            }
        }

        public static List<string> ExportObjectList(int id)
        {
            List<string> objects = new List<string>();

            foreach (LevelObject lo in currentLevel.levelObjects)
            {
                objects.Add(lo.GetType().Name + "," + ((int)lo.position.X) + "," + ((int)lo.position.Y));
            }

            return objects;
        }

        public static List<string> ExportTileList(int id)
        {
            StringBuilder stringToAdd = new StringBuilder(currentLevel.tiles.GetLength(0));
            char[,] types = new char[currentLevel.tiles.GetLength(0), currentLevel.tiles.GetLength(1)];
            for (int i = 0; i < currentLevel.tiles.GetLength(1); i++)
            {
                for (int j = 0; j < currentLevel.tiles.GetLength(0); j++)
                {
                    if (currentLevel.tiles[j, i] is GrassTile)
                    {
                        types[j, i] = 'g';
                    }
                    if (currentLevel.tiles[j, i] is DirtTile)
                    {
                        types[j, i] = 'd';
                    }
                    if (currentLevel.tiles[j, i] is BrickWall)
                    {
                        types[j, i] = 'w';
                    }
                }
            }
            List<string> listToWrite = new List<string>();
            for (int j = 0; j < types.GetLength(1); j++)
            {
                for (int i = 0; i < types.GetLength(0); i++)
                {
                    if (types[i, j] != null)
                    { stringToAdd.Append(types[i, j]); }
                } //this won't work probably but I'm too tired
                listToWrite.Add(stringToAdd.ToString());
                stringToAdd.Clear();
            }

            return listToWrite;

        }
        /*
        private static List<Enemy> EnemyLoad(int id)
        {
            
        }
        */
    }
}