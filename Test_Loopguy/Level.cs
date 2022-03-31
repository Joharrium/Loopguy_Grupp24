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
        int id;
        Rectangle cameraBounds;
        public Tile[,] tiles;
        public List<LevelObject> levelObjects;
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
            //sorts by Y level so objects automatically are drawn in order but its kinda wonky and maybe not good
            //List<LevelObject> sortedList = levelObjects.OrderBy(o => o.position.Y + o.texture.Height).ToList();
            //levelObjects = sortedList;
        }

        public void SetMapSize(int x, int y)
        {
            Tile[,] newMap = new Tile[x, y];
            for (int i = 0; i < newMap.GetLength(0); i++)
            {
                for (int j = 0; j < newMap.GetLength(1); j++)
                {
                    newMap[i, j] = tiles[i, j];
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

                        break;
                }
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
        Vector2 position;
        int idFrom;
        int idTo;
    }
}