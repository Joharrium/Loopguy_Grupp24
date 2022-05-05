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
        internal AnimatedSprite anim;
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
            if(anim != null)
            {
                anim.Draw(spriteBatch);
            }
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
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
            
            
        }
    }

    public class Console : LevelObject
    {
        public Console(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.console;
            sourceRectangle.Width = 32;
            sourceRectangle.Height = 24;
            footprint = new Rectangle((int)position.X, (int)position.Y + 14, 32, 10);
            height = 12;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);


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
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class TreeBig : LevelObject
    {
        public TreeBig(Vector2 position) : base(position)
        {
            this.position = position;
            variation = Game1.rnd.Next(4);
            texture = TextureManager.tree_big;
            sourceRectangle = new Rectangle(48 * variation, 0, 48, 48);
            footprint = new Rectangle((int)position.X + 11, (int)position.Y + 30, 26, 16);
            height = 44;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class TreeSmall : LevelObject
    {
        public TreeSmall(Vector2 position) : base(position)
        {
            this.position = position;
            variation = Game1.rnd.Next(4);
            texture = TextureManager.tree_small;
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 32);

            footprint = new Rectangle((int)position.X + 2, (int)position.Y + 21, 12, 8);
            height = 28;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
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
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class ShrubBig : LevelObject
    {
        public ShrubBig(Vector2 position) : base(position)
        {
            this.position = position;
            variation = Game1.rnd.Next(4);
            texture = TextureManager.shrub_big;
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 16);

            footprint = new Rectangle((int)position.X, (int)position.Y + 4, 16, 12);
            height = 5;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
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
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class BillBoard : LevelObject
    {
        public BillBoard(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.billboard;

            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(32 * variation, 0, 32, 32);

            footprint = new Rectangle((int)position.X, (int)position.Y + 4, 0, 0);
            height = 0;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class Cabinet : LevelObject
    {
        public Cabinet(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.cabinet;
            variation = Game1.rnd.Next(3);
            sourceRectangle = new Rectangle(32 * variation, 0, 32, 40);
            footprint = new Rectangle((int)position.X + 2, (int)position.Y + 28, 27, 10);
            height = 27;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class CardboardBoxStackSmall : LevelObject
    {
        public CardboardBoxStackSmall(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.cardboard_box_stack_small;
            variation = Game1.rnd.Next(1);
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 16);
            footprint = new Rectangle((int)position.X, (int)position.Y + 10, 12, 5);
            height = 9;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class Counter : LevelObject
    {
        public Counter(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.counter;
            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(32 * variation, 0, 32, 28);
            footprint = new Rectangle((int)position.X + 2, (int)position.Y + 17, 28, 9);
            height = 7;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }
    public class ChairOfficeBw : LevelObject
    {
        public ChairOfficeBw(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.chair_office_bw;
            variation = Game1.rnd.Next(3);
            sourceRectangle = new Rectangle(12 * variation, 0, 12, 24);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 14, 12, 8);
            height = 7;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }
    public class ChairOfficeFw : LevelObject
    {
        public ChairOfficeFw(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.chair_office_fw;
            variation = Game1.rnd.Next(3);
            sourceRectangle = new Rectangle(12 * variation, 0, 12, 24);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 16, 12, 8);
            height = 7;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class Server : LevelObject
    {
        public Server(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.server;
            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 34);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 25, 16, 9);
            height = 24;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class ShelfArchiving : LevelObject
    {
        public ShelfArchiving(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.shelf_archiving;
            variation = Game1.rnd.Next(5);
            sourceRectangle = new Rectangle(32 * variation, 0, 32, 36);
            footprint = new Rectangle((int)position.X + 2, (int)position.Y + 28, 30, 7);
            height = 20;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class ShelfArchivingSmall : LevelObject
    {
        public ShelfArchivingSmall(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.shelf_archiving_small;
            variation = Game1.rnd.Next(5);
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 28);
            footprint = new Rectangle((int)position.X + 1, (int)position.Y + 20, 16, 7);
            height = 20;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class MonitorWall : LevelObject
    {
        public MonitorWall(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.monitor_wall;
            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(60 * variation, 0, 60, 36);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 0, 0, 0);
            height = 20;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class DeskOffice : LevelObject
    {
        public DeskOffice(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.desk_office;
            variation = Game1.rnd.Next(4);
            sourceRectangle = new Rectangle(48 * variation, 0, 48, 32);
            footprint = new Rectangle((int)position.X + 1, (int)position.Y + 18, 46, 14);
            height = 7;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }
}
