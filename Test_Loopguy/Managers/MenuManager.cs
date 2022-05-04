using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test_Loopguy
{
    class MenuManager
    {

    
        static List<Component> gameComponents;

        public static void LoadMenuButtons()
        {
            float menuSpacing = Game1.windowY / 10;

            Vector2 menuSlot1 = new Vector2(Game1.windowX / 2, Game1.windowY / 2);
            Vector2 menuSlot2 = new Vector2(Game1.windowX / 2, Game1.windowY / 2 + menuSpacing);
            Vector2 menuSlot3 = new Vector2(Game1.windowX / 2, Game1.windowY / 2 + menuSpacing * 2);
            Vector2 menuSlot4 = new Vector2(Game1.windowX / 2, Game1.windowY / 2 + menuSpacing * 3);
            Vector2 menuSlot5 = new Vector2(Game1.windowX / 2, Game1.windowY / 2 + menuSpacing * 4);

            var newGameButton = new Button(TextureManager.UI_selectedMenuBox, TextureManager.UI_menuFont, menuSlot1)
            {
                Position = menuSlot1,
                Text = "Start Game",
            };

            Button loadGameButton = new Button(TextureManager.UI_selectedMenuBox, TextureManager.UI_menuFont, menuSlot2)
            {
                Position = menuSlot2,
                Text = "Load Game",
            };

            var highScoreButton = new Button(TextureManager.UI_selectedMenuBox, TextureManager.UI_menuFont, menuSlot3)
            {
                Position = menuSlot3,
                Text = "High Score",
            };

            var settingsButton = new Button(TextureManager.UI_selectedMenuBox, TextureManager.UI_menuFont, menuSlot4)
            {
                Position = menuSlot4,
                Text = "Settings",
            };

            var quitGameButton = new Button(TextureManager.UI_selectedMenuBox, TextureManager.UI_menuFont, menuSlot5)
            {
                Position = menuSlot5,
                Text = "Quit Game",
            };

            gameComponents = new List<Component>()
            {
                newGameButton,
                loadGameButton,
                highScoreButton,
                settingsButton,
                quitGameButton,
            };

            newGameButton.Click += NewGameButton_Click;
            loadGameButton.Click += LoadGameButton_Click;
            highScoreButton.Click += HighScoreButton_Click;
            settingsButton.Click += SettingsButton_Click;
            quitGameButton.Click += QuitGameButton_Click;

        }

        private static void NewGameButton_Click(object sender, EventArgs e)
        {
            StateManager.currentState = StateManager.GameState.InGame;
        }

        private static void LoadGameButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void HighScoreButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void SettingsButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        //Quits the game, however what I think is due to the win.forms the application doesn't stop at this point but only closes the windows.
        private static void QuitGameButton_Click(object sender, EventArgs e)
        {
            Game1.game1.Exit();
        }

        public static void Update(GameTime gameTime)
        {
            foreach (var component in gameComponents)
            {
                component.Update(gameTime);
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            Vector2 imagePosition = new Vector2(Game1.windowX / 8, Game1.windowY / 3);
            Vector2 titlePosition = new Vector2(Game1.windowX / 2, Game1.windowY / 4);

            spriteBatch.Begin();

            //Draw buttons
            foreach (var component in gameComponents)
            {
                component.Draw(spriteBatch);
            }

            //Draw cursor
            Color cursorColor = new Color(200, 200, 200, 200);
            spriteBatch.Draw(TextureManager.cursor, new Vector2(Game1.mousePos.X - TextureManager.cursor.Width / 2, Game1.mousePos.Y - TextureManager.cursor.Height / 2), cursorColor);

            //Draw title


            //Draw image
            spriteBatch.Draw(TextureManager.playerCharacterForMenu, imagePosition, Color.White);

            spriteBatch.End();
        }
    }
}
