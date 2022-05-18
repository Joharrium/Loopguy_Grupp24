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
    class MainMenu : Menu
    {
        static bool usingMouse = false;

        static Button newGameButton, loadGameButton, highScoreButton, settingsButton, quitGameButton;
    
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
            float menuSpacing = Game1.windowY / 10;

            Vector2 menuSlot1 = new Vector2(Game1.windowX / 2, Game1.windowY / 2);
            Vector2 menuSlot2 = new Vector2(Game1.windowX / 2, Game1.windowY / 2 + menuSpacing);
            Vector2 menuSlot3 = new Vector2(Game1.windowX / 2, Game1.windowY / 2 + menuSpacing * 2);
            Vector2 menuSlot4 = new Vector2(Game1.windowX / 2, Game1.windowY / 2 + menuSpacing * 3);
            Vector2 menuSlot5 = new Vector2(Game1.windowX / 2, Game1.windowY / 2 + menuSpacing * 4);

            newGameButton = new Button(TextureManager.UI_selectedMenuBox, TextureManager.UI_menuFont, menuSlot1)
            {
                Position = menuSlot1,
                Text = "Start Game",
            };

            loadGameButton = new Button(TextureManager.UI_selectedMenuBox, TextureManager.UI_menuFont, menuSlot2)
            {
                Position = menuSlot2,
                Text = "Profile : 1",
            };

            highScoreButton = new Button(TextureManager.UI_selectedMenuBox, TextureManager.UI_menuFont, menuSlot3)
            {
                Position = menuSlot3,
                Text = "High Score",
            };

            settingsButton = new Button(TextureManager.UI_selectedMenuBox, TextureManager.UI_menuFont, menuSlot4)
            {
                Position = menuSlot4,
                Text = "Settings",
            };

            quitGameButton = new Button(TextureManager.UI_selectedMenuBox, TextureManager.UI_menuFont, menuSlot5)
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

            currentSelection = gameComponents[0];


            newGameButton.Click += NewGameButton_Click;
            loadGameButton.Click += LoadGameButton_Click;
            highScoreButton.Click += HighScoreButton_Click;
            settingsButton.Click += SettingsButton_Click;
            quitGameButton.Click += QuitGameButton_Click;

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

        public void MakeSelection()
        {
            if (currentSelection == newGameButton)
            {
                if (InputReader.ButtonPressed(Buttons.A) || InputReader.KeyPressed(Keys.Enter))
                {
                    StateManager.currentState = StateManager.GameState.InGame;
                    Audio.UpdateLevelMusic();
                }
            }

            if (currentSelection == loadGameButton)
            {
                if (InputReader.ButtonPressed(Buttons.A) || InputReader.KeyPressed(Keys.Enter))
                {
                    ProfileManager.ChangeProfile();
                    loadGameButton.Text = "Profile : " + ProfileManager.CurrentProfileId();
                }
            }

            if (currentSelection == highScoreButton)
            {
                if (InputReader.ButtonPressed(Buttons.A) || InputReader.KeyPressed(Keys.Enter))
                {
                    highScoreButton.isHovering = false;
                }
            }

            if (currentSelection == settingsButton)
            {
                if (InputReader.ButtonPressed(Buttons.A) || InputReader.KeyPressed(Keys.Enter))
                {
                    MenuManager.GoToSettingsMenu();
                }
            }

            if (currentSelection == quitGameButton)
            {
                if (InputReader.ButtonPressed(Buttons.A) || InputReader.KeyPressed(Keys.Enter))
                {
                    Game1.game1.Exit();
                    Game1.editorFrm.BeginInvoke(new Action(() => CloseEditorForm()));
                }
            }
        }

        private static void NewGameButton_Click(object sender, EventArgs e)
        {
            StateManager.currentState = StateManager.GameState.InGame;
            Audio.UpdateLevelMusic();
        }

        private static void LoadGameButton_Click(object sender, EventArgs e)
        {
            ProfileManager.ChangeProfile();
            loadGameButton.Text = "Profile : " + ProfileManager.CurrentProfileId();
        }

        private static void HighScoreButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void SettingsButton_Click(object sender, EventArgs e)
        {
            MenuManager.GoToSettingsMenu();
        }

        //Quits the game, however what I think is due to the win.forms the application doesn't stop at this point but only closes the windows.
        private static void QuitGameButton_Click(object sender, EventArgs e)
        {
            Game1.game1.Exit();
            Game1.editorFrm.BeginInvoke(new Action(() => CloseEditorForm()));
        }

        private static void CloseEditorForm()
        {
            Game1.editorFrm.Close(); // byebye
        }

        public override void Update(GameTime gameTime)
        {
            MakeSelection();
            NavigateMenu();

            foreach (Button b in gameComponents.OfType<Button>())
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
            Vector2 imagePosition = new Vector2(- 24, (Game1.windowY / 10) + gameComponents.IndexOf(currentSelection)*(Game1.windowY / 10));
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
            spriteBatch.Draw(TextureManager.logo, new Vector2(240, 20), Color.White);

            //Draw image
            spriteBatch.Draw(TextureManager.playerCharacterForMenu, imagePosition, Color.White);

            spriteBatch.End();
        }
    }
}
