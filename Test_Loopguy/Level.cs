using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    public class Level
    {
        public int id;
        Rectangle cameraBounds;
        public Tile[,] tiles;
        public List<LevelObject> levelObjects;
        internal List<Enemy> enemies;
        internal List<Projectile> enemyProjectiles;
        internal List<Projectile> playerProjectiles;
        internal List<Song> idleSongs = new List<Song>();
        internal List<Song> combatSongs = new List<Song>();
        private List<Wall> walls = new List<Wall>();
        private List<Hazard> hazards = new List<Hazard>();
        private List<Destructible> destructibles = new List<Destructible>();
        internal List<HintArea> hints = new List<HintArea>();
        internal List<HintArea> Hints
        {
            get { return hints; }
        }

        internal Level (int id, Rectangle cameraBounds, List<LevelObject> levelObjects, Tile[,] tiles, List<Enemy> enemies)
        {
            this.id = id;
            this.cameraBounds = cameraBounds;
            this.levelObjects = levelObjects;
            this.tiles = tiles;
            this.enemies = enemies;
            //this.levelObjects.Add(new RailgunPickup(new Vector2(128, 128)));
            enemyProjectiles = new List<Projectile>();
            playerProjectiles = new List<Projectile>();
            idleSongs.AddRange(LevelManager.SongLoad(id, false));
            combatSongs.AddRange(LevelManager.SongLoad(id, true));
            walls.AddRange(tiles.OfType<Wall>());
            hazards.AddRange(tiles.OfType<Hazard>());
            destructibles.AddRange(levelObjects.OfType<Destructible>());
            hints = LevelManager.HintAreaLoad(id);
            //hints.Add(new HintArea(new Rectangle(0, 0, 400, 400), "to dash", InputIcon.A));
            if(id == 9)
            {
                //ProfileManager.GiveRailgun();
            }

        }

        public void RefreshMap()
        {
            foreach (Tile t in tiles)
            {
                t.UpdateEdges();
            }
            foreach (Wall w in tiles.OfType<Wall>())
            {
                w.SetConnections();
            }
            walls.Clear();
            hazards.Clear();
            destructibles.Clear();
            walls.AddRange(tiles.OfType<Wall>());
            hazards.AddRange(tiles.OfType<Hazard>());
            destructibles.AddRange(levelObjects.OfType<Destructible>());
        }

        internal void Update(GameTime gameTime, Player player)
        {
            List<Projectile> projectilesToRemove = new List<Projectile>();
            List<Pickup> pickupsToRemove = new List<Pickup>();
            DestructibleUpdate(gameTime);
            EnemyUpdate(gameTime);
            Rectangle mapbounds = new Rectangle(0, 0, cameraBounds.Width * 2 + Game1.windowX, cameraBounds.Height * 2 + Game1.windowY);

            foreach(Tile t in tiles)
            {
                if(t.updateAble)
                {
                    t.Update(gameTime);
                }
            }

            foreach (Projectile s in enemyProjectiles)
            {
                s.Update(gameTime);
                //Make projectiles bounce back if hit by player Melee attack
                if (EntityManager.player.MeleeHit(s) && EntityManager.player.attacking)
                {
                    Projectile reflS = s.Clone();
                    reflS.Bounce((float)Helper.GetAngle(player.centerPosition, reflS.centerPosition, Math.PI));
                    playerProjectiles.Add(reflS);

                    projectilesToRemove.Add(s);
                }

                if (s.CheckCollision(player) && !player.dashing)
                {

                    player.TakeDamage(s.damage, Character.DamageType.laserGun);


                    projectilesToRemove.Add(s);
                }
                
                if (!mapbounds.Contains(s.centerPosition))
                {
                    projectilesToRemove.Add(s);
                }
                foreach (LevelObject lo in levelObjects)
                {
                    if(lo.height > 8 && !(lo is Destructible))
                    {
                        if (s.CheckCollision(lo))
                        {
                            projectilesToRemove.Add(s);
                        }
                    }
                    
                }

                foreach (Wall w in walls)
                {
                    if (s.CheckCollision(w))
                    {
                        projectilesToRemove.Add(s);
                    }
                }
            }
            foreach (Projectile s in playerProjectiles)
            {
                s.Update(gameTime);
                
                foreach(LevelObject lo in levelObjects)
                {
                    if (lo.height > 8 && !(lo is Destructible))
                    {
                        if (s.CheckCollision(lo))
                        {
                            projectilesToRemove.Add(s);
                        }
                    }
                }

                foreach (Wall w in walls)
                {
                    if (s.CheckCollision(w))
                    {
                        projectilesToRemove.Add(s);
                    }
                }

                if (!mapbounds.Contains(s.centerPosition))
                {
                    projectilesToRemove.Add(s);
                }
            }

            foreach (Pickup p in levelObjects.OfType<Pickup>())
            {
                p.Update();
                if(p.pickedUp)
                {
                    pickupsToRemove.Add(p);
                }
            }
            foreach (Door d in levelObjects.OfType<Door>())
            {
                d.Update(gameTime);
            }

            foreach (Projectile s in projectilesToRemove)
            {
                if (playerProjectiles.Contains(s))
                {
                    playerProjectiles.Remove(s);
                }
                if (enemyProjectiles.Contains(s))
                {
                    enemyProjectiles.Remove(s);
                }
            }

            foreach (Pickup p in pickupsToRemove)
            {
                levelObjects.Remove(p);
            }

            foreach(LevelObject lo in LevelManager.objectsToAdd)
            {
                levelObjects.Add(lo);
            }

            foreach(Hazard h in hazards)
            {
                if(player.footprint.Intersects(h.hitBox) && !player.dashing)
                {
                    if(!IsOnGround(player.footprint))
                    {
                        player.EnteredHazard();
                        break;
                    }   
                }

            }

            foreach (HintArea h in hints)
            {
                h.Update();
            }

            LevelManager.objectsToAdd.Clear();
        }

        private bool IsOnGround(Rectangle footprint)
        {

            Rectangle checkPrint = new Rectangle(footprint.X, footprint.Y, footprint.Width, footprint.Height);
            foreach(Floor f in tiles.OfType<Floor>())
            {
                if(f.hitBox.Intersects(checkPrint))
                {
                    return true;
                }
            }
            return false;
        }

        private void DestructibleUpdate(GameTime gameTime)
        {
            List<Destructible> destructiblesToRemove = new List<Destructible>();
            List<Projectile> projectilesToRemove = new List<Projectile>();

            foreach (Destructible lo in destructibles)
            {
                foreach (Projectile s in playerProjectiles)
                {
                    if (s.CheckCollision(lo))
                    {
                        lo.Damage(s.damage);
                        projectilesToRemove.Add(s);
                    }
                }
                foreach (Projectile s in enemyProjectiles)
                {
                    if (s.CheckCollision(lo))
                    {
                        lo.Damage(s.damage);
                        projectilesToRemove.Add(s);
                    }
                }
                if (lo is Destructible && EntityManager.player.MeleeHit(lo) && EntityManager.player.attacking)
                {
                    lo.Damage(1);
                    lo.hitDuringCurrentAttack = true;
                }
                if (!EntityManager.player.attacking)
                {
                    lo.hitDuringCurrentAttack = false;
                }

                lo.Update(gameTime);
                if (lo.actuallyDestroyed)
                {
                    destructiblesToRemove.Add(lo);
                }
            }


            foreach (Destructible d in destructiblesToRemove)
            {
                levelObjects.Remove(d);
                destructibles.Remove(d);
            }

            foreach (Projectile s in projectilesToRemove)
            {
                if(playerProjectiles.Contains(s))
                {
                    playerProjectiles.Remove(s);
                }
                if(enemyProjectiles.Contains(s))
                {
                    enemyProjectiles.Remove(s);
                }
            }
        }

        private void EnemyUpdate(GameTime gameTime)
        {
            List<Enemy> enemiesToRemove = new List<Enemy>();
            List<Projectile> projectilesToRemove = new List<Projectile>();
            if (!Game1.editLevel)
            {
                foreach (Enemy e in enemies)
                {
                    foreach(Projectile s in playerProjectiles)
                    {
                        if(s.CheckCollision(e))
                        {
                            e.TakeDamage(s.damage, Character.DamageType.laserGun);
                            projectilesToRemove.Add(s);
                        }
                    }
                    e.Update(gameTime);
                    if (EntityManager.player.MeleeHit(e) && EntityManager.player.attacking)
                    {
                        e.TakeDamage(1, Character.DamageType.melee);
                        e.hitDuringCurrentAttack = true;                 

                    }
                    if (!EntityManager.player.attacking)
                    {
                        e.hitDuringCurrentAttack = false;
                    }
                    if (!e.isNotDying)
                    {
                        enemiesToRemove.Add(e);
                    }
                }
            }
            foreach (Enemy e in enemiesToRemove)
            {
                enemies.Remove(e);
            }
            foreach (Projectile s in projectilesToRemove)
            {
                if (playerProjectiles.Contains(s))
                {
                    playerProjectiles.Remove(s);
                }
            }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle cullingRect = new Rectangle((int)(EntityManager.player.position.X - 510), (int)(EntityManager.player.position.Y - 300), 1040, 620);
            foreach (Tile t in tiles)
            {
                if(!(t is Wall))
                {
                    if(cullingRect.Contains(t.centerPosition))
                    {
                        t.Draw(spriteBatch);
                    }
                }
               
            }
            foreach (Tile t in tiles)
            {
                t.DrawEdges(spriteBatch);
            }

            

            List<GameObject> objects = new List<GameObject>();
            objects.AddRange(levelObjects);
            objects.AddRange(enemies);
            objects.Add(EntityManager.player);
            objects.AddRange(playerProjectiles);
            objects.AddRange(enemyProjectiles);
            objects.AddRange(walls);
            {
                foreach (LevelObject g in objects.OfType<LevelObject>())
                {
                    g.drawDepth = g.footprint.Bottom;
                }
                foreach (Enemy g in objects.OfType<Enemy>())
                {
                    g.drawDepth = g.footprint.Bottom;
                }
                foreach (Player g in objects.OfType<Player>())
                {
                    g.drawDepth = g.footprint.Bottom;
                }
                foreach (Wall g in objects.OfType<Wall>())
                {
                    g.drawDepth = g.footprint.Bottom;
                }
                foreach (Projectile g in objects.OfType<Projectile>())
                {
                    g.drawDepth = g.hitBox.Center.Y;
                }
                
            }
            

            List<GameObject> sortedList = objects.OrderBy(o => o.drawDepth).ToList();
            objects = sortedList;

            foreach (GameObject g in objects)
            {

                if (g != null)
                {
                    if(cullingRect.Contains(g.centerPosition))
                    g.Draw(spriteBatch);
                }
                
            }

            

        }

        public bool LevelObjectCollision(Rectangle check, int height)
        {
            foreach(LevelObject lo in levelObjects)
            {
                if(lo != null && lo.height > height)
                {
                    if (lo.footprint.Intersects(check))
                    {
                        return true;
                    }
                }
                
            }
            {
                return WallCollision(check.Center.ToVector2());
            }
            

        }
        public bool LevelObjectCollision(Line line, int height)
        {
            Line shortestLine = line;
            foreach (LevelObject lo in levelObjects)
            {
                if (lo != null && lo.height > height)
                {
                    if (line.RectangleIntersects(lo.hitBox) && line.LineFromIntersect(lo.hitBox).Length() < shortestLine.Length())
                    {
                        shortestLine = line.LineFromIntersect(lo.hitBox);
                        return true;
                    }
                }

            }
            {
                return WallCollision(line);
            }
        }
        public Rectangle GetBounds()
        {
            return cameraBounds;
        }

        public void SetBounds(int x, int y)
        {
            cameraBounds.Width = x;
            cameraBounds.Height = y;
        }

        public bool WallCollision(Vector2 check)
        {
            foreach (Wall w in walls)
            {
                if (w.footprint.Contains(check))
                {
                    return true;
                }
            }
            return false;
        }

        public bool WallCollision(Line line)
        {
            Line shortestLine = line;
            foreach (Wall w in walls)
            {
                if (line.RectangleIntersects(w.hitBox) && line.LineFromIntersect(w.hitBox).Length() < shortestLine.Length())
                {
                    shortestLine = line.LineFromIntersect(w.hitBox);
                    return true;
                }
            }
            return false;
        }

        public void AddObject(LevelObject levelObject)
        {
            levelObjects.Add(levelObject);
            //sorts by Y level so objects automatically are drawn in order but its kinda wonky and maybe not good
            List<LevelObject> sortedList = levelObjects.OrderBy(o => o.position.Y + o.texture.Height).ToList();
            levelObjects = sortedList;
            destructibles.AddRange(levelObjects.OfType<Destructible>());
        }

        public void RemoveObject(Vector2 pos)
        {
            LevelObject objectToRemove = null;
            foreach(LevelObject lo in levelObjects)
            {
                if(lo.hitBox.Contains(pos))
                {
                    objectToRemove = lo;
                    break;
                }
            }
            levelObjects.Remove(objectToRemove);
        }

        public void SetMapSize(int x, int y)
        {
            Tile[,] newMap = new Tile[x, y];
            for (int i = 0; i < newMap.GetLength(0); i++)
            {
                for (int j = 0; j < newMap.GetLength(1); j++)
                {
                    if(tiles.GetLength(0) <= i || tiles.GetLength(1) <= j)
                    {
                        newMap[i, j] = new GrassTile(new Vector2(i * 16, j * 16));
                    }
                    else
                    {
                        newMap[i, j] = tiles[i, j];
                    }
                    
                }
            }
            tiles = newMap;
        }

        public void TileEdit(TileSelection tile, Vector2 position)
        {
            
            Tile editedTile = null;
            foreach(Tile t in tiles)
            {
                if(t.hitBox.Contains(position))
                {
                    editedTile = t;
                }
            }

            if(editedTile != null)
            {
                Point coordinates = GetTileCoordinate(editedTile);
                Vector2 gameLocation = new Vector2(coordinates.X * 16, coordinates.Y * 16);
                switch (tile)
                {
                    case TileSelection.Grass:
                        tiles[coordinates.X, coordinates.Y] = new GrassTile(gameLocation);
                        break;
                    case TileSelection.Dirt:
                        tiles[coordinates.X, coordinates.Y] = new DirtTile(gameLocation);
                        break;
                    case TileSelection.GrayBrick:
                        tiles[coordinates.X, coordinates.Y] = new BrickWall(gameLocation);
                        break;
                    case TileSelection.TilesCheckeredGray:
                        tiles[coordinates.X, coordinates.Y] = new CheckeredTileGray(gameLocation);
                        break;
                    case TileSelection.TilesCheckeredBrown:
                        tiles[coordinates.X, coordinates.Y] = new CheckeredTileBrown(gameLocation);
                        break;
                    case TileSelection.TilesBigLight:
                        tiles[coordinates.X, coordinates.Y] = new TileBigLight(gameLocation);
                        break;
                    case TileSelection.TilesBigDark:
                        tiles[coordinates.X, coordinates.Y] = new TileBigDark(gameLocation);
                        break;
                    case TileSelection.TileMetal:
                        tiles[coordinates.X, coordinates.Y] = new TileMetal(gameLocation);
                        break;
                    case TileSelection.WallMetal:
                        tiles[coordinates.X, coordinates.Y] = new WallGray(gameLocation);
                        break;
                    case TileSelection.CarpetWorn:
                        tiles[coordinates.X, coordinates.Y] = new Carpet(gameLocation);
                        break;
                    case TileSelection.DrywallWorn:
                        tiles[coordinates.X, coordinates.Y] = new WornWall(gameLocation);
                        break;
                    case TileSelection.Water:
                        tiles[coordinates.X, coordinates.Y] = new Water(gameLocation);
                        break;
                    case TileSelection.Warning:
                        tiles[coordinates.X, coordinates.Y] = new TileWarning(gameLocation);

                        break;
                }
            }
            walls.AddRange(tiles.OfType<Wall>());
            hazards.AddRange(tiles.OfType<Hazard>());

        }

        public Point GetTileCoordinate(Tile tile)
        {
            
                int xCoordinate = 0;
                int yCoordinate = 0;

                int w = tiles.GetLength(0);
                int h = tiles.GetLength(1);

                for (int x = 0; x < w; ++x)
                {
                    for (int y = 0; y < h; ++y)
                    {
                        if (tiles[x, y].Equals(tile))
                        {
                            xCoordinate = x;
                            yCoordinate = y;
                        }
                    }
                }

            return new Point(xCoordinate, yCoordinate);
            
        }

    }
    public class Entrance
    {
        int id;
        int level1;
        int level2;
        Rectangle hitbox;
        Vector2 target;
        public Entrance(int id, int lvl1, int lvl2, Rectangle gate, Vector2 target)
        {
            this.id = id;
            level1 = lvl1;
            level2 = lvl2;
            hitbox = gate;
            this.target = target;
        }

        internal void CheckGate(Player player)
        {
            if(hitbox.Contains(player.centerPosition) && LevelManager.GetCurrentId() == level1 && LevelManager.loadStarted == false)
            {
                
                Fadeout.LevelTransitionFade();
                LevelManager.StartLevelTransition(level2, player, target);
                
            }
        }
    }
}