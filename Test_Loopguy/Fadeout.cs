using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Test_Loopguy
{
    static class Fadeout
    {
        static private double delay = 200;
        static private bool started = false;
        static private bool end = false;
        static private Color fadeout = new Color(255, 255, 255, 0);
        static private Texture2D texture = TextureManager.black_screen;
        static public bool active = false;
        public static void LevelTransitionFade()
        {
            started = true;
            if(fadeout.A > 40)
            {
                fadeout.A = 255;
            }
            else
            {
                fadeout.A = 0;
            }
            
            end = false;
            delay = 200;
            active = true;
        }

        public static void Update(GameTime time)
        {
            if (started)
            {
                
                if(fadeout.A < 255)
                {
                    fadeout.A += 51;
                }
                
                if(fadeout.A == 255)
                {
                    delay -= time.ElapsedGameTime.TotalMilliseconds;
                }
                if (delay < 0)
                {
                    end = true;
                    started = false;
                }
            }

            if (end)
            {
                if (fadeout.A > 0)
                {
                    fadeout.A -= 5;
                }
                if(fadeout.A == 0)
                {
                    end = false;
                    active = false;
                }
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(0,0,150000,150000), fadeout);
        }
    }
}
