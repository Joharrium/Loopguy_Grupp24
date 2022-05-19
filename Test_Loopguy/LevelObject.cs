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

            footprint = new Rectangle((int)position.X, (int)position.Y + 48, 0, 0);
            height = 0;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class WhiteBoard : LevelObject
    {
        public WhiteBoard(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.whiteboard;

            variation = Game1.rnd.Next(3);
            sourceRectangle = new Rectangle(32 * variation, 0, 32, 24);

            footprint = new Rectangle((int)position.X, (int)position.Y + 48, 0, 0);
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
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 36, 0, 0);
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

    public class DeskForward : LevelObject
    {
        public DeskForward(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.desk_fw;
            variation = Game1.rnd.Next(4);
            sourceRectangle = new Rectangle(48 * variation, 0, 48, 32);
            footprint = new Rectangle((int)position.X + 2, (int)position.Y + 14, 42, 16);
            height = 6;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }
    public class DeskBackward : LevelObject
    {
        public DeskBackward(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.desk_bw;
            variation = Game1.rnd.Next(4);
            sourceRectangle = new Rectangle(48 * variation, 0, 48, 32);
            footprint = new Rectangle((int)position.X + 2, (int)position.Y + 14, 42, 16);
            height = 6;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }
    public class PottedPlant : LevelObject
    {
        public PottedPlant(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.potted_plant;
            variation = Game1.rnd.Next(3);
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 32);
            footprint = new Rectangle((int)position.X + 3, (int)position.Y + 24, 10, 6);
            height = 24;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class OperationEquipment : LevelObject
    {
        public OperationEquipment(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.table_operation;
            variation = Game1.rnd.Next(3);
            sourceRectangle = new Rectangle(28 * variation, 0, 28, 24);
            footprint = new Rectangle((int)position.X + 1, (int)position.Y + 8, 26, 16);
            height = 7;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }
    public class CarryingThing : LevelObject
    {
        public CarryingThing(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.carrying_thing;
            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(20 * variation, 0, 20, 36);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 22, 19, 14);
            height = 30;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class CrateStack : LevelObject
    {
        public CrateStack(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.crate_stack;
            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(26 * variation, 0, 26, 22);
            footprint = new Rectangle((int)position.X + 3, (int)position.Y + 24, 25, 10);
            height = 13;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class BigSink : LevelObject
    {
        public BigSink(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.big_sink;
            variation = Game1.rnd.Next(1);
            sourceRectangle = new Rectangle(46 * variation, 0, 46, 24);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 8, 46, 16);
            height = 7;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }
    public class Sofa : LevelObject
    {
        public Sofa(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.sofa;
            variation = Game1.rnd.Next(1);
            sourceRectangle = new Rectangle(48 * variation, 0, 48, 28);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 12, 48, 16);
            height = 12;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class Sink : LevelObject
    {
        public Sink(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.sink;
            variation = Game1.rnd.Next(1);
            sourceRectangle = new Rectangle(36 * variation, 0, 36, 20);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 10, 36, 10);
            height = 8;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class Morgue : LevelObject
    {
        public Morgue(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.morgue;
            variation = Game1.rnd.Next(1);
            sourceRectangle = new Rectangle(32 * variation, 0, 32, 54);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 30, 32, 24);
            height = 30;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class ShelfWeird : LevelObject
    {
        public ShelfWeird(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.shelf_weird;
            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(32 * variation, 0, 32, 32);
            footprint = new Rectangle((int)position.X + 1, (int)position.Y + 21, 30, 10);
            height = 20;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class NiceBookshelf : LevelObject
    {
        public NiceBookshelf(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.nice_bookshelf;
            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(64 * variation, 0, 64, 34);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 26, 32, 10);
            height = 24;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class CameraObject : LevelObject
    {
        public CameraObject(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.camera ;
            variation = Game1.rnd.Next(1);
            sourceRectangle = new Rectangle(20 * variation, 0, 20, 24);
            footprint = new Rectangle((int)position.X + 1, (int)position.Y + 16, 16, 8);
            height = 24;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class ShootingRangeBench : LevelObject
    {
        public ShootingRangeBench(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.shooting_range_bench;
            variation = Game1.rnd.Next(1);
            sourceRectangle = new Rectangle(48 * variation, 0, 48, 32);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 22, 48, 10);
            height = 6;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class Locker : LevelObject
    {
        public Locker(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.locker;
            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 32);
            footprint = new Rectangle((int)position.X + 1, (int)position.Y + 19, 13, 10);
            height = 16;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }
}
