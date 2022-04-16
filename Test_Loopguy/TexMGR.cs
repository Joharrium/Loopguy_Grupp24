using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    static class TexMGR
    {
        public static Texture2D notex, playerSheet, target, blueArc, redPixel, cyanPixel, gunSheet, testTile, testAlt, box, checkers;
        public static Texture2D meleeFx;
        // terrain files
        public static Texture2D grassBasic, grassAlt, grayBrickWall, dirt, tiles_checkered_gray, tiles_checkered_brown, tiles_big_light, tiles_big_dark;
        // object files
        public static Texture2D boxOpen, barrel, pot, shrub_big, shrub_small, tree_big, tree_small, fernDestroyed, door, door_open, keycard;
        // ui files
        public static Texture2D UI_dirt, UI_grass, UI_selectedMenuBox, UI_graybrick, black_screen;
        public static SpriteFont UI_menuFont;

        public static void LoadTextures(ContentManager c)
        {
            LoadTerrain(c);
            LoadObjects(c);
            LoadUI(c);

            notex = c.Load<Texture2D>("notex");
            checkers = c.Load<Texture2D>("checkers");
            playerSheet = c.Load<Texture2D>("Loopy3");
            gunSheet = c.Load<Texture2D>("guns");
            target = c.Load<Texture2D>("target");
            blueArc = c.Load<Texture2D>("blueArc");
            redPixel = c.Load<Texture2D>("redPixel");
            cyanPixel = c.Load<Texture2D>("cyanPixel");
            testTile = c.Load<Texture2D>("testtile");
            testAlt = c.Load<Texture2D>("testtilealt");
            meleeFx = c.Load<Texture2D>("meleefx");
        }

        private static void LoadTerrain(ContentManager c)
        {
            dirt = c.Load<Texture2D>("gfx/terrain/dirt");
            grassBasic = c.Load<Texture2D>("gfx/terrain/grass_basic");
            grassAlt = c.Load<Texture2D>("gfx/terrain/grass_variation");
            grayBrickWall = c.Load<Texture2D>("gfx/terrain/wall_small");
            tiles_checkered_brown = c.Load<Texture2D>("gfx/terrain/tiles_checkered_brown");
            tiles_checkered_gray = c.Load<Texture2D>("gfx/terrain/tiles_checkered_gray");
            tiles_big_light = c.Load<Texture2D>("gfx/terrain/tiles_big_light");
            tiles_big_dark = c.Load<Texture2D>("gfx/terrain/tiles_big_dark");
        }

        private static void LoadObjects(ContentManager c)
        {
            box = c.Load<Texture2D>("gfx/objects/box");
            boxOpen = c.Load<Texture2D>("gfx/objects/box_open");
            barrel = c.Load<Texture2D>("gfx/objects/barrel");
            pot = c.Load<Texture2D>("gfx/objects/pot");
            shrub_big = c.Load<Texture2D>("gfx/objects/shrub");
            shrub_small = c.Load<Texture2D>("gfx/objects/shrub_small");
            tree_big = c.Load<Texture2D>("gfx/objects/tree_big");
            tree_small = c.Load<Texture2D>("gfx/objects/tree_small");
            fernDestroyed = c.Load<Texture2D>("gfx/objects/fern_destructible");
            door = c.Load<Texture2D>("gfx/objects/door_big");
            door_open = c.Load<Texture2D>("gfx/objects/door_big_open");
            keycard = c.Load<Texture2D>("gfx/objects/keycard");
        }

        private static void LoadUI(ContentManager c)
        {
            UI_grass = c.Load<Texture2D>("gfx/interface/editor_icons/grass_small");
            UI_dirt = c.Load<Texture2D>("gfx/interface/editor_icons/dirt_small");
            UI_selectedMenuBox = c.Load<Texture2D>("gfx/interface/menu_items/selectedMenuBox");
            UI_menuFont = c.Load<SpriteFont>("gfx/fonts/menuFont");
            UI_graybrick = c.Load<Texture2D>("gfx/interface/editor_icons/graybrick_small");
            black_screen = c.Load<Texture2D>("gfx/interface/black_screenlol");
        }
    }
}
