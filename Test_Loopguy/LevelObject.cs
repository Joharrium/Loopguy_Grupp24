using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    public class LevelObject : GameObject
    {
        public Rectangle sourceRectangle;
        float rotation;
        SpriteEffects spriteEffects;
        protected int variation = 0;
        internal Rectangle footprint;
        internal int height;
        public LevelObject(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.box;
            hitBox.X = (int)position.X;
            hitBox.Y = (int)position.Y;
            hitBox.Width = texture.Width;
            hitBox.Height = texture.Height;
            sourceRectangle.Width = 16;
            sourceRectangle.Height = 16;

            centerPosition = new Vector2(position.X - texture.Width / 2, position.Y - texture.Height / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position + new Vector2(hitBox.Width/2, hitBox.Height/2), sourceRectangle, Color.White, rotation, new Vector2(hitBox.Width / 2, hitBox.Height / 2), 1, spriteEffects, 1);
        }
    }

    


    public class Box : LevelObject
    {
        public Box(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.box;
            sourceRectangle.Width = 16;
            sourceRectangle.Height = 24;
            footprint = new Rectangle((int)position.X, (int)position.Y + 10, 16, 14);
            height = 12;
            
            
        }
    }

    public class BoxOpen : LevelObject
    {
        public BoxOpen(Vector2 position) : base(position)
        {
            this.position = position;
            variation = Game1.rnd.Next(12);
            texture = TextureManager.boxOpen;
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 16);
            footprint = new Rectangle((int)position.X, (int)position.Y + 6, 16, 10);
            height = 6;
        }
    }

    public class TreeBig : LevelObject
    {
        public TreeBig(Vector2 position) : base(position)
        {
            this.position = position;
            variation = Game1.rnd.Next(4);
            texture = TextureManager.tree_big;
            hitBox.Width = 48;
            hitBox.Height = 48;
            sourceRectangle = new Rectangle(48 * variation, 0, 48, 48);
            footprint = new Rectangle((int)position.X + 11, (int)position.Y + 30, 26, 16);
            height = 44;
        }
    }

    public class TreeSmall : LevelObject
    {
        public TreeSmall(Vector2 position) : base(position)
        {
            this.position = position;
            variation = Game1.rnd.Next(4);
            texture = TextureManager.tree_small;
            hitBox.Width = 16;
            hitBox.Height = 24;
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 32);

            footprint = new Rectangle((int)position.X + 2, (int)position.Y + 21, 12, 8);
            height = 28;
        }
    }

    public class Barrel : LevelObject
    {
        public Barrel(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.barrel;
            sourceRectangle.Height = 16;
            footprint = new Rectangle((int)position.X, (int)position.Y + 6, 16, 10);
            height = 9;
        }
    }

    public class ShrubBig : LevelObject
    {
        public ShrubBig(Vector2 position) : base(position)
        {
            this.position = position;
            variation = Game1.rnd.Next(4);
            texture = TextureManager.shrub_big;
            hitBox.Width = 16;
            hitBox.Height = 12;
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 16);

            footprint = new Rectangle((int)position.X, (int)position.Y + 4, 16, 12);
            height = 5;
        }
    }

    public class Pot : LevelObject
    {
        public Pot(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.pot;
            sourceRectangle.Height = 16;
            footprint = new Rectangle((int)position.X, (int)position.Y + 4, 16, 12);
            height = 6;
        }
    }
}
