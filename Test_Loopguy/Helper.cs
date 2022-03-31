using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Test_Loopguy
{
    static class Helper
    {

        public static double GetAngle(Vector2 source, Vector2 target, double offset)
        {
            double v = Math.Atan2(target.X - source.X, target.Y - source.Y);

            //No adjustment measures the angle from below the source (if that makes any sense at all) otherwise adjust below
            v += offset;

            if (v < 0.0)
                v += Math.PI * 2;

            return v;
        }

        public static Color RandomTransparency(Random rnd, int minAlpha, int maxAlpha)
        {
            //Below doesn't work for some reason

            //float rndAlpha = (float)(0.01 + rnd.NextDouble() * 0.33);
            //return new Color(Color.White, rndAlpha);

            //So I have to do this stupid thing

            int rndInt = rnd.Next(minAlpha, maxAlpha);
            return new Color(rndInt, rndInt, rndInt, rndInt);
        }
    }
}
