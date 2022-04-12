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

        public Rectangle RefreshEdges(Tile[] tiles, Tile self)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, 0, 0);
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

    public class Cliff : Tile
    {
        public Cliff(Vector2 position) : base(position)
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
