using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test_Loopguy
{
    class StateManager
    {
        


        public enum GameState
        {
            Menu,
            InGame,
            InEditor,
            Load,
        }

        public static GameState currentState = GameState.Menu;

        public static void Update(GameTime gameTime)
        {
            switch (currentState)
            {
                case GameState.Menu:

                    MenuManager.Update(gameTime);

                    break;
                case GameState.InGame:

                    LevelManager.Update(gameTime);
                    LevelEditor.Update(gameTime);

                    EntityManager.Update(gameTime);
                    CameraManager.Update(gameTime);



                    break;
                case GameState.InEditor:
                    break;
                case GameState.Load:
                    break;
                default:
                    break;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            switch (currentState)
            {
                case GameState.Menu:

                    MenuManager.Draw(spriteBatch);

                    break;
                case GameState.InGame:

                    LevelManager.Draw(spriteBatch);

                    EntityManager.Draw(spriteBatch);

                    break;
                case GameState.InEditor:
                    break;
                case GameState.Load:
                    break;
                default:
                    break;
            }
        }

    }
}
