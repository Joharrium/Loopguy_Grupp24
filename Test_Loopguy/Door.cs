using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    public class Door : LevelObject
    {
        public int requiredKey;
        bool open = false;
        Rectangle unlockArea;
        AnimSprite animation;
        public Door(Vector2 position, int requiredKey) : base(position)
        {
            this.texture = TexMGR.door;
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
                //CheckIfKey(k)
            }
            if (open)
            {
                if(animation.PlayOnce(0, 64, 180))
                {
                   
                    if(animation.currentFrame.X > 3)
                    {
                        animation.Position = new Vector2(-1000, -1000);
                    }
                    texture = TexMGR.door_open;
                }

                texture = TexMGR.door_open;
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
                
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            animation.Draw(spriteBatch);
        }
    }
}
