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

        public static GameState currentState = GameState.Menu;

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

                    //Everything that goes into the menuscreen draws in MenuManager.Draw()
                    MenuManager.Draw(spriteBatch);

                    break;
                case GameState.InGame:

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Game1.camera.Transform);
                    //Draw game stuff here!

                    LevelManager.Draw(spriteBatch);
                    ParticleManager.Draw(spriteBatch);

                    if (Game1.editLevel)
                        LevelEditor.Draw(spriteBatch);

                    Color cursorColor = new Color(200, 200, 200, 200);
                    spriteBatch.Draw(TextureManager.cursor, new Vector2(Game1.mousePos.X - TextureManager.cursor.Width / 2, Game1.mousePos.Y - TextureManager.cursor.Height / 2), cursorColor);

                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
                    //DRAW HUD HERE

                    Player.ammoBar.Draw(spriteBatch);
                    Player.healthBar.Draw(spriteBatch);
                    LevelManager.DrawTimer(spriteBatch);
                    spriteBatch.End();

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
