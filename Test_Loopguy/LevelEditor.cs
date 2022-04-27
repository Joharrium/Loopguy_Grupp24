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
        Box, Barrel, Pot, ShrubSmall, TreeSmall, BoxOpen, TreeBig, ShrubBig, DoorWood, KeycardRed, DoorSliding, BarrelDestructible, HealingSmall
    }
    public enum TileSelection
    {
        Grass, Dirt, GrayBrick, TilesCheckeredGray, TilesCheckeredBrown, TilesBigDark, TilesBigLight, TileMetal, WallMetal, CarpetWorn, DrywallWorn
    }

    public enum EnemySelection
    {
        MeleeTest, RangedTest
    }
    static public class LevelEditor
    {
        public static Selection currentSelection;
        public static ObjectSelection selectedObject;
        public static TileSelection selectedTile;
        public static EnemySelection selectedEnemy;
        private static int doorRequiredKey;
        private static int keyID;
        private static bool keyPermanent;
        private static Level currentLevel;

        public static void SetDoorParams(int key)
        {
            doorRequiredKey = key;
        }

        public static void SetKeyParams(int id, bool permanent)
        {
            keyID = id;
            keyPermanent = permanent;
        }

        public static void SelectObject(ObjectSelection obj)
        {
            currentSelection = Selection.Object;
            selectedObject = obj;
        }

        public static void SelectEnemy(EnemySelection enemy)
        {
            currentSelection = Selection.Enemy;
            selectedEnemy = enemy;
        }

        public static void SelectTile(TileSelection tile)
        {
            currentSelection = Selection.Tile;
            selectedTile = tile;
        }

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

                            case ObjectSelection.DoorWood:
                                LevelManager.ObjectAdd(new Door(Game1.mousePos - new Vector2(16, 16), doorRequiredKey));
                                break;

                            case ObjectSelection.DoorSliding:
                                LevelManager.ObjectAdd(new DoorSliding(Game1.mousePos - new Vector2(16, 16), doorRequiredKey));
                                break;

                            case ObjectSelection.KeycardRed:
                                LevelManager.ObjectAdd(new KeyPickup(Game1.mousePos - new Vector2(8, 8), doorRequiredKey, keyPermanent));
                                break;

                            case ObjectSelection.BarrelDestructible:
                                LevelManager.ObjectAdd(new BarrelDestructible(Game1.mousePos - new Vector2(8, 8)));
                                break;

                            case ObjectSelection.HealingSmall:
                                LevelManager.ObjectAdd(new SmallHealthPickup(Game1.mousePos - new Vector2(8, 8)));
                                break;

                            default:
                                break;
                        }
                        break;

                    case Selection.Enemy:
                        switch (selectedEnemy)
                        {
                            case EnemySelection.MeleeTest:
                                LevelManager.EnemyAdd(new TestEnemy(Game1.mousePos - new Vector2(8, 8)));
                                break;
                            case EnemySelection.RangedTest:
                                LevelManager.EnemyAdd(new TestEnemyRanged(Game1.mousePos - new Vector2(8, 8)));
                                break;
                        }
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
                        spriteBatch.Draw(TextureManager.shrub_small, Game1.mousePos - new Vector2(8, 8), Color.White);
                        break;

                    case ObjectSelection.Barrel:
                        spriteBatch.Draw(TextureManager.barrel, Game1.mousePos - new Vector2(8, 8), Color.White);
                        break;

                    case ObjectSelection.BarrelDestructible:
                        spriteBatch.Draw(TextureManager.barrel, Game1.mousePos - new Vector2(8, 8), Color.White);
                        break;

                    case ObjectSelection.Box:
                        spriteBatch.Draw(TextureManager.box, Game1.mousePos - new Vector2(8, 8), Color.White);
                        break;

                    case ObjectSelection.BoxOpen:
                        spriteBatch.Draw(TextureManager.boxOpen, Game1.mousePos - new Vector2(8, 8), Color.White);
                        break;

                    case ObjectSelection.TreeSmall:
                        spriteBatch.Draw(TextureManager.tree_small, Game1.mousePos - new Vector2(8, 16), Color.White);
                        break;

                    case ObjectSelection.TreeBig:
                        spriteBatch.Draw(TextureManager.tree_big, Game1.mousePos - new Vector2(24, 24), Color.White);
                        break;

                    case ObjectSelection.ShrubBig:
                        spriteBatch.Draw(TextureManager.shrub_big, Game1.mousePos - new Vector2(8, 8), Color.White);
                        break;

                    case ObjectSelection.Pot:
                        spriteBatch.Draw(TextureManager.pot, Game1.mousePos - new Vector2(8, 8), Color.White);
                        break;

                    case ObjectSelection.DoorWood:
                        spriteBatch.Draw(TextureManager.UI_door, Game1.mousePos - new Vector2(16, 16), Color.White);
                        break;

                    case ObjectSelection.DoorSliding:
                        spriteBatch.Draw(TextureManager.UI_door, Game1.mousePos - new Vector2(16, 16), Color.White);
                        break;

                    case ObjectSelection.KeycardRed:
                        spriteBatch.Draw(TextureManager.keycard, Game1.mousePos - new Vector2(8, 8), Color.White);
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
                        spriteBatch.Draw(TextureManager.UI_grass, Game1.mousePos, Color.White);
                        break;

                    case TileSelection.Dirt:
                        spriteBatch.Draw(TextureManager.UI_dirt, Game1.mousePos, Color.White);
                        break;

                    default:
                        spriteBatch.Draw(TextureManager.UI_graybrick, Game1.mousePos, Color.White);
                        break;
                }
            }
            
        }

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