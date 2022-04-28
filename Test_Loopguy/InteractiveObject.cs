using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    public class Destructible : LevelObject
    {
        protected int health;
        protected int maxHealth;
        //for objects that would leave something behind e.g. a tree would leave a stump behind.
        protected LevelObject spawnObject;
        internal AnimatedSprite animation;
        protected HealthBar healthBar;
        internal bool destroyed = false;
        internal bool actuallyDestroyed = false;
        public bool hitDuringCurrentAttack = false;
        protected SoundEffect hitSound;
        private float wave = 0;
        private float waveadjust = 0;
        private bool wobbling = false;

        public Destructible(Vector2 position) : base(position)
        {
            this.position = position;
            
        }

        public void Damage(int damage)
        {
            if(!hitDuringCurrentAttack)
            {
                
                health -= damage;
                wobbling = true;

                if(hitSound != null)
                {
                    Audio.PlaySound(hitSound);
                }
            }
            
        }

        private void Wobble()
        {
            wave += 0.5F;
            waveadjust = (float)(Math.Sin(wave)) / 2;
            position.X += waveadjust;
            if(wave % 5.5 == 0)
            {
                wobbling = false;
            }
        }

        public void Update(GameTime gameTime)
        {
            if(health < maxHealth)
            {
                healthBar.SetCurrentValue(position + new Vector2(2, hitBox.Height), health);
            }
            
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
            else
            {
                if (wobbling)
                {
                    Wobble();
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
            if (health < maxHealth)
            {
                healthBar.Draw(spriteBatch);
            }
        }


    }

    public class ShrubSmall : Destructible
    {
        public ShrubSmall(Vector2 position) : base(position)
        {
            animation = new AnimatedSprite(TextureManager.fernDestroyed, new Point(24,24));
            animation.Position = position - new Vector2(4, 4);
            this.position = position;
            health = 1;
            maxHealth = 1;
            variation = Game1.rnd.Next(4);
            texture = TextureManager.shrub_small;
            hitBox.Width = 16;
            hitBox.Height = 16;
            sourceRectangle = new Rectangle(16 * variation, 0, 16, 16);
            healthBar = new HealthBar(maxHealth);
            hitSound = Audio.shrub_destroy;

            footprint = new Rectangle((int)position.X + 1, (int)position.Y + 4, 14, 10);
            height = 2;
        }
    }

    public class BarrelDestructible : Destructible
    {
        public BarrelDestructible(Vector2 position) : base(position)
        {
            animation = new AnimatedSprite(TextureManager.barrelDestroyed, new Point(24, 24));
            animation.Position = position - new Vector2(4, 4);
            this.position = position;
            health = 3;
            maxHealth = 3;
            texture = TextureManager.barrel;
            hitBox.Width = 16;
            hitBox.Height = 16;
            sourceRectangle = new Rectangle(0, 0, 16, 16);
            healthBar = new HealthBar(maxHealth);
            hitSound = Audio.box_destroy;

            footprint = new Rectangle((int)position.X, (int)position.Y + 6, 16, 10);
            height = 10;
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
