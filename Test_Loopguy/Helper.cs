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


        public static double GetAngle(Vector2 source, Vector2 target)
        {
            double v = Math.Atan2(target.X - source.X, target.Y - source.Y);

            v -= Math.PI / 2;

            if (v < 0.0)
                v += Math.PI * 2;

            return v;
        }
    }
}
