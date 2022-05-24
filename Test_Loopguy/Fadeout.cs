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
        static private Color fadeout = new Color(0, 0, 0, 0);
        static private Texture2D texture = TextureManager.black_screen;
        static public bool active = false;
        static private byte fadeInSpeed;
        static private byte fadeOutSpeed;
        public static void LevelTransitionFade()
        {
            texture = TextureManager.black_screen;
            InputReader.playerInputEnabled = false;
            started = true;
            if(fadeout.A > 40)
            {
                fadeout.A = 255;
            }
            else
            {
                fadeout.A = 0;
            }

            fadeInSpeed = 51;
            fadeOutSpeed = 5;
            end = false;
            delay = 200;
            active = true;
        }

        public static void LoopFade()
        {
            texture = TextureManager.white_screen;
            InputReader.playerInputEnabled = false;
            started = true;
            //if (fadeout.A > 40)
            //{
            //    fadeout.A = 255;
            //}
            //else
            //{
                fadeout.A = 0;
            //}
            fadeInSpeed = 8;
            fadeOutSpeed = 1;
            end = false;
            delay = 1200;
            active = true;

        }

        public static void HazardFade()
        {
            texture = TextureManager.black_screen;
            InputReader.playerInputEnabled = false;
            started = true;
            if (fadeout.A > 40)
            {
                fadeout.A = 255;
            }
            else
            {
                fadeout.A = 0;
            }

            fadeInSpeed = 51;
            fadeOutSpeed = 4;
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
                    if(fadeout.A + fadeInSpeed > 255)
                    {
                        fadeout.A = 255;
                    }
                    else
                    {
                        fadeout.A += fadeInSpeed;
                    }
                    
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
                    if(fadeout.A - fadeOutSpeed < 0)
                    {
                        fadeout.A = 0;
                    }
                    else
                    {
                        fadeout.A -= fadeOutSpeed;
                    }
            
                }
                if(fadeout.A < 240)
                {
                    InputReader.playerInputEnabled = true;
                }
                if(fadeout.A == 0)
                {
                    end = false;
                    active = false;
                    InputReader.playerInputEnabled = true;
                }
            }
        }

        

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(0,0,150000,150000), fadeout);
        }
    }
}
