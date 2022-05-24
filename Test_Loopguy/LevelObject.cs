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

            footprint = new Rectangle((int)position.X, (int)position.Y + 36, 0, 0);
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

            footprint = new Rectangle((int)position.X, (int)position.Y + 36, 0, 0);
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
            sourceRectangle = new Rectangle(32 * variation, 0, 32, 34);
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
    public class CanteenFoodThing : LevelObject
    {
        public CanteenFoodThing(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.canteen_food_thing;
            variation = Game1.rnd.Next(3);
            sourceRectangle = new Rectangle(64 * variation, 0, 64, 24);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 12, 64, 12);
            height = 8;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class CanteenTable : LevelObject
    {
        public CanteenTable(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.canteen_table;
            variation = Game1.rnd.Next(4);
            sourceRectangle = new Rectangle(24 * variation, 0, 24, 54);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 8, 24, 46);
            height = 8;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class KitchenCounter : LevelObject
    {
        public KitchenCounter(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.kitchen_counter;
            variation = Game1.rnd.Next(3);
            sourceRectangle = new Rectangle(48 * variation, 0, 48, 24);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 14, 48, 10);
            height = 8;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class CanteenChairLeft : LevelObject
    {
        public CanteenChairLeft(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.canteen_chair_left;
            variation = Game1.rnd.Next(1);
            sourceRectangle = new Rectangle(14 * variation, 0, 14, 22);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 14, 14, 8);
            height = 8;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class CanteenChairRight : LevelObject
    {
        public CanteenChairRight(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.canteen_chair_right;
            variation = Game1.rnd.Next(1);
            sourceRectangle = new Rectangle(14 * variation, 0, 14, 22);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 14, 14, 8);
            height = 8;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class SofaLeft : LevelObject
    {
        public SofaLeft(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.sofa_left;
            variation = Game1.rnd.Next(1);
            sourceRectangle = new Rectangle(24 * variation, 0, 24, 42);
            footprint = new Rectangle((int)position.X + 1, (int)position.Y + 17, 22, 24);
            height = 14;
            hitBox = new Rectangle(footprint.X + 13, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class SofaRight : LevelObject
    {
        public SofaRight(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.sofa_right;
            variation = Game1.rnd.Next(1);
            sourceRectangle = new Rectangle(24 * variation, 0, 24, 42);
            footprint = new Rectangle((int)position.X + 1, (int)position.Y + 17, 22, 24);
            height = 14;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width - 13, footprint.Height);
        }
    }

    public class Bench : LevelObject
    {
        public Bench(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.bench;
            variation = Game1.rnd.Next(1);
            sourceRectangle = new Rectangle(46 * variation, 0, 46, 15);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 4, 46, 1);
            height = 5;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class BigScreenTele : LevelObject
    {
        public BigScreenTele(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.bigScreenTele;
            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(48 * variation, 0, 48, 22);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 34, 0, 0);
            height = 8;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class NormalScreenTele : LevelObject
    {
        public NormalScreenTele(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.normalScreenTele;
            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(32 * variation, 0, 32, 17);
            height = 8;
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 34, 0, 0);
        }
    }

    public class CarLeft : LevelObject
    {
        public CarLeft(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.carsLeft;
            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(48 * variation, 0, 48, 26);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 14, 48, 12);
            height = 8;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class CarRight : LevelObject
    {
        public CarRight(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.carsRight;
            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(48 * variation, 0, 48, 26);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 14, 48, 14);
            height = 8;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class CopCarLeft : LevelObject
    {
        public CopCarLeft(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.copCarLeft;
            variation = Game1.rnd.Next(1);
            sourceRectangle = new Rectangle(48 * variation, 9, 48, 24);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 9, 48, 10);
            height = 8;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);

        }
    }

    public class CopCarRight : LevelObject
    {
        public CopCarRight(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.copCarRight;
            variation = Game1.rnd.Next(1);
            sourceRectangle = new Rectangle(48 * variation, 9, 48, 24);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 9, 48, 10);
            height = 8;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class SmallCarLeft : LevelObject
    {
        public SmallCarLeft(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.smallCarLeft;
            variation = Game1.rnd.Next(1);
            sourceRectangle = new Rectangle(32 * variation, 0, 32, 24);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 12, 32, 12);
            height = 8;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class SmallCarRight : LevelObject
    {
        public SmallCarRight(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.smallCarRight;
            variation = Game1.rnd.Next(1);
            sourceRectangle = new Rectangle(32 * variation, 0, 32, 24);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 12, 32, 12);
            height = 8;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class ChairBack : LevelObject
    {
        public ChairBack(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.chairBack;
            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 17);
            footprint = new Rectangle((int)position.X + 5, (int)position.Y + 0, 16, 17);
            height = 5;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);

        }
    }

    public class ChairFront : LevelObject
    {
        public ChairFront(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.chairFront;
            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 17);
            footprint = new Rectangle((int)position.X + 5, (int)position.Y + 0, 16, 17);
            height = 5;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);

        }
    }

    public class Chest : LevelObject
    {
        public Chest(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.chest;
            variation = Game1.rnd.Next(4);
            sourceRectangle = new Rectangle(32 * variation, 0, 32, 17);
            footprint = new Rectangle((int)position.X + 4, (int)position.Y + 10, 25, 7);
            height = 5;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class HumanVialsEmpty : LevelObject
    {
        public HumanVialsEmpty(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.humanVialsEmpty;
            variation = Game1.rnd.Next(1);
            sourceRectangle = new Rectangle(32 * variation, 0, 32, 42);
            footprint = new Rectangle((int)position.X + 6, (int)position.Y + 25, 21, 13);
            height = 10;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class HumanVialsFilled : LevelObject
    {
        public HumanVialsFilled(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.humanVialsFilled;
            variation = Game1.rnd.Next(4);
            sourceRectangle = new Rectangle(32 * variation, 0, 32, 42);
            footprint = new Rectangle((int)position.X + 6, (int)position.Y + 25, 21, 13);
            height = 10;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class HumanVialsNoBody : LevelObject
    {
        public HumanVialsNoBody(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.humanVialsNoBody;
            variation = Game1.rnd.Next(4);
            sourceRectangle = new Rectangle(32 * variation, 0, 32, 42);
            footprint = new Rectangle((int)position.X + 6, (int)position.Y + 25, 21, 13);
            height = 10;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class RadioactiveStain : LevelObject
    {
        public RadioactiveStain(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.radioactiveStain;
            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(32 * variation, 0, 64, 11);
            height = 1;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);

        }
    }
    public class WaterStain : LevelObject
    {
        public WaterStain(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.waterStain;
            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(32 * variation, 0, 64, 17);
            height = 1;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class TrashCan : LevelObject
    {
        public TrashCan(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.trashCan;
            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 13);
            footprint = new Rectangle((int)position.X + 4, (int)position.Y + 7, 9, 6);
            height = 4;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class Workstation : LevelObject
    {
        public Workstation(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.workstation;
            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(32 * variation, 0, 32, 17);
            footprint = new Rectangle((int)position.X + 5, (int)position.Y + 6, 25, 11);
            height = 5;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class ComputerFront : LevelObject
    {
        public ComputerFront(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.computerFront;
            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(32 * variation, 0, 32, 27);
            footprint = new Rectangle((int)position.X + 5, (int)position.Y + 16, 24, 9);
            height = 8;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);

        }
    }

    public class ComputerBack : LevelObject
    {
        public ComputerBack(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.computerBack;
            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(32 * variation, 0, 32, 27);
            footprint = new Rectangle((int)position.X + 5, (int)position.Y + 16, 24, 9);
            height = 8;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class BigMonitor : LevelObject
    {
        public BigMonitor(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.bigMonitor;
            variation = Game1.rnd.Next(2);
            sourceRectangle = new Rectangle(32 * variation, 0, 32, 27);
            footprint = new Rectangle((int)position.X + 3, (int)position.Y + 22, 28, 4);
            height = 10;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class SmallLocker : LevelObject
    {
        public SmallLocker(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.smallLocker;
            variation = Game1.rnd.Next(3);
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 33);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 20, 14, 13);
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);
        }
    }

    public class ShootingRangeTarget : LevelObject
    {
        public ShootingRangeTarget(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TextureManager.shooting_range_target;
            variation = Game1.rnd.Next(4);
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 40);
            footprint = new Rectangle((int)position.X + 0, (int)position.Y + 36, 16, 4);
            height = 12;
            hitBox = new Rectangle(footprint.X, footprint.Y - 8, footprint.Width, footprint.Height);

        }
    }
}
