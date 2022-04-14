using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    public enum TileEdge
    {
        North, East, South, West

    }
    public class Edges
    {
        public bool north;
        public bool east;
        public bool south;
        public bool west;

        public bool northeast;
        public bool southeast;
        public bool southwest;
        public bool northwest;

        public Rectangle RefreshEdges(Tile[,] tiles, Tile self)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, 16, 16);

            int xPos = 0;
            int yPos = 0;

            int w = tiles.GetLength(0);
            int h = tiles.GetLength(1);

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (tiles[x, y].Equals(self))
                    {
                        xPos = x;
                        yPos = y;
                    }
                }
            }

            //ok so fuck all of this
            //new idea:
            //a separate heightmap document with different numbers indicating different heights
            //at the edge of different heights, cliffs will be generated

            //checks neighboring tiles
            {
                if (tiles[xPos - 1, yPos - 1].GetType().Equals(self.GetType()))
                {
                    northwest = true;
                }
                if (tiles[xPos, yPos - 1].GetType().Equals(self.GetType()))
                {
                    north = true;
                }
                if (tiles[xPos + 1, yPos - 1].GetType().Equals(self.GetType()))
                {
                    northeast = true;
                }
                if (tiles[xPos - 1, yPos].GetType().Equals(self.GetType()))
                {
                    west = true;
                }
                if (tiles[xPos + 1, yPos].GetType().Equals(self.GetType()))
                {
                    east = true;
                }
                if (tiles[xPos - 1, yPos + 1].GetType().Equals(self.GetType()))
                {
                    southwest = true;
                }
                if (tiles[xPos, yPos + 1].GetType().Equals(self.GetType()))
                {
                    south = true;
                }
                if (tiles[xPos + 1, yPos + 1].GetType().Equals(self.GetType()))
                {
                    southeast = true;
                }
            }

            //sets source rectangle
            {
                
                if(!south)
                {
                    sourceRectangle.Y = 0;
                    //outer corner NW, NE and straight wall N
                    if (!east && north && west && northwest)
                    {
                        sourceRectangle.X = 0;
                    }
                    if (!east && !west && north)
                    {
                        sourceRectangle.X = 16;
                    }
                    if (!west && north && east)
                    {
                        sourceRectangle.X = 32;
                    }
                }

                if(!south && !north && west && east)
                {
                    sourceRectangle.X = 16;
                    sourceRectangle.Y = 16;
                }

                if (south && north && !west && !east)
                {
                    sourceRectangle.X = 80;
                    sourceRectangle.Y = 0;
                }

                //outer corner SW, SE and straight wall S
                if (!north)
                {
                    sourceRectangle.Y = 16;
                    if(!east && south && west)
                    {
                        sourceRectangle.X = 0;
                    }
                    if(!east && south && !west)
                    {
                        sourceRectangle.X = 16;
                    }
                    if(east && south && !west)
                    {
                        sourceRectangle.X = 32;
                    }
                }
                


                
                //straight walls to east and west
                if (!south && !north && !east && west)
                {
                    sourceRectangle.X = 80;
                    sourceRectangle.Y = 0;
                }
                if (!south && !north && east && !west)
                {
                    sourceRectangle.X = 80;
                    sourceRectangle.Y = 16;
                }


                //inner corner walls
                if (!south && !west && !north && !east)
                {
                    if(northeast || southeast)
                    {
                        sourceRectangle.X = 48;
                    }
                    if(northwest || southwest)
                    {
                        sourceRectangle.X = 64;
                    }
                    if(southeast || southwest)
                    {
                        sourceRectangle.Y = 16;
                    }
                    if(northeast || northwest)
                    {
                        sourceRectangle.Y = 0;
                    }
                }
            }

            //checks neighboring tiles, assigns cardinal direction bools if self type is same as neighbor
            //make a standard for tiles with edge overlaps which creates correct source rectangle
            //0,0 edges to west and north
            //16,0 edges to north
            //32,0 deges to east and north
            //48,0 edges only northeast
            //64,0 edges only northwest 
            //80,0 edges to west

            //0,16 edges to west and south
            //16,16 edges to south
            //32,16 deges to right and south
            //48,16 edges only southeast
            //64,16 edges only southwest
            //80,16 edges to east

            return sourceRectangle;
        }
    }

    public class Tile : GameObject
    {
        public Rectangle sourceRectangle;
        float rotation;
        SpriteEffects spriteEffects;
        public Tile(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TexMGR.testAlt;
            hitBox.X = (int)position.X;
            hitBox.Y = (int)position.Y;
            hitBox.Width = texture.Width;
            hitBox.Height = texture.Height;

            centerPosition = new Vector2(position.X - texture.Width / 2, position.Y - texture.Height / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position + new Vector2(hitBox.Width/2, hitBox.Height/2), sourceRectangle, Color.White, rotation, new Vector2(hitBox.Width / 2, hitBox.Height / 2), 1, spriteEffects, 1);
        }
    }

    public class Floor : Tile
    {
        public Floor(Vector2 position) : base(position)
        {
            this.position = position;
        }
    }

    

    public class GrassTile : Floor
    {
        int variation;
        public GrassTile(Vector2 position) : base(position)
        {
            this.position = position;

            int randomizer = Game1.rnd.Next(10);
            if (randomizer < 8)
            {
                texture = TexMGR.grassBasic;
                variation = Game1.rnd.Next(2);
            }
            else
            {
                texture = TexMGR.grassAlt;
                variation = Game1.rnd.Next(4);
            }
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 16);
        }
    }

    public class DirtTile : Floor
    {
        int variation;
        public DirtTile(Vector2 position) : base(position)
        {
            this.position = position;

            texture = TexMGR.dirt;
            variation = Game1.rnd.Next(4);
            
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 16);
        }
    }

    public class TileBigLight : Floor
    {
        int variation;
        public TileBigLight(Vector2 position) : base(position)
        {
            this.position = position;

            texture = TexMGR.tiles_big_light;
            variation = Game1.rnd.Next(2);

            sourceRectangle = new Rectangle(16 * variation, 0, 16, 16);
        }
    }

    public class TileBigDark : Floor
    {
        int variation;
        public TileBigDark(Vector2 position) : base(position)
        {
            this.position = position;

            texture = TexMGR.tiles_big_dark;
            variation = Game1.rnd.Next(2);

            sourceRectangle = new Rectangle(16 * variation, 0, 16, 16);
        }
    }

    public class CheckeredTileGray : Floor
    {
        int variation;
        public CheckeredTileGray(Vector2 position) : base(position)
        {
            this.position = position;

            texture = TexMGR.tiles_checkered_gray;
            int randomizer = Game1.rnd.Next(10);
            if (randomizer == 9)
            {
                variation = 2;
            }
            else
            {
                variation = Game1.rnd.Next(2);
            }

            sourceRectangle = new Rectangle(16 * variation, 0, 16, 16);
        }
    }

    public class CheckeredTileBrown : Floor
    {
        int variation;
        public CheckeredTileBrown(Vector2 position) : base(position)
        {
            this.position = position;

            texture = TexMGR.tiles_checkered_brown;
            int randomizer = Game1.rnd.Next(10);
            if(randomizer == 9)
            {
                variation = 2;
            }
            else
            {
                variation = Game1.rnd.Next(2);
            }
            

            sourceRectangle = new Rectangle(16 * variation, 0, 16, 16);
        }
    }

    public class Wall : Tile
    {

        public Wall(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TexMGR.box;
            hitBox.X = (int)position.X;
            hitBox.Y = (int)position.Y;
            hitBox.Width = texture.Width;
            hitBox.Height = texture.Height;

            centerPosition = new Vector2(position.X - texture.Width / 2, position.Y - texture.Height / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }

    public class BrickWall : Wall
    {
        public BrickWall(Vector2 position) : base(position)
        {
            this.position = position - new Vector2(0,16);
            texture = TexMGR.grayBrickWall;
            hitBox.Width = 16;
            hitBox.Height = 16;
        }
    }
}
