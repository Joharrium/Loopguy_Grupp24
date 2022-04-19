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
        internal AnimSprite animation;
        bool repeating;
        bool donePlaying = false;

        public Particle(Vector2 position) : base(position)
        {
            this.position = position;
            
        }

        public void Update()
        {
            if(repeating)
            {
                animation.Play(0, 1000, 60);
            }
            else if (!repeating)
            {
                if (animation.PlayOnce(0, 1000, 60))
                {
                    position = new Vector2(-1000, -1000);
                    donePlaying = true;
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
            animation = new AnimSprite(TexMGR.spark_small, new Point(16, 16));
        }
    }
}
