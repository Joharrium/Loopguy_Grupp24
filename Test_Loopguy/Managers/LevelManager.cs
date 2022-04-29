using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    static public class LevelManager
    {
        private static Level currentLevel;
        private static int queuedLevel;
        private static List<Level> loadedLevels = new List<Level>();
        public static List<Entrance> gates = new List<Entrance>();
        private static double loadTimer = 80;
        public static bool loadStarted = false;
        const double LOADTIMER = 80;
        

        private static Player player;
        private static Vector2 target;
        internal static void StartLevelTransition(int levelToLoad, Player player, Vector2 target)
        {
            loadTimer = LOADTIMER;
            loadStarted = true;
            queuedLevel = levelToLoad;
            LevelManager.player = player;
            LevelManager.target = target;
        }
        private static void LevelTransition(GameTime gameTime)
        {
            loadTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if(loadTimer < 0)
            {
                TryLoad(queuedLevel);
                player.position = target;
                loadStarted = false;
            }
        }

        internal static List<Vector2> GetPointsOfObject(GameObject lo)
        {
            List<Vector2> points = new List<Vector2>();
            points.Add(lo.position);
            points.Add(lo.position + new Vector2(lo.hitBox.Width, 0));
            points.Add(lo.position + new Vector2(0, lo.hitBox.Height));
            points.Add(lo.position + new Vector2(lo.hitBox.Width, lo.hitBox.Height));
            points.Add(lo.centerPosition);
            return points;
        }

        

        internal static void Update(GameTime gameTime, Player player)
        {
            if(loadStarted)
            {
                LevelTransition(gameTime);
            }
            
            currentLevel.Update(gameTime, player);
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            currentLevel.Draw(spriteBatch);
        }

        internal static void CheckGate(Player player)
        {
            foreach(Entrance g in gates)
            {
                g.CheckGate(player);
            }
        }

        public static void Reset()
        {
            List<Level> levelsToClear = new List<Level>();
            Level level1 = LoadLevel(1);
            currentLevel = level1;
            EntityManager.player.Reset( new Vector2(64, 64));

            foreach (Level l in loadedLevels)
            {
                levelsToClear.Add(l);
            }
            foreach(Level l in levelsToClear)
            {
                loadedLevels.Remove(l);
            }
            loadedLevels.Add(level1);

        }

        public static Rectangle GetBounds()
        {
            return currentLevel.GetBounds();
        }

        public static void SetBounds(int x, int y)
        {
            currentLevel.SetBounds(x, y);
        }

        public static void TryLoad(int id)
        {
            
            foreach (Level l in loadedLevels)
            {
                if(l.id == id)
                {
                    currentLevel = l;
                    Audio.UpdateLevelMusic();
                    return;
                }
            }
            
            currentLevel = LoadLevel(id);
            Audio.UpdateLevelMusic();

        }

        public static int GetCurrentId()
        {
            return currentLevel.id;
        }
        
        public static List<Song> GetMusic(bool combat)
        {
            if (combat)
            {
                return currentLevel.combatSongs;
            }
            else
            {
                return currentLevel.idleSongs;
            }
        }
        
        public static bool LevelObjectCollision(Vector2 position)
        {
            if(!currentLevel.LevelObjectCollision(position))
            {
                return currentLevel.WallCollision(position);
            }
            else
            {
                return currentLevel.LevelObjectCollision(position);
            }
            
            
        }


        internal static void AddEnemyProjectile(Shot projectile)
        {
            currentLevel.enemyProjectiles.Add(projectile);
        }

        internal static void AddPlayerProjectile(Shot projectile)
        {
            currentLevel.playerProjectiles.Add(projectile);
        }

        public static bool WallCollision(Vector2 position)
        {
            return currentLevel.WallCollision(position);
        }

        public static bool[,] GetEdges(Tile tile)
        {
            bool[,] edges = new bool[3,3];
            Point coords = currentLevel.GetTileCoordinate(tile);
            for (int i = -1; i < 2; i++)
            {
                for(int j = -1; j < 2; j++)
                {
                    if(coords.X + i >= 0 && coords.Y + j >= 0 && coords.X + i < currentLevel.tiles.GetLength(0) && coords.Y + j < currentLevel.tiles.GetLength(1))
                    {
                        edges[i + 1, j + 1] = true;
                        if (currentLevel.tiles[coords.X + i, coords.Y + j] is GrassTile)
                        {
                            edges[i + 1, j + 1] = false;
                        }
                        else
                        {
                            edges[i + 1, j + 1] = true;
                        }
                    }
                    
                }
                
            }
            if(edges[0,2])
            {
                Console.WriteLine("fuck this");
            }
            return edges;
        }

        //below are editor and load methods

        public static Level LoadLevel(int id)
        {


            Level level = new Level(id, BoundsLoad(id), ObjectLoad(id), TileLoad(id), EnemyLoad(id));

            loadedLevels.Add(level);
            currentLevel = level;
            currentLevel.RefreshEdges();
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

        public static List<Song> SongLoad(int id, bool combat)
        {
            List<string> lines = new List<string>();
            if(combat)
            {
                foreach (string line in System.IO.File.ReadLines(string.Format(@"maps\level{0}\music_combat.txt", id)))
                {
                    lines.Add(line);
                }
            }
            else
            {
                foreach (string line in System.IO.File.ReadLines(string.Format(@"maps\level{0}\music_idle.txt", id)))
                {
                    lines.Add(line);
                }
            }
            
            return Audio.LoadLevelMusic(lines, combat);
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
                    if (terrainStrings[j][i] == 'd')
                    {
                        tiles[i, j] = new DirtTile(tempPos);
                    }
                    if (terrainStrings[j][i] == 'w')
                    {
                        tiles[i, j] = new BrickWall(tempPos);
                    }
                    if (terrainStrings[j][i] == 'a')
                    {
                        tiles[i, j] = new CheckeredTileGray(tempPos);
                    }
                    if (terrainStrings[j][i] == 'b')
                    {
                        tiles[i, j] = new CheckeredTileBrown(tempPos);
                    }
                    if (terrainStrings[j][i] == 'c')
                    {
                        tiles[i, j] = new TileBigDark(tempPos);
                    }
                    if (terrainStrings[j][i] == 'e')
                    {
                        tiles[i, j] = new TileBigLight(tempPos);
                    }
                }
            }

            return tiles;
        }

        private static List<Enemy> EnemyLoad(int id)
        {
            List<Enemy> enemies = new List<Enemy>();

            List<string> lines = new List<string>();
            foreach (string line in System.IO.File.ReadLines(string.Format(@"maps\level{0}\enemymap.txt", id)))
            {
                lines.Add(line);
            }

            for (int i = 0; i < lines.Count; i++)
            {
                string[] splitter = lines[i].Split(',');
                string enemyToFind = splitter[0];
                Vector2 enemyPosition = new Vector2(0, 0);
                enemyPosition.X = Int32.Parse(splitter[1]);
                enemyPosition.Y = Int32.Parse(splitter[2]);

                enemies.Add(EnemyCreator(enemyToFind, enemyPosition));

            }


            return enemies;
        }

        private static List<LevelObject> ObjectLoad(int id)
        {
            List<LevelObject> levelObjects = new List<LevelObject>();

            List<string> lines = new List<string>();
            foreach (string line in System.IO.File.ReadLines(string.Format(@"maps\level{0}\objectmap.txt", id)))
            {
                lines.Add(line);
            }

            for (int i = 0; i < lines.Count; i++)
            {
                //parameter3
                int? requiredKey = null;
                //parameter4
                bool? keyPermanent = null;

                string[] splitter = lines[i].Split(',');
                string objectToFind = splitter[0];
                Vector2 objectPosition = new Vector2(0, 0);
                objectPosition.X = Int32.Parse(splitter[1]);
                objectPosition.Y = Int32.Parse(splitter[2]);
                if(splitter.Length > 3)
                {
                    requiredKey = Int32.Parse(splitter[3]);
                }
                if(splitter.Length > 4)
                {
                    keyPermanent = Boolean.Parse(splitter[4]);
                }

                if (keyPermanent != null)
                {
                    levelObjects.Add(KeyCreator(objectPosition, (int)requiredKey, (bool)keyPermanent));
                }
                else if (requiredKey != null)
                {
                    levelObjects.Add(DoorCreator(objectToFind, objectPosition, (int)requiredKey));
                }
                else
                {
                    levelObjects.Add(ObjectCreator(objectToFind, objectPosition));
                }
                
            }


            return levelObjects;
        }

        public static void ObjectAdd(LevelObject levelObject)
        {
            currentLevel.AddObject(levelObject);
        }

        public static void ObjectRemove(Vector2 pos)
        {
            currentLevel.RemoveObject(pos);
        }

        internal static void EnemyAdd(Enemy enemy)
        {
            currentLevel.enemies.Add(enemy);
        }

        public static void TileEdit(TileSelection tile, Vector2 position)
        {
            currentLevel.TileEdit(tile, position);
        }

        public static void SetMapSize(int x, int y)
        {
            currentLevel.SetMapSize(x, y);
        }

        public static Door DoorCreator(string name, Vector2 pos, int key)
        {
            switch (name)
            {
                case "Door":
                    return new Door(pos, key);

                case "DoorSliding":
                    return new DoorSliding(pos, key);
                    
                default:
                    return null;
                    

            }
            
        }

        public static KeyPickup KeyCreator(Vector2 pos, int id, bool permanent)
        {
            return new KeyPickup(pos, id, permanent);
        }

        internal static Enemy EnemyCreator(string name, Vector2 pos)
        {
            switch (name)
            {
                case "TestEnemyRanged":
                    return new TestEnemyRanged(pos);

                case "TestEnemy":
                    return new TestEnemy(pos);
                default:
                    return new TestEnemy(pos);
            }
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

                case "Pot":
                    return new Pot(pos);

                case "BarrelDestructible":
                    return new BarrelDestructible(pos);

                default:
                    return null;
                    
            }
        }

        public static List<string> ExportObjectList(int id)
        {
            List<string> objects = new List<string>();

            foreach (LevelObject lo in currentLevel.levelObjects)
            {
                if(lo is Door)
                {
                    
                }
                else if(lo is KeyPickup)
                {

                }
                else
                {
                    objects.Add(lo.GetType().Name + "," + ((int)lo.position.X) + "," + ((int)lo.position.Y));
                }
                
            }

            foreach (Door d in currentLevel.levelObjects.OfType<Door>())
            {
                objects.Add(d.GetType().Name + "," + ((int)d.position.X) + "," + ((int)d.position.Y) + "," + d.requiredKey);
            }

            foreach (KeyPickup k in currentLevel.levelObjects.OfType<KeyPickup>())
            {
                objects.Add(k.GetType().Name + "," + ((int)k.position.X) + "," + ((int)k.position.Y) + "," + k.GetStringOfParams());
            }

            return objects;
        }
        public static List<string> ExportEnemyList()
        {
            List<string> enemies = new List<string>();

            foreach(Enemy e in currentLevel.enemies)
            {
                enemies.Add(e.GetType().Name + "," + ((int)e.position.X) + "," + ((int)e.position.Y));
            }

            return enemies;
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
                    if (currentLevel.tiles[j, i] is CheckeredTileGray)
                    {
                        types[j, i] = 'a';
                    }
                    if (currentLevel.tiles[j, i] is CheckeredTileBrown)
                    {
                        types[j, i] = 'b';
                    }
                    if (currentLevel.tiles[j, i] is TileBigDark)
                    {
                        types[j, i] = 'c';
                    }
                    if (currentLevel.tiles[j, i] is TileBigLight)
                    {
                        types[j, i] = 'e';
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
                }
                listToWrite.Add(stringToAdd.ToString());
                stringToAdd.Clear();
            }

            return listToWrite;

        }
        
        public static List<Entrance> EntranceLoad()
        {
            List<Entrance> entrances = new List<Entrance>();
            for (int i = 0; i<999; i++)
            {
                if(File.Exists(string.Format(@"maps\gates\{0}.txt", i)))
                {
                    List<string> lines = new List<string>();
                    foreach (string line in System.IO.File.ReadLines(string.Format(@"maps\gates\{0}.txt", i)))
                    {
                        lines.Add(line);
                    }

                    int map1 = Int32.Parse(lines[0]);
                    int map2 = Int32.Parse(lines[1]);

                    string[] splitter = lines[2].Split(',');
                    Rectangle entrance1 = new Rectangle(Int32.Parse(splitter[0]), Int32.Parse(splitter[1]), Int32.Parse(splitter[2]), Int32.Parse(splitter[3]));
                    string[] splitter2 = lines[3].Split(',');
                    Vector2 entrance2 = new Vector2(Int32.Parse(splitter2[0]), Int32.Parse(splitter2[1]));
                    gates.Add(new Entrance(i, map1, map2, entrance1, entrance2));
                }
            }
            
            return entrances;
        }
        
    }
}