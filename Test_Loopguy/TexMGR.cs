using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    static class TexMGR
    {
        public static Texture2D notex, playerSheet, target, blueArc, redPixel, cyanPixel, checkers, bigcheckers;

        public static void LoadTextures(ContentManager c)
        {
            notex = c.Load<Texture2D>("notex");
            playerSheet = c.Load<Texture2D>("Loopy");
            target = c.Load<Texture2D>("target");
            blueArc = c.Load<Texture2D>("blueArc");
            redPixel = c.Load<Texture2D>("redPixel");
            cyanPixel = c.Load<Texture2D>("cyanPixel");
            checkers = c.Load<Texture2D>("checkers");
            bigcheckers = c.Load<Texture2D>("bigcheckers");
        }
    }
}
