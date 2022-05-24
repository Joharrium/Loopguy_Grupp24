using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    static class TextureManager
    {
        public static Texture2D notex, playerSheet, target, blueArc, redPixel, cyanPixel, pistolSheet, railgunSheet, testTile, testAlt, box;
        public static Texture2D meleeFx, shot, robotEnemyShot, blueDot, cursor, dashCloud, railgunBeam, railgunImpactSheet, railgun;
        public static Texture2D androidEnemySheet, evilMeleeFx, evilPistolSheet, evilShot, greenPixel, greenDot;
        // terrain files
        public static Texture2D grassBasic, grassAlt, grayBrickWall, dirt, tiles_checkered_gray, tiles_checkered_brown, tiles_big_light, tiles_big_dark,
            carpet_worn, tile_carpet, tile_warning, wall_metal, wall_worn, tile_metal_bright, grass_edge, tile_clinical, water, wall_beige, wall_gray, wall_brick_beige;
        // object files
        public static Texture2D boxOpen, barrel, pot, shrub_big, shrub_small, tree_big, tree_small, fernDestroyed, door, door_open,
            keycard, door_sliding, door_sliding_open, barrelDestroyed, medkit, console, ammo,
            billboard, cabinet, cardboard_box_stack_small, counter, desk_monitors, machine_heartbeat_thing, reception_desk, shelf_archiving,
            shelf_archiving_small, table_computer_medicine, table_operation, monitor_wall, desk_office, chair_office_fw, chair_office_bw, server,
            big_sink, sink, carrying_thing, crate_stack, nice_bookshelf, shooting_range_bench, shooting_range_target, sofa, camera, desk_bw, desk_fw, 
            morgue, potted_plant, shelf_weird, locker, whiteboard, canteen_chair_left, canteen_chair_right, sofa_left, sofa_right, canteen_table, canteen_food_thing,
            kitchen_counter, bench,  bigScreenTele, carsLeft, carsRight, smallCarLeft, smallCarRight, chairBack, chairFront, chest, copCarLeft, copCarRight, humanVialsEmpty,
            humanVialsFilled, humanVialsNoBody, normalScreenTele, radioactiveStain, trashCan, waterStain, computerBack, computerFront, bigMonitor, workstation,
            smallLocker, explosionSheet, wallGlass, nuclearBomb;
        // ui files
        public static Texture2D UI_dirt, UI_grass, UI_selectedMenuBox, UI_graybrick, black_screen, UI_door,
            healthbar_small_outline, healthbar_small_fill_bg, healthbar_small_fill,
            player_healthbar_outline, player_healthbar_outline_alt, player_healthbar_inline, player_healthbar_fill, player_healthbar_fill_bg, player_healthbar_fill_bg_alt,
            ammobar_fill, ammobar_outline, ammobar_fill_bg, menu_bg, slider_container, slider_fill, logo,
            checkbox_true, checkbox_false, control_atlas, white_screen
            
            ;
        public static SpriteFont UI_menuFont, UI_menuFont2, smallestFont;
        // character files
        public static Texture2D enemyPlaceholder, playerCharacterForMenu, robotEnemySheet, blankbig, blanksmall, smallFastEnemySheet;

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
            playerSheet = c.Load<Texture2D>("Loopy3");
            pistolSheet = c.Load<Texture2D>("pistol");
            railgunSheet = c.Load<Texture2D>("railgun");
            target = c.Load<Texture2D>("target");
            blueArc = c.Load<Texture2D>("blueArc");
            redPixel = c.Load<Texture2D>("redPixel");
            cyanPixel = c.Load<Texture2D>("cyanPixel");
            testTile = c.Load<Texture2D>("testtile");
            testAlt = c.Load<Texture2D>("testtilealt");
            meleeFx = c.Load<Texture2D>("meleefx");
            explosionSheet = c.Load<Texture2D>("explosionV2");
            shot = c.Load<Texture2D>("shot");
            robotEnemyShot = c.Load<Texture2D>("projektilSpriteSheetRobotEnemyVit");
            blueDot = c.Load<Texture2D>("bluedot");
            cursor = c.Load<Texture2D>("cursor");
            dashCloud = c.Load<Texture2D>("dashCloud");
            railgunBeam = c.Load<Texture2D>("railgunBeam");
            railgunImpactSheet = c.Load<Texture2D>("railgunImpact");
            railgun = c.Load<Texture2D>("railgunPickup");
            androidEnemySheet = c.Load<Texture2D>("evilloopy");
            evilMeleeFx = c.Load<Texture2D>("evilmeleefx");
            evilPistolSheet = c.Load<Texture2D>("evilpistol");
            evilShot = c.Load<Texture2D>("evilshot");
            greenPixel = c.Load<Texture2D>("greenpixel");
            greenDot = c.Load<Texture2D>("greendot");
        }

        private static void LoadTerrain(ContentManager c)
        {
            wallGlass = c.Load<Texture2D>("gfx/terrain/glassWallSlimmed");
            dirt = c.Load<Texture2D>("gfx/terrain/dirt");
            grassBasic = c.Load<Texture2D>("gfx/terrain/grass_basic");
            grassAlt = c.Load<Texture2D>("gfx/terrain/grass_variation");
            grayBrickWall = c.Load<Texture2D>("gfx/terrain/wall_small");
            tiles_checkered_brown = c.Load<Texture2D>("gfx/terrain/tiles_checkered_brown");
            tiles_checkered_gray = c.Load<Texture2D>("gfx/terrain/tiles_checkered_gray");
            tiles_big_light = c.Load<Texture2D>("gfx/terrain/tiles_big_light");
            tiles_big_dark = c.Load<Texture2D>("gfx/terrain/tiles_big_dark");

            carpet_worn = c.Load<Texture2D>("gfx/terrain/carpet_run_down");
            tile_carpet = c.Load<Texture2D>("gfx/terrain/tile_carpet");
            tile_warning = c.Load<Texture2D>("gfx/terrain/tile_clinical_warning");
            wall_metal = c.Load<Texture2D>("gfx/terrain/wall_metal");
            wall_worn = c.Load<Texture2D>("gfx/terrain/wall_run_down");
            tile_metal_bright = c.Load<Texture2D>("gfx/terrain/metal_tile_bright");

            grass_edge = c.Load<Texture2D>("gfx/terrain/grass_edges");
            tile_clinical = c.Load<Texture2D>("gfx/terrain/tile_clinical");
            water = c.Load<Texture2D>("gfx/terrain/water");
            wall_beige = c.Load<Texture2D>("gfx/terrain/wall_beige");
            wall_gray = c.Load<Texture2D>("gfx/terrain/wall_gray");
            wall_brick_beige = c.Load<Texture2D>("gfx/terrain/wall_brick_beige");
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
            medkit = c.Load<Texture2D>("gfx/objects/healthPackNewest");
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

            {
                bench = c.Load<Texture2D>("gfx/objects/assetpack/bench");
                canteen_chair_left = c.Load<Texture2D>("gfx/objects/assetpack/canteen_chair_left");
                canteen_chair_right = c.Load<Texture2D>("gfx/objects/assetpack/canteen_chair_right");
                sofa_left = c.Load<Texture2D>("gfx/objects/assetpack/sofa_left");
                sofa_right = c.Load<Texture2D>("gfx/objects/assetpack/sofa_right");
                canteen_table = c.Load<Texture2D>("gfx/objects/assetpack/canteen_table");
                canteen_food_thing = c.Load<Texture2D>("gfx/objects/assetpack/canteen_food_thing");
                kitchen_counter = c.Load<Texture2D>("gfx/objects/assetpack/kitchen_counter");
            }

            {
                big_sink = c.Load<Texture2D>("gfx/objects/assetpack/big_sink");
                sink = c.Load<Texture2D>("gfx/objects/assetpack/sink");
                carrying_thing = c.Load<Texture2D>("gfx/objects/assetpack/carrying_thing");
                crate_stack = c.Load<Texture2D>("gfx/objects/assetpack/crate_stack");
                nice_bookshelf = c.Load<Texture2D>("gfx/objects/assetpack/nice_bookshelf");
                shooting_range_bench = c.Load<Texture2D>("gfx/objects/assetpack/shooting_range_bench");
                shooting_range_target = c.Load<Texture2D>("gfx/objects/assetpack/shooting_range_target");
                sofa = c.Load<Texture2D>("gfx/objects/assetpack/sofa");
                camera = c.Load<Texture2D>("gfx/objects/camera");
                desk_bw = c.Load<Texture2D>("gfx/objects/desk_backward");
                desk_fw = c.Load<Texture2D>("gfx/objects/desk_forward");
                morgue = c.Load<Texture2D>("gfx/objects/morgue");
                potted_plant = c.Load<Texture2D>("gfx/objects/potted_plant");
                shelf_weird = c.Load<Texture2D>("gfx/objects/shelf_weird");
                locker = c.Load<Texture2D>("gfx/objects/locker");
                whiteboard = c.Load<Texture2D>("gfx/objects/whiteboard");
            }
            {
                bigScreenTele = c.Load<Texture2D>("gfx/objects/bigScreenTele");
                carsLeft = c.Load<Texture2D>("gfx/objects/carsleft");
                carsRight = c.Load<Texture2D>("gfx/objects/carsRight");
                copCarLeft = c.Load<Texture2D>("gfx/objects/copCarLeft");
                copCarRight = c.Load<Texture2D>("gfx/objects/copCarRight");
                chairBack = c.Load<Texture2D>("gfx/objects/chairBack");
                chairFront = c.Load<Texture2D>("gfx/objects/chairFront");
                chest = c.Load<Texture2D>("gfx/objects/chest");
                humanVialsEmpty = c.Load<Texture2D>("gfx/objects/humanVialsEmpty");
                humanVialsFilled = c.Load<Texture2D>("gfx/objects/humanVialsFilled");
                humanVialsNoBody = c.Load<Texture2D>("gfx/objects/humanVialsNoBody");
                normalScreenTele = c.Load<Texture2D>("gfx/objects/normalScreenTele");
                radioactiveStain = c.Load<Texture2D>("gfx/objects/radioactiveStain");
                trashCan = c.Load<Texture2D>("gfx/objects/trashCan");
                waterStain = c.Load<Texture2D>("gfx/objects/waterStain");
                smallCarLeft = c.Load<Texture2D>("gfx/objects/smallCarLeft");
                smallCarRight = c.Load<Texture2D>("gfx/objects/smallCarRight");
                workstation = c.Load<Texture2D>("gfx/objects/workstation");
                computerBack = c.Load<Texture2D>("gfx/objects/computerBack");
                computerFront = c.Load<Texture2D>("gfx/objects/computerFront");
                bigMonitor = c.Load<Texture2D>("gfx/objects/bigMonitor");
                smallLocker = c.Load<Texture2D>("gfx/objects/smallLocker");
            }
            nuclearBomb = c.Load<Texture2D>("gfx/objects/nuclearBomb");
        }

        private static void LoadUI(ContentManager c)
        {
            UI_grass = c.Load<Texture2D>("gfx/interface/editor_icons/grass_small");
            UI_dirt = c.Load<Texture2D>("gfx/interface/editor_icons/dirt_small");
            UI_selectedMenuBox = c.Load<Texture2D>("gfx/interface/menu_items/selectedMenuBox");
            UI_menuFont = c.Load<SpriteFont>("gfx/fonts/menuFont");
            UI_menuFont2 = c.Load<SpriteFont>("MenuFont2");
            smallestFont = c.Load<SpriteFont>("smallestFont");
            UI_graybrick = c.Load<Texture2D>("gfx/interface/editor_icons/graybrick_small");
            black_screen = c.Load<Texture2D>("gfx/interface/black_screenlol");
            white_screen = c.Load<Texture2D>("gfx/interface/white_screenlol");
            UI_door = c.Load<Texture2D>("gfx/interface/editor_icons/door_small");
            healthbar_small_fill = c.Load<Texture2D>("gfx/interface/healthbar_small_fill");
            healthbar_small_fill_bg = c.Load<Texture2D>("gfx/interface/healthbar_small_fill_bg");
            healthbar_small_outline = c.Load<Texture2D>("gfx/interface/healthbar_small_outline");

            player_healthbar_outline = c.Load<Texture2D>("gfx/interface/player_healthbar_outline");
            player_healthbar_outline_alt = c.Load<Texture2D>("gfx/interface/player_healthbar_outline_alt");
            player_healthbar_inline = c.Load<Texture2D>("gfx/interface/player_healthbar_inline");
            player_healthbar_fill = c.Load<Texture2D>("gfx/interface/player_healthbar_fill");
            player_healthbar_fill_bg = c.Load<Texture2D>("gfx/interface/player_healthbar_fill_bg");
            player_healthbar_fill_bg_alt = c.Load<Texture2D>("gfx/interface/player_healthbar_fill_bg_alt");

            ammobar_fill = c.Load<Texture2D>("gfx/interface/ammobar_fill");
            ammobar_outline = c.Load<Texture2D>("gfx/interface/ammobar_outline");
            ammobar_fill_bg = c.Load<Texture2D>("gfx/interface/ammobar_fill_bg");
            menu_bg = c.Load<Texture2D>("gfx/interface/menu_bg2");
            control_atlas = c.Load<Texture2D>("gfx/interface/control_atlas");

            slider_container = c.Load<Texture2D>("gfx/interface/menu_items/slider_container");
            slider_fill = c.Load<Texture2D>("gfx/interface/menu_items/slider_fill");

            logo = c.Load<Texture2D>("gfx/interface/menu_items/logo");
            checkbox_false = c.Load<Texture2D>("gfx/interface/menu_items/checkbox_false");
            checkbox_true = c.Load<Texture2D>("gfx/interface/menu_items/checkbox_true");
        }

        private static void LoadCharacters(ContentManager c)
        {
            enemyPlaceholder = c.Load<Texture2D>("gfx/characters/enemy_placeholder");
            playerCharacterForMenu = c.Load<Texture2D>("gfx/interface/menu_items/guy");
            robotEnemySheet = c.Load<Texture2D>("uppdateradRobotEnemySSheetFinal");
            blankbig = c.Load<Texture2D>("gfx/characters/64x64blank");
            blanksmall = c.Load<Texture2D>("gfx/characters/32x32blank");
            smallFastEnemySheet = c.Load<Texture2D>(@"weakEnemyUpdated");
        }
        
        private static void LoadParticles(ContentManager c)
        {
            spark_small = c.Load<Texture2D>("gfx/particles/spark_small");
            shot_explosion = c.Load<Texture2D>("gfx/particles/shot_explosion");
            heal_effect = c.Load<Texture2D>("gfx/particles/heal_effect");
        }
    }
}
