using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    public class Door : LevelObject
    {
        public int requiredKey;
        protected bool open = false;
        protected Rectangle unlockArea;
        internal AnimSprite animation;
        protected Texture2D openTex;
        protected bool playerInArea;
        protected SoundEffect openSound;
        protected SoundEffect denySound;
        public Door(Vector2 position, int requiredKey) : base(position)
        {
            this.texture = TexMGR.door;
            openTex = TexMGR.door_open;
            denySound = Audio.meepmerp;
            sourceRectangle = new Rectangle(0, 0, 32, 32);
            this.position = position;
            hitBox = new Rectangle((int)position.X, (int)position.Y, 32, 32);
            this.requiredKey = requiredKey;
            unlockArea = new Rectangle((int)(position.X - 16), (int)(position.Y - 16), 64, 64);
            animation = new AnimSprite(TexMGR.door, new Point(32, 32));
        }

        public void Update(GameTime gameTime)
        {
            if (unlockArea.Contains(EntityManager.player.centerPosition))
            {
                
                
                foreach (int key in EntityManager.player.keys)
                {
                    CheckIfKey(key);
                }
                if (!playerInArea && !open)
                {
                    Audio.PlaySound(denySound);
                }
                playerInArea = true;
                //CheckIfKey(k)

            }
            else
            {
                playerInArea = false;
            }
            if (open)
            {
                if(animation.PlayOnce(0, 64, 180))
                {
                   
                    if(animation.currentFrame.X > 3)
                    {
                        animation.Position = new Vector2(-1000, -1000);
                    }
                    texture = openTex;
                }

                texture = openTex;
            }
            animation.Update(gameTime);

        }

        public void CheckIfKey(int key)
        {
            if (key == requiredKey && !open)
            {
                hitBox.X = 0;
                hitBox.Y = 0;
                hitBox.Width = 0;
                hitBox.Height = 0;
                open = true;
                animation.Position = position;
                
                Audio.PlaySound(openSound);
            }
            else
            if(!playerInArea && !open)
            {
                playerInArea = true;
                
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            animation.Draw(spriteBatch);
        }
    }

    public class DoorSliding : Door
    {
        public DoorSliding(Vector2 position, int requiredKey) : base(position, requiredKey)
        {
            openSound = Audio.door_hiss_sound;
            denySound = Audio.meepmerp;
            this.texture = TexMGR.door_sliding;
            openTex = TexMGR.door_sliding_open;
            sourceRectangle = new Rectangle(0, 0, 32, 32);
            this.position = position;
            hitBox = new Rectangle((int)position.X, (int)position.Y, 32, 32);
            this.requiredKey = requiredKey;
            unlockArea = new Rectangle((int)(position.X - 16), (int)(position.Y - 16), 64, 64);
            animation = new AnimSprite(TexMGR.door_sliding, new Point(32, 32));
        }
    }
}
