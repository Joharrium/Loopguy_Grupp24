using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Test_Loopguy.Content;

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

        public static GameState currentState = GameState.InGame;

        public static void Update(GameTime gameTime)
        {
            switch (currentState)
            {
                case GameState.Menu:

                    MenuManager.Update(gameTime);

                    break;
                case GameState.InGame:

                    LevelManager.Update(gameTime, EntityManager.player);
                    ParticleManager.Update(gameTime);
                    if(Game1.editLevel)
                        LevelEditor.Update(gameTime);

                    EntityManager.Update(gameTime);
                    CameraManager.Update(gameTime);
                    Audio.Update();



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
                    ParticleManager.Draw(spriteBatch);
                    //Player.healthBar.Draw(spriteBatch);
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
