﻿using System;
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
        public LevelObject(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TexMGR.box;
            hitBox.X = (int)position.X;
            hitBox.Y = (int)position.Y;
            hitBox.Width = texture.Width;
            hitBox.Height = texture.Height;
            sourceRectangle.Width = 16;
            sourceRectangle.Height = 24;

            centerPosition = new Vector2(position.X - texture.Width / 2, position.Y - texture.Height / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position + new Vector2(hitBox.Width/2, hitBox.Height/2), sourceRectangle, Color.White, rotation, new Vector2(hitBox.Width / 2, hitBox.Height / 2), 1, spriteEffects, 1);
        }
    }

    public class Destructible : LevelObject
    {
        int health;
        public Destructible(Vector2 position) : base(position)
        {
            this.position = position;
        }
    }

    public class Box : LevelObject
    {
        public Box(Vector2 position) : base(position)
        {
            this.position = position;
            texture = TexMGR.box;
        }
    }

    public class BoxOpen : LevelObject
    {
        public BoxOpen(Vector2 position) : base(position)
        {
            this.position = position;
            variation = Game1.rnd.Next(12);
            texture = TexMGR.boxOpen;
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 16);
        }
    }

    public class TreeBig : LevelObject
    {
        public TreeBig(Vector2 position) : base(position)
        {
            this.position = position;
            variation = Game1.rnd.Next(4);
            texture = TexMGR.tree_big;
            hitBox.Width = 48;
            hitBox.Height = 48;
            sourceRectangle = new Rectangle(48 * variation, 0, 48, 48);
        }
    }

    public class ShrubSmall : LevelObject
    {
        public ShrubSmall(Vector2 position) : base(position)
        {
            this.position = position;
            variation = Game1.rnd.Next(4);
            texture = TexMGR.shrub_small;
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 16);
        }
    }
}
