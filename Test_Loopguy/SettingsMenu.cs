using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Test_Loopguy
{
    class SettingsMenu : Menu
    {
        static bool usingMouse = false;

        static Button returnButton;
        Slider sound, music, screenSize;
        Checkbox fullscreen;
        //wat
        List<Component> gameComponents;

        static Component currentSelection;

        enum NavigationState
        {
            Mouse,
            KeyboardDpad
        }

        static NavigationState currentState;

        public override void LoadMenuButtons()
        {
            float menuSpacing = Game1.windowY / 7;
            float offset = (Game1.windowY / 3) - 60;

            Vector2 menuSlot1 = new Vector2(Game1.windowX / 2 - 150, offset);
            Vector2 menuSlot2 = new Vector2(Game1.windowX / 2 - 150, offset + menuSpacing);
            Vector2 menuSlot3 = new Vector2(Game1.windowX / 2 - 150, offset + menuSpacing * 2);
            Vector2 menuSlot4 = new Vector2(Game1.windowX / 2 - 150, offset + menuSpacing * 3);
            Vector2 menuSlot5 = new Vector2(Game1.windowX / 2 - 150, offset + menuSpacing * 4);

            returnButton = new Button(TextureManager.UI_selectedMenuBox, TextureManager.UI_menuFont, menuSlot1)
            {
                Position = menuSlot1,
                Text = "Return to Main Menu",
            };

            sound = new Slider(menuSlot2, 100, "Sound Volume", false);


            music = new Slider(menuSlot3, 100, "Music Volume", false);

            screenSize = new Slider(menuSlot4, 4, "Screen Size", true);

            fullscreen = new Checkbox(menuSlot5, "Fullscreen");


            gameComponents = new List<Component>()
            {
                returnButton,
                sound,
                music,
                screenSize,
                fullscreen,
            };

            currentSelection = gameComponents[0];


            returnButton.Click += NewGameButton_Click;
        }

        public void NavigateMenu()
        {
            if (InputReader.MovementDownNonContinous())
            {
                if (gameComponents.IndexOf(currentSelection) + 2 > gameComponents.Count)
                {
                    currentSelection = gameComponents[0];
                }
                else
                {
                    currentSelection = gameComponents[gameComponents.IndexOf(currentSelection) + 1];
                }
            }

            if (InputReader.MovementUpNonContinous())
            {
                if (gameComponents.IndexOf(currentSelection) - 1 < 0)
                {
                    currentSelection = gameComponents[gameComponents.Count - 1];
                }
                else
                {
                    currentSelection = gameComponents[gameComponents.IndexOf(currentSelection) - 1];
                }
            }
        }

        public void MakeSelection(GameTime gameTime)
        {
            if (currentSelection == returnButton)
            {
                if (InputReader.ButtonPressed(Buttons.A) || InputReader.KeyPressed(Keys.Enter))
                {
                    MenuManager.GoToMainMenu();
                }
            }

            if (currentSelection == sound)
            {
                //sound.Update(gameTime);
                Audio.SetSoundVolume(sound.Value);
            }

            if (currentSelection == music)
            {
                //music.Update(gameTime);
                Audio.SetMusicVolume(music.Value);
            }

            if (currentSelection == screenSize)
            {
                Game1.game1.ScaleWindowAbsolute(screenSize.Value + 1);
                //screenSize.Update(gameTime);
            }

            if(currentSelection == fullscreen)
            {
                fullscreen.Update(gameTime);
                Game1.game1.ToggleFullscreen(fullscreen.Check());
            }
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            MenuManager.GoToMainMenu();
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

        public override void Update(GameTime gameTime)
        {
            MakeSelection(gameTime);
            NavigateMenu();

            foreach (Component b in gameComponents)
            {

                Vector2 windowMousePos = new Vector2(InputReader.mouseState.X / Game1.windowScale, InputReader.mouseState.Y / Game1.windowScale);

                b.isHovering = false;

                switch (currentState)
                {

                    case NavigationState.Mouse:

                        if (b.Rectangle.Contains(windowMousePos))
                        {
                            b.isHovering = true;
                            currentSelection = b;
                        }
                        break;

                    case NavigationState.KeyboardDpad:

                        if (currentSelection == b)
                        {
                            b.isHovering = true;
                        }
                        break;
                }

                if (InputReader.MovementDownNonContinous() || InputReader.MovementUpNonContinous())
                {
                    currentState = NavigationState.KeyboardDpad;

                }

                if (InputReader.mouseState != InputReader.oldMouseState)
                {
                    currentState = NavigationState.Mouse;
                }

                b.Update(gameTime);

            }

        }

        public override void Draw(SpriteBatch spriteBatch)
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
            //spriteBatch.Draw(TextureManager.playerCharacterForMenu, imagePosition, Color.White);

            spriteBatch.End();
        }
    }
}

