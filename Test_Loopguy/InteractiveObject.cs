using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    public class Destructible : LevelObject
    {
        protected int health;
        //for objects that would leave something behind e.g. a tree would leave a stump behind.
        protected LevelObject spawnObject;
        internal AnimSprite animation;
        internal bool destroyed = false;
        internal bool actuallyDestroyed = false;

        public Destructible(Vector2 position) : base(position)
        {
            this.position = position;
            
        }

        public void Damage(int damage)
        {
            health -= damage;
        }

        public void Update(GameTime gameTime)
        {
            if(health <= 0)
            {
                position = new Vector2(-99999, -99999);
                hitBox.X = -100000;
                destroyed = true;
                animation.PlayOnce(0, 200, 70);
                if(animation.currentFrame.X > 10)
                {
                    
                    animation.Position = new Vector2(-10000, -10000);
                    actuallyDestroyed = true;
                }
                
            }
            animation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if(destroyed)
            {
                animation.Draw(spriteBatch);
            }
            
        }


    }

    public class ShrubSmall : Destructible
    {
        public ShrubSmall(Vector2 position) : base(position)
        {
            animation = new AnimSprite(TexMGR.fernDestroyed, new Point(24,24));
            animation.Position = position - new Vector2(4, 4);
            this.position = position;
            health = 1;
            variation = Game1.rnd.Next(4);
            texture = TexMGR.shrub_small;
            hitBox.Width = 16;
            hitBox.Height = 16;
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 16);

        }
    }

    

    public class Switch : LevelObject
    {
        bool state;
        public Switch(Vector2 position) : base(position)
        {
            this.position = position;
        }
    }

    
}
