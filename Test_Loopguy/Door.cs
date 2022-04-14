using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    public class Door : LevelObject
    {
        int requiredKey;
        bool open = false;
        Rectangle hitBox;
        Rectangle unlockArea;
        AnimSprite animation;
        public Door(Vector2 position, int requiredKey) : base(position)
        {
            this.position = position;
            hitBox = new Rectangle((int)position.X, (int)position.Y, 32, 32);
            this.requiredKey = requiredKey;
            unlockArea = new Rectangle((int)(position.X - 32), (int)(position.Y - 32), 128, 96);
            animation = new AnimSprite(TexMGR.door, new Point(32, 32));
        }

        public void Update()
        {
            if (unlockArea.Contains(EntityManager.player.centerPosition))
            {
                foreach (int key in EntityManager.player.keys)
                {
                    CheckIfKey(key);
                }
                //CheckIfKey(k)
            }

        }

        public void CheckIfKey(int key)
        {
            if (key == requiredKey && !open)
            {
                open = true;
                animation.PlayOnce(0, 96, 40);
            }
        }
    }
}
