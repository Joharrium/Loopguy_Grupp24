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
        public int[,] heightMap;
        public List<LevelObject> levelObjects;
        //list of enemies
        //list of corresponding entrances from different ids and their position


        public Level (int id, Rectangle cameraBounds, List<LevelObject> levelObjects, Tile[,] tiles/*, List<Entrance> entrances*/)
        {
            this.id = id;
            this.cameraBounds = cameraBounds;
            this.levelObjects = levelObjects;
            this.tiles = tiles;
            heightMap = new int[tiles.GetLength(0),tiles.GetLength(1)];
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    heightMap[i, j] = 1;

                }
            }
        }

        internal void Update(GameTime gameTime, Player player)
        {
            RefreshTileEdges();
            List<Destructible> destructiblesToRemove = new List<Destructible>();
            foreach(Destructible lo in levelObjects.OfType<Destructible>())
            {
                if(lo is Destructible && lo.hitBox.Intersects(player.hitBox))
                {
                    lo.Damage(1);
                }

                lo.Update(gameTime);
                if(lo.actuallyDestroyed)
                {
                    destructiblesToRemove.Add(lo);
                }
            }

            foreach(Destructible d in destructiblesToRemove)
            {
                levelObjects.Remove(d);
            }
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

            if(Game1.editLevel)
            {
                for(int i = 0; i<tiles.GetLength(0); i++)
                {
                    for(int j = 0; j<tiles.GetLength(1); j++)
                    {
                        spriteBatch.DrawString(Game1.smallFont, heightMap[i, j].ToString(), new Vector2(i * 16, j * 16), Color.White);
                        
                    }
                }
            }
            //dra1w tiles and objects and enemies, in the correct order
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

        public void HeightEdit(Vector2 position, int height)
        {
            Tile editedTile = null;
            foreach (Tile t in tiles)
            {
                if (t.hitBox.Contains(position))
                {
                    editedTile = t;
                }
            }

            if (editedTile != null)
            {
                Point coordinates = GetTileCoordinate(editedTile);
                heightMap[coordinates.X, coordinates.Y] = height;
            }
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
                    case TileSelection.CliffGray:
                        tiles[coordinates.X, coordinates.Y] = new CliffGray(gameLocation);
                        break;
                }
            }

            RefreshTileEdges();
            
        }

        public void RefreshTileEdges()
        {
            foreach(CliffGray cliff in tiles.OfType<CliffGray>())
            {
                cliff.RefreshEdges(tiles);
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