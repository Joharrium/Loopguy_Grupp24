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
        public static bool countTime = false;
        private static float timeLeft = 600;
        private static float startingTime = 600;
        private static Level currentLevel;
        public static Level CurrentLevel
        {
            get { return currentLevel; }
        }
        private static int queuedLevel;
        private static List<Level> loadedLevels = new List<Level>();
        public static List<Entrance> gates = new List<Entrance>();
        private static double loadTimer = 80;
        public static bool loadStarted = false;
        const double LOADTIMER = 80;

        private static bool loopStarted = false;
        private static double loopTimer = 400;
        const double LOOPTIMER = 400;

        private static double winTimer = 8000;
        private static double WINTIMER = 8000;

        private static bool gameWon = false;
        public static bool GameWon
        {
            get { return gameWon; }
        }

        public static List<LevelObject> objectsToAdd = new List<LevelObject>();

        private static Player player;
        private static Vector2 target;

        internal static void Init()
        {
            LoadLevel(10);
            EntityManager.player.EnterRoom(new Vector2(208, 24));
               
            EntranceLoad();
        }

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
                player.EnterRoom(target);
                loadStarted = false;
                if(queuedLevel == 11)
                {
                    countTime = true;
                }
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
            points.Add(lo.centerPosition + new Vector2(lo.hitBox.Width / 2, 0));
            points.Add(lo.centerPosition - new Vector2(lo.hitBox.Width / 2, 0));
            points.Add(lo.centerPosition + new Vector2(0, lo.hitBox.Height / 2));
            points.Add(lo.centerPosition - new Vector2(0, lo.hitBox.Height / 2));
            return points;
        }

        public static Tile GetTile(Vector2 pos)
        {
            foreach(Tile t in currentLevel.tiles)
            {
                if(t.hitBox.Contains(pos))
                {
                    return t;
                }
            }
            return new Tile(pos);
        }

        internal static void Update(GameTime gameTime, Player player)
        {
            if(loadStarted)
            {
                LevelTransition(gameTime);
            }
            
            currentLevel.Update(gameTime, player);
            if(countTime)
            timeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if(loopStarted)
            {
                loopTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            
            if(timeLeft < 0 && !loopStarted)
            {
                StartReset();
            }
            if(loopTimer < 0 && loopStarted)
            {
                Reset();
            }

            if(gameWon)
            {
                winTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if(winTimer < 0)
                {
                    Reset();
                    gameWon = false;
                }
            }
        }

        internal static void StartReset()
        {
            Fadeout.LoopFade();
            loopTimer = LOOPTIMER;
            if (!loopStarted)
            {
                Audio.PlaySound(Audio.nuclearExplosion);
            }
            loopStarted = true;
            
            
        }

        public static void DrawTimer(SpriteBatch spriteBatch)
        {

            if(!(timeLeft == startingTime))
            {
                string add = "";
                if (Math.Truncate(timeLeft % 60) < 10)
                {
                    add = "0";
                }
                string calculateTimer = (((timeLeft - timeLeft % 60) / 60) + ":" + add + Math.Truncate(timeLeft % 60)).ToString();
                Vector2 pos = new Vector2(Game1.windowX - 48, 4);

                Color bgColor = Color.Black;
                Color fgColor = Color.White;
                if(timeLeft < 10)
                {
                    bgColor = Color.White;
                    fgColor = Color.Red;
                }

                if(timeLeft < 4 && timeLeft > 3.95)
                {
                    Audio.PlaySound(Audio.countdownAlarm);
                }

                OutlinedText.DrawOutlinedText(spriteBatch, pos, TextureManager.UI_menuFont, calculateTimer, bgColor, fgColor);

                //spriteBatch.DrawString(TextureManager.UI_menuFont, calculateTimer, pos, Color.White);
            }
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

        public static void Win()
        {
            gameWon = true;
            winTimer = WINTIMER;
            Fadeout.WinFade();
        }

        

        public static void Reset()
        {
            //Fadeout.LoopFade();
            List<Level> levelsToClear = new List<Level>();
            Level level1 = LoadLevel(10);
            
            currentLevel = level1;
            EntityManager.player.Reset( new Vector2(64, 64));
            timeLeft = startingTime;

            foreach (Level l in loadedLevels)
            {
                levelsToClear.Add(l);
            }
            foreach(Level l in levelsToClear)
            {
                loadedLevels.Remove(l);
            }
            loadedLevels.Add(level1);
            loopStarted = false;
            countTime = false;

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
        
        public static bool LevelObjectCollision(Rectangle position, int height)
        {
            if(!currentLevel.LevelObjectCollision(position, height))
            {
                return currentLevel.WallCollision(position.Center.ToVector2());
            }
            else
            {
                return true; //currentLevel.LevelObjectCollision(position, height);
            }
        }

        public static bool LevelObjectCollision(Line line, int height)
        {
            if (!currentLevel.LevelObjectCollision(line, height))
            {
                return false;
                //return currentLevel.WallCollision(position.Center.ToVector2());
            }
            else
            {
                return true; //currentLevel.LevelObjectCollision(position, height);
            }
        }

        public static bool RailgunCollision(Line line)
        {
            if (!currentLevel.RailgunCollision(line))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void RailgunEnemyCollision(Line line)
        {
            currentLevel.RailgunEnemyCollision(line);
        }

        internal static void QueueAddObject(LevelObject lo)
        {
            objectsToAdd.Add(lo);
        }

        internal static void AddEnemyProjectile(Projectile projectile)
        {
            currentLevel.enemyProjectiles.Add(projectile);
        }

        internal static void AddPlayerProjectile(Projectile projectile)
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
                        if (currentLevel.tiles[coords.X + i, coords.Y + j].GetType() == tile.GetType())
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
            return edges;
        }

        //below are editor and load methods
        public static void RefreshEdges()
        {
            currentLevel.RefreshMap();
        }

        public static Level LoadLevel(int id)
        {


            Level level = new Level(id, BoundsLoad(id), ObjectLoad(id), TileLoad(id), EnemyLoad(id));

            loadedLevels.Add(level);
            currentLevel = level;
            currentLevel.RefreshMap();
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
                    if (terrainStrings[j][i] == 'f')
                    {
                        tiles[i, j] = new WornWall(tempPos);
                    }
                    if (terrainStrings[j][i] == 'h')
                    {
                        tiles[i, j] = new TileMetal(tempPos);
                    }
                    if (terrainStrings[j][i] == 'i')
                    {
                        tiles[i, j] = new Water(tempPos);
                    }
                    if (terrainStrings[j][i] == 'j')
                    {
                        tiles[i, j] = new WallGray(tempPos);
                    }
                    if (terrainStrings[j][i] == 'k')
                    {
                        tiles[i, j] = new TileWarning(tempPos);
                    }
                    if (terrainStrings[j][i] == 'l')
                    {
                        tiles[i, j] = new Carpet(tempPos);
                    }
                    if (terrainStrings[j][i] == 'm')
                    {
                        tiles[i, j] = new GlassWall(tempPos);
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
            return name switch
            {
                "TestEnemyRanged" => new TestEnemyRanged(pos),

                "MeleeEnemyWeak" => new MeleeEnemyWeak(pos),

                "RangedRobotEnemy" => new RangedRobotEnemy(pos),

                "AndroidEnemy" => new AndroidEnemy(pos),

                _ => new MeleeEnemyWeak(pos),
            };
        }
        public static LevelObject ObjectCreator(string name, Vector2 pos)
        {
            return name switch
            {
                "Box" => new Box(pos),
                "ShrubSmall" => new ShrubSmall(pos),
                "TreeBig" => new TreeBig(pos),
                "BoxOpen" => new BoxOpen(pos),
                "Barrel" => new Barrel(pos),
                "TreeSmall" => new TreeSmall(pos),
                "ShrubBig" => new ShrubBig(pos),
                "Pot" => new Pot(pos),
                "BarrelDestructible" => new BarrelDestructible(pos),
                "SmallHealthPickup" => new SmallHealthPickup(pos),
                "SmallAmmoPickup" => new SmallAmmoPickup(pos),
                "Cabinet" => new Cabinet(pos),
                "Counter" => new Counter(pos),
                "CardboardBoxStackSmall" => new CardboardBoxStackSmall(pos),
                "BillBoard" => new BillBoard(pos),
                "ShelfArchivingSmall" => new ShelfArchivingSmall(pos),
                "ShelfArchiving" => new ShelfArchiving(pos),
                "ChairOfficeBw" => new ChairOfficeBw(pos),
                "ChairOfficeFw" => new ChairOfficeFw(pos),
                "MonitorWall" => new MonitorWall(pos),
                "Server" => new Server(pos),
                "DeskOffice" => new DeskOffice(pos),
                "DeskBackward" => new DeskBackward(pos),
                "DeskForward" => new DeskForward(pos),
                "PottedPlant" => new PottedPlant(pos),
                "BigSink" => new BigSink(pos),
                "CrateStack" => new CrateStack(pos),
                "OperationEquipment" => new OperationEquipment(pos),
                "CarryingThing" => new CarryingThing(pos),
                "Sofa" => new Sofa(pos),
                "Sink" => new Sink(pos),
                "Morgue" => new Morgue(pos),
                "ShelfWeird" => new ShelfWeird(pos),
                "NiceBookshelf" => new NiceBookshelf(pos),
                "CameraObject" => new CameraObject(pos),
                "ShootingRangeBench" => new ShootingRangeBench(pos),
                "Locker" => new Locker(pos),
                "WhiteBoard" => new WhiteBoard(pos),
                "CanteenChairLeft" => new CanteenChairLeft(pos),
                "CanteenChairRight" => new CanteenChairRight(pos),
                "CanteenFoodThing" => new CanteenFoodThing(pos),
                "CanteenTable" => new CanteenTable(pos),
                "SofaLeft" => new SofaLeft(pos),
                "SofaRight" => new SofaRight(pos),
                "KitchenCounter" => new KitchenCounter(pos),
                "Bench" => new Bench(pos),
                "BigScreenTele" => new BigScreenTele(pos),
                "CarsLeft" => new CarLeft(pos),
                "CarsRight" => new CarRight(pos),
                "CopCarLeft" => new CopCarLeft(pos),
                "CopCarRight" => new CopCarRight(pos),
                "ChairBack" => new ChairBack(pos),
                "ChairFront" => new ChairFront(pos),
                "Chest" => new Chest(pos),
                "HumanVialsEmpty" => new HumanVialsEmpty(pos),
                "HumanVialsFilled" => new HumanVialsFilled(pos),
                "HumanVialsNoBody" => new HumanVialsNoBody(pos),
                "NormalScreenTele" => new NormalScreenTele(pos),
                "RadioactiveStain" => new RadioactiveStain(pos),
                "TrashCan" => new TrashCan(pos),
                "WaterStain" => new WaterStain(pos),
                "SmallCarLeft" => new SmallCarLeft(pos),
                "SmallCarRight" => new SmallCarRight(pos),
                "Workstation" => new Workstation(pos),
                "ComputerBack" => new ComputerBack(pos),
                "ComputerFront" => new ComputerFront(pos),
                "BigMonitor" => new BigMonitor(pos),
                "SmallLocker" => new SmallLocker(pos),
                "ShootingRangeTarget" => new ShootingRangeTarget(pos),
                "RailgunPickup" => new RailgunPickup(pos),
                "WinPickup" => new WinPickup(pos),
                _ => null,
            };
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
                    if (currentLevel.tiles[j, i] is WornWall)
                    {
                        types[j, i] = 'f';
                    }
                    if (currentLevel.tiles[j, i] is TileMetal)
                    {
                        types[j, i] = 'h';
                    }
                    if (currentLevel.tiles[j, i] is Water)
                    {
                        types[j, i] = 'i';
                    }
                    if (currentLevel.tiles[j, i] is WallGray)
                    {
                        types[j, i] = 'j';
                    }
                    if (currentLevel.tiles[j, i] is TileWarning)
                    {
                        types[j, i] = 'k';
                    }
                    if (currentLevel.tiles[j, i] is Carpet)
                    {
                        types[j, i] = 'l';
                    }
                    if (currentLevel.tiles[j, i] is GlassWall)
                    {
                        types[j, i] = 'm'; 
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

        internal static List<HintArea> HintAreaLoad(int i)
        {
            List<HintArea> hints = new List<HintArea>();
            if (File.Exists(string.Format(@"maps\level{0}\hints.txt", i)))
            {
                List<string> lines = new List<string>();
                foreach (string line in System.IO.File.ReadLines(string.Format(@"maps\level{0}\hints.txt", i)))
                {
                    lines.Add(line);
                }

               foreach(string line in lines)
                {
                    string[] splitter = line.Split(',');
                    Rectangle area = new Rectangle(Int32.Parse(splitter[0]), Int32.Parse(splitter[1]), Int32.Parse(splitter[2]), Int32.Parse(splitter[3]));
                    string text = splitter[4];
                    InputIcon icon = (InputIcon)(Int32.Parse(splitter[5]));
                    hints.Add(new HintArea(area, text, icon));
                }
            }
            

            return hints;
        }
        
    }
}