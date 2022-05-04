using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy.Content
{
    public class Particle : GameObject
    {
        internal AnimatedSprite animation;
        bool repeating;
        public bool donePlaying = false;
        protected int animLength = 4;

        public Particle(Vector2 position) : base(position)
        {
            this.position = position;
            
        }

        public void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
            if(repeating)
            {
                animation.Play(0, 100, 60);
            }
            else if (!repeating)
            {
                if (animation.PlayOnce(0, 100, 60))
                {
                    if(animation.currentFrame.X > animLength)
                    {
                        position = new Vector2(-1000, -1000);
                        donePlaying = true;
                    }
                    
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }

    public class SparkSmall : Particle
    {
        public SparkSmall(Vector2 position) : base(position)
        {
            this.position = position;
            animation = new AnimatedSprite(TextureManager.spark_small, new Point(16, 16));
            animation.size = new Point(16, 16);
            animation.Position = position;
        }
    }

    public class ShotExplosion : Particle
    {
        public ShotExplosion(Vector2 position) : base(position)
        {
            animation = new AnimatedSprite(TextureManager.shot_explosion, new Point(8, 8));
            animation.size = new Point(8, 8);
            animation.Position = position;
            animLength = 3;
        }
    }

    public class HealEffect : Particle
    {
        public HealEffect(Vector2 position) : base(position)
        {
            animation = new AnimatedSprite(TextureManager.heal_effect, new Point(24, 24));
            animation.size = new Point(24, 24);
            animation.Position = position;
            animLength = 3;
        }
    }
}
