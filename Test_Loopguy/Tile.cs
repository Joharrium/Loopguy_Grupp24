﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    public class Tile : GameObject
    {
        public Rectangle sourceRectangle;
        protected float rotation;
        protected SpriteEffects spriteEffects;
        protected TileEdge edges;
        public Tile(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.testAlt;
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

        public void DrawEdges(SpriteBatch spriteBatch)
        {
            if (edges != null)
            {
                edges.Draw(spriteBatch);
            }
        }

        public void UpdateEdges()
        {
            if(edges != null)
            {
                edges.UpdateEdges(this);
            }
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
                texture = TextureManager.grassBasic;
                variation = Game1.rnd.Next(2);
            }
            else
            {
                texture = TextureManager.grassAlt;
                variation = Game1.rnd.Next(4);
            }
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 16);
            edges = new TileEdge(TextureManager.grass_edge, this);
        }
    }

    public class DirtTile : Floor
    {
        int variation;
        public DirtTile(Vector2 position) : base(position)
        {
            this.position = position;

            texture = TextureManager.dirt;
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

            texture = TextureManager.tiles_big_light;
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

            texture = TextureManager.tiles_big_dark;
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

            texture = TextureManager.tiles_checkered_gray;
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

            texture = TextureManager.tiles_checkered_brown;
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

    public class TileMetal : Floor
    {
        int variation;
        public TileMetal(Vector2 position) : base(position)
        {
            this.position = position;

            texture = TextureManager.tile_metal;
            variation = Game1.rnd.Next(1);

            sourceRectangle = new Rectangle(16 * variation, 0, 16, 16);
        }
    }

    public class CarpetWorn : Floor
    {
        int variation;
        public CarpetWorn(Vector2 position) : base(position)
        {
            this.position = position;

            texture = TextureManager.carpet_worn;
            variation = Game1.rnd.Next(3);


            sourceRectangle = new Rectangle(16 * variation, 0, 16, 16);
        }
    }

    public class Wall : Tile
    {

        public Wall(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.box;
            hitBox.X = (int)position.X;
            hitBox.Y = (int)position.Y;
            hitBox.Width = texture.Width;
            hitBox.Height = texture.Height;

            centerPosition = new Vector2(position.X - texture.Width / 2, position.Y - texture.Height / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position + new Vector2(hitBox.Width / 2, hitBox.Height / 2), sourceRectangle, Color.White, rotation, new Vector2(hitBox.Width / 2, hitBox.Height / 2), 1, spriteEffects, 1);
        }
    }

    public class BrickWall : Wall
    {
        public BrickWall(Vector2 position) : base(position)
        {
            this.position = position - new Vector2(0,16);
            texture = TextureManager.grayBrickWall;
            hitBox.Width = 16;
            hitBox.Height = 16;
            sourceRectangle = new Rectangle(16 * 0, 0, 16, 32);
        }
    }

    public class MetalWall : Wall
    {
        int variation;
        public MetalWall(Vector2 position) : base(position)
        {
            this.position = position - new Vector2(0, 16);
            texture = TextureManager.wall_metal;
            hitBox.Width = 16;
            hitBox.Height = 16;
            int variation = Game1.rnd.Next(3);
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 32);
        }
    }

    public class WornWall : Wall
    {
        int variation;
        public WornWall(Vector2 position) : base(position)
        {
            this.position = position - new Vector2(0, 16);
            texture = TextureManager.wall_worn;
            hitBox.Width = 16;
            hitBox.Height = 16;
            variation = Game1.rnd.Next(4);
            if(variation == 1)
            {
                variation = Game1.rnd.Next(4);
            }
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 32);
        }
    }
}
