using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    public enum Selection
    {
        Object, Tile, Enemy
    }
    public enum ObjectSelection
    {
        Box, Barrel, Pot, ShrubSmall, TreeSmall, BoxOpen, TreeBig, ShrubBig
    }
    public enum TileSelection
    {
        Grass, Dirt, GrayBrick, TilesCheckeredGray, TilesCheckeredBrown
    }
    static public class LevelEditor
    {
        public static Selection currentSelection;
        public static ObjectSelection selectedObject;
        public static TileSelection selectedTile;
        private static Level currentLevel;

        //have list of levels?
        public static void Update(GameTime gameTime)
        {
            if(InputReader.RightClick())
            {
                LevelManager.ObjectRemove(Game1.mousePos);
            }

            if(InputReader.mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && currentSelection == Selection.Tile)
            {
                LevelManager.TileEdit(selectedTile, Game1.mousePos);
            }

            if(InputReader.LeftClick())
            {
                switch (currentSelection)
                {
                    case Selection.Object:
                        switch (selectedObject)
                        {
                            case ObjectSelection.ShrubSmall:
                                LevelManager.ObjectAdd(new ShrubSmall(Game1.mousePos - new Vector2(8, 8)));
                                break;

                            case ObjectSelection.Barrel:
                                LevelManager.ObjectAdd(new Barrel(Game1.mousePos - new Vector2(8, 8)));
                                break;

                            case ObjectSelection.Box:
                                LevelManager.ObjectAdd(new Box(Game1.mousePos - new Vector2(8, 8)));
                                break;

                            case ObjectSelection.BoxOpen:
                                LevelManager.ObjectAdd(new BoxOpen(Game1.mousePos - new Vector2(8, 8)));
                                break;

                            case ObjectSelection.ShrubBig:
                                LevelManager.ObjectAdd(new ShrubBig(Game1.mousePos - new Vector2(8, 8)));
                                break;

                            case ObjectSelection.TreeBig:
                                LevelManager.ObjectAdd(new TreeBig(Game1.mousePos - new Vector2(24, 24)));
                                break;

                            case ObjectSelection.TreeSmall:
                                LevelManager.ObjectAdd(new TreeSmall(Game1.mousePos - new Vector2(8, 16)));
                                break;

                            case ObjectSelection.Pot:
                                LevelManager.ObjectAdd(new Pot(Game1.mousePos - new Vector2(8, 8)));
                                break;

                            default:
                                break;
                        }
                        break;

                    case Selection.Tile:
                        //LevelManager.TileEdit(selectedTile, Game1.mousePos);
                        break;
                }
                
            }
            
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            if(currentSelection == Selection.Object)
            {
                switch (selectedObject)
                {
                    case ObjectSelection.ShrubSmall:
                        spriteBatch.Draw(TexMGR.shrub_small, Game1.mousePos - new Vector2(8, 8), Color.White);
                        break;

                    case ObjectSelection.Barrel:
                        spriteBatch.Draw(TexMGR.barrel, Game1.mousePos - new Vector2(8, 8), Color.White);
                        break;

                    case ObjectSelection.Box:
                        spriteBatch.Draw(TexMGR.box, Game1.mousePos - new Vector2(8, 8), Color.White);
                        break;

                    case ObjectSelection.BoxOpen:
                        spriteBatch.Draw(TexMGR.boxOpen, Game1.mousePos - new Vector2(8, 8), Color.White);
                        break;

                    case ObjectSelection.TreeSmall:
                        spriteBatch.Draw(TexMGR.tree_small, Game1.mousePos - new Vector2(8, 16), Color.White);
                        break;

                    case ObjectSelection.TreeBig:
                        spriteBatch.Draw(TexMGR.tree_big, Game1.mousePos - new Vector2(24, 24), Color.White);
                        break;

                    case ObjectSelection.ShrubBig:
                        spriteBatch.Draw(TexMGR.shrub_big, Game1.mousePos - new Vector2(8, 8), Color.White);
                        break;

                    case ObjectSelection.Pot:
                        spriteBatch.Draw(TexMGR.pot, Game1.mousePos - new Vector2(8, 8), Color.White);
                        break;

                    default:

                        break;
                }
            }

            if(currentSelection == Selection.Tile)
            {
                switch(selectedTile)
                {
                    case TileSelection.Grass:
                        spriteBatch.Draw(TexMGR.UI_grass, Game1.mousePos, Color.White);
                        break;

                    case TileSelection.Dirt:
                        spriteBatch.Draw(TexMGR.UI_dirt, Game1.mousePos, Color.White);
                        break;

                    default:
                        spriteBatch.Draw(TexMGR.UI_graybrick, Game1.mousePos, Color.White);
                        break;
                }
            }
            
        }

        /*
        public static Rectangle SetBounds()
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
        
        */

        public static void SaveLevelToFile(int id, List<string> objects, List<string> tiles)
        {
            string path = string.Format(@"maps\level{0}\", id);

            System.IO.Directory.CreateDirectory(string.Format(@"maps\level{0}\", id));

            List<string> bounds = new List<string>();
            bounds.Add(LevelManager.GetBounds().Width + "," + LevelManager.GetBounds().Height);

            File.WriteAllLines(path + "bounds.txt", bounds);
            File.WriteAllLines(path + "objectmap.txt", objects);
            File.WriteAllLines(path + "tilemap.txt", tiles);
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

        private static LevelObject ObjectCreator(string name, Vector2 pos)
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

                default:
                    return null;
            }
        }

        /*
        private static List<Enemy> EnemyLoad(int id)
        {
            
        }
        */
    }
}