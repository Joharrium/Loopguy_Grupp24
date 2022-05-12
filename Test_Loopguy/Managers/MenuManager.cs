using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test_Loopguy
{
    static class MenuManager
    {
        static Menu main, settings;
        static Menu currentMenu;

        public static void Init()
        {
            main = new MainMenu();
            settings = new SettingsMenu();
            settings.LoadMenuButtons();
            main.LoadMenuButtons();
            currentMenu = main;
        }

        public static void GoToMainMenu()
        {
            currentMenu = main;
        }

        public static void GoToSettingsMenu()
        {
            currentMenu = settings;
        }

        public static void Update(GameTime gameTime)
        {
            currentMenu.Update(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            currentMenu.Draw(spriteBatch);
        }
    }
}
