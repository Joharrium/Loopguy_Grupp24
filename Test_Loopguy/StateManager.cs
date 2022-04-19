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
        RenderTarget2D renderTarget;

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

                    //CameraManager.camera.xClamped = false;
                    //CameraManager.camera.yClamped = false;

                    CameraManager.Update(gameTime);

                    MenuManager.Update(gameTime);

                    break;
                case GameState.InGame:

                    //GraphicsDevice.SetRenderTarget(Game1.renderTarget);

                    LevelManager.Update(gameTime, EntityManager.player);

                    if(Game1.editLevel)
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

                    if(Game1.editLevel)
                        LevelEditor.Draw(spriteBatch);

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
