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

        internal Level (int id, Rectangle cameraBounds, List<LevelObject> levelObjects, Tile[,] tiles, List<Enemy> enemies)
        {
            this.id = id;
            this.cameraBounds = cameraBounds;
            this.levelObjects = levelObjects;
            this.tiles = tiles;
            this.enemies = enemies;
            enemyProjectiles = new List<Projectile>();
            playerProjectiles = new List<Projectile>();
            idleSongs.AddRange(LevelManager.SongLoad(id, false));
            combatSongs.AddRange(LevelManager.SongLoad(id, true));

        }

        public void RefreshEdges()
        {
            foreach (Tile t in tiles)
            {
                t.UpdateEdges();
            }
            foreach (Wall w in tiles.OfType<Wall>())
            {
                w.SetConnections();
            }
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
                    player.TakeDamage(1);
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

                foreach (Tile w in tiles)
                {
                    if (w is Wall)
                    {
                        if (s.CheckCollision(w))
                        {
                            projectilesToRemove.Add(s);
                        }
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

                foreach (Tile w in tiles)
                {
                    if(w is Wall)
                    {
                        if (s.CheckCollision(w))
                        {
                            projectilesToRemove.Add(s);
                        }
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

            foreach(Hazard h in tiles.OfType<Hazard>())
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

            foreach (Destructible lo in levelObjects.OfType<Destructible>())
            {
                foreach (Projectile s in playerProjectiles)
                {
                    if (s.CheckCollision(lo))
                    {
                        lo.Damage(1);
                        projectilesToRemove.Add(s);
                    }
                }
                foreach (Projectile s in enemyProjectiles)
                {
                    if (s.CheckCollision(lo))
                    {
                        lo.Damage(1);
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
                            e.TakeDamage(1);
                            projectilesToRemove.Add(s);
                        }
                    }
                    e.Update(gameTime);
                    if (EntityManager.player.MeleeHit(e) && EntityManager.player.attacking)
                    {
                        e.TakeDamage(1);
                        e.hitDuringCurrentAttack = true;
                    }
                    if (!EntityManager.player.attacking)
                    {
                        e.hitDuringCurrentAttack = false;
                    }
                    if (e.health <= 0)
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
            foreach (Tile t in tiles)
            {
                if(t is Floor || t is Hazard)
                t.Draw(spriteBatch);
            }
            foreach (Tile t in tiles)
            {
                t.DrawEdges(spriteBatch);
            }
            foreach (Tile t in tiles.OfType<Wall>())
            {
                //t.Draw(spriteBatch);
            }


            List<GameObject> objects = new List<GameObject>();
            objects.AddRange(levelObjects);
            objects.AddRange(enemies);
            objects.Add(EntityManager.player);
            objects.AddRange(playerProjectiles);
            objects.AddRange(enemyProjectiles);
            objects.AddRange(tiles.OfType<Wall>());

            List<GameObject> sortedList = objects.OrderBy(o => o.centerPosition.Y + o.texture.Height).ToList();
            objects = sortedList;

            foreach (GameObject g in objects)
            {

                if (g != null)
                {
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
            foreach (LevelObject lo in levelObjects)
            {
                if (lo != null && lo.height > height)
                {
                    if (line.RectangleIntersects(lo.footprint))
                    {
                        return true;
                    }
                }

            }
            {
                //return WallCollision(check.Center.ToVector2());
            }
            return false;
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
            //sorts by Y level so objects automatically are drawn in order but its kinda wonky and maybe not good
            List<LevelObject> sortedList = levelObjects.OrderBy(o => o.position.Y + o.texture.Height).ToList();
            levelObjects = sortedList;
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
                        tiles[coordinates.X, coordinates.Y] = new CarpetWorn(gameLocation);
                        break;
                    case TileSelection.DrywallWorn:
                        tiles[coordinates.X, coordinates.Y] = new WornWall(gameLocation);
                        break;
                    case TileSelection.Water:
                        tiles[coordinates.X, coordinates.Y] = new Water(gameLocation);

                        break;
                }
            }
            foreach (Tile t in tiles)
            {
                //t.UpdateEdges();
            }
            foreach (Wall w in tiles.OfType<Wall>())
            {
                //w.SetConnections();
            }

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