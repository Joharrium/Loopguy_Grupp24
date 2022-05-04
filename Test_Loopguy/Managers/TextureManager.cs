using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    static class TextureManager
    {
        public static Texture2D notex, playerSheet, target, blueArc, redPixel, cyanPixel, gunSheet, testTile, testAlt, box, checkers;
        public static Texture2D meleeFx, shot, blueDot, cursor;
        // terrain files
        public static Texture2D grassBasic, grassAlt, grayBrickWall, dirt, tiles_checkered_gray, tiles_checkered_brown, tiles_big_light, tiles_big_dark,
            carpet_worn, tile_metal, tile_metal_copper, wall_metal, wall_worn, tile_metal_bright, grass_edge, tile_clinical;
        // object files
        public static Texture2D boxOpen, barrel, pot, shrub_big, shrub_small, tree_big, tree_small, fernDestroyed, door, door_open,
            keycard, door_sliding, door_sliding_open, barrelDestroyed, medkit, console, ammo,
            billboard, cabinet, cardboard_box_stack_small, counter, desk_monitors, machine_heartbeat_thing, reception_desk, shelf_archiving,
            shelf_archiving_small, table_computer_medicine, table_operation, monitor_wall, desk_office, chair_office_fw, chair_office_bw, server
            ;
        // ui files
        public static Texture2D UI_dirt, UI_grass, UI_selectedMenuBox, UI_graybrick, black_screen, UI_door, 
            healthbar_small_outline, healthbar_small_fill_bg, healthbar_small_fill,
            player_healthbar_outline, player_healthbar_inline, player_healthbar_fill, player_healthbar_fill_bg,
            ammobar_fill, ammobar_outline, ammobar_fill_bg;
        public static SpriteFont UI_menuFont;
        // character files
        public static Texture2D enemyPlaceholder, playerCharacterForMenu;

        //particles
        public static Texture2D spark_small, shot_explosion, heal_effect;

        public static void LoadTextures(ContentManager c)
        {
            LoadTerrain(c);
            LoadObjects(c);
            LoadUI(c);
            LoadParticles(c);
            LoadCharacters(c);

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
            shot = c.Load<Texture2D>("shot");
            blueDot = c.Load<Texture2D>("bluedot");
            cursor = c.Load<Texture2D>("cursor");
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

            carpet_worn = c.Load<Texture2D>("gfx/terrain/carpet_run_down");
            tile_metal = c.Load<Texture2D>("gfx/terrain/tile_metal");
            tile_metal_copper = c.Load<Texture2D>("gfx/terrain/tile_metal_weathered");
            wall_metal = c.Load<Texture2D>("gfx/terrain/wall_metal");
            wall_worn = c.Load<Texture2D>("gfx/terrain/wall_run_down");
            tile_metal_bright = c.Load<Texture2D>("gfx/terrain/metal_tile_bright");

            grass_edge = c.Load<Texture2D>("gfx/terrain/grass_edges");
            tile_clinical = c.Load<Texture2D>("gfx/terrain/tile_clinical");
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
            door_sliding = c.Load<Texture2D>("gfx/objects/door_sliding");
            door_sliding_open = c.Load<Texture2D>("gfx/objects/door_sliding_open");
            barrelDestroyed = c.Load<Texture2D>("gfx/objects/barrel_destructible");
            medkit = c.Load<Texture2D>("gfx/objects/medkit");
            console = c.Load<Texture2D>("gfx/objects/console_thing");
            ammo = c.Load<Texture2D>("gfx/objects/ammo");

            {
                billboard = c.Load<Texture2D>("gfx/objects/assetpack/billboard");
                cabinet = c.Load<Texture2D>("gfx/objects/assetpack/cabinet");
                cardboard_box_stack_small = c.Load<Texture2D>("gfx/objects/assetpack/cardboard_box_stack_small");
                counter = c.Load<Texture2D>("gfx/objects/assetpack/counter");
                desk_monitors = c.Load<Texture2D>("gfx/objects/assetpack/desk_monitors");
                machine_heartbeat_thing = c.Load<Texture2D>("gfx/objects/assetpack/machine_heartbeat_thing");
                reception_desk = c.Load<Texture2D>("gfx/objects/assetpack/reception_desk");
                shelf_archiving = c.Load<Texture2D>("gfx/objects/assetpack/shelf_archiving");
                shelf_archiving_small = c.Load<Texture2D>("gfx/objects/assetpack/shelf_archiving_small");
                table_computer_medicine = c.Load<Texture2D>("gfx/objects/assetpack/table_computer_medicine");
                table_operation = c.Load<Texture2D>("gfx/objects/assetpack/table_operation");
                server = c.Load<Texture2D>("gfx/objects/assetpack/server");
                monitor_wall = c.Load<Texture2D>("gfx/objects/assetpack/monitor_wall");
                desk_office = c.Load<Texture2D>("gfx/objects/assetpack/desk_office");
                chair_office_bw = c.Load<Texture2D>("gfx/objects/assetpack/chair_office_backwards");
                chair_office_fw = c.Load<Texture2D>("gfx/objects/assetpack/chair_office_forward");


            }

        }

        private static void LoadUI(ContentManager c)
        {
            UI_grass = c.Load<Texture2D>("gfx/interface/editor_icons/grass_small");
            UI_dirt = c.Load<Texture2D>("gfx/interface/editor_icons/dirt_small");
            UI_selectedMenuBox = c.Load<Texture2D>("gfx/interface/menu_items/selectedMenuBox");
            UI_menuFont = c.Load<SpriteFont>("gfx/fonts/menuFont");
            UI_graybrick = c.Load<Texture2D>("gfx/interface/editor_icons/graybrick_small");
            black_screen = c.Load<Texture2D>("gfx/interface/black_screenlol");
            UI_door = c.Load<Texture2D>("gfx/interface/editor_icons/door_small");
            healthbar_small_fill = c.Load<Texture2D>("gfx/interface/healthbar_small_fill");
            healthbar_small_fill_bg = c.Load<Texture2D>("gfx/interface/healthbar_small_fill_bg");
            healthbar_small_outline = c.Load<Texture2D>("gfx/interface/healthbar_small_outline");

            player_healthbar_outline = c.Load<Texture2D>("gfx/interface/player_healthbar_outline");
            player_healthbar_inline = c.Load<Texture2D>("gfx/interface/player_healthbar_inline");
            player_healthbar_fill = c.Load<Texture2D>("gfx/interface/player_healthbar_fill");
            player_healthbar_fill_bg = c.Load<Texture2D>("gfx/interface/player_healthbar_fill_bg");

            ammobar_fill = c.Load<Texture2D>("gfx/interface/ammobar_fill");
            ammobar_outline = c.Load<Texture2D>("gfx/interface/ammobar_outline");
            ammobar_fill_bg = c.Load<Texture2D>("gfx/interface/ammobar_fill_bg");
        }

        private static void LoadCharacters(ContentManager c)
        {
            enemyPlaceholder = c.Load<Texture2D>("gfx/characters/enemy_placeholder");
            playerCharacterForMenu = c.Load<Texture2D>("LOOP_FELLA");
        }
        
        private static void LoadParticles(ContentManager c)
        {
            spark_small = c.Load<Texture2D>("gfx/particles/spark_small");
            shot_explosion = c.Load<Texture2D>("gfx/particles/shot_explosion");
            heal_effect = c.Load<Texture2D>("gfx/particles/heal_effect");
        }
    }
}
