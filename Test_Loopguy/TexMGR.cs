﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    static class TexMGR
    {
        public static Texture2D notex, playerSheet, target, blueArc, redPixel, cyanPixel, testTile, testAlt, box;
        // terrain files
        public static Texture2D grassBasic, grassAlt;
        // object files
        public static Texture2D boxOpen, barrel, pot, shrub, shrub_small, tree_big, tree_small;

        public static void LoadTextures(ContentManager c)
        {
            LoadTerrain(c);
            LoadObjects(c);

            notex = c.Load<Texture2D>("notex");
            playerSheet = c.Load<Texture2D>("Loopy");
            target = c.Load<Texture2D>("target");
            blueArc = c.Load<Texture2D>("blueArc");
            redPixel = c.Load<Texture2D>("redPixel");
            cyanPixel = c.Load<Texture2D>("cyanPixel");
            testTile = c.Load<Texture2D>("testtile");
            testAlt = c.Load<Texture2D>("testtilealt");
            box = c.Load<Texture2D>("boxe");
        }

        private static void LoadTerrain(ContentManager c)
        {
            grassBasic = c.Load<Texture2D>("gfx/terrain/grass_basic");
            grassAlt = c.Load<Texture2D>("gfx/terrain/grass_variation");
        }

        private static void LoadObjects(ContentManager c)
        {
            box = c.Load<Texture2D>("gfx/objects/box");
            boxOpen = c.Load<Texture2D>("gfx/objects/box_open");
            barrel = c.Load<Texture2D>("gfx/objects/barrel");
            pot = c.Load<Texture2D>("gfx/objects/pot");
            shrub = c.Load<Texture2D>("gfx/objects/shrub");
            shrub_small = c.Load<Texture2D>("gfx/objects/shrub_small");
            tree_big = c.Load<Texture2D>("gfx/objects/tree_big");
            tree_small = c.Load<Texture2D>("gfx/objects/tree_small");
        }
    }
}
