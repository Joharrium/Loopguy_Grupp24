using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
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
