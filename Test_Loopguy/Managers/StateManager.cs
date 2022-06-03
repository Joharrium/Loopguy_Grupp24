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
                    Player.dashBar.Draw(spriteBatch);

                    if (Game1.editLevel)
                        LevelEditor.Draw(spriteBatch);

                    Color cursorColor = new Color(200, 200, 200, 200);
                    spriteBatch.Draw(TextureManager.cursor, new Vector2(Game1.mousePos.X - TextureManager.cursor.Width / 2, Game1.mousePos.Y - TextureManager.cursor.Height / 2), cursorColor);

                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
                    //DRAW HUD HERE
                    bool displayingTwoHints = false;
                    foreach(HintArea h in LevelManager.CurrentLevel.Hints)
                    {
                        
                        h.Draw(spriteBatch,displayingTwoHints);
                        if (h.active)
                        {
                            displayingTwoHints = true;
                        }
                    }
                    Player.ammoBar.Draw(spriteBatch);
                    Player.healthBar.Draw(spriteBatch);
                    spriteBatch.Draw(TextureManager.medkit, new Vector2(104, 12), Color.White);
                    spriteBatch.DrawString(TextureManager.UI_menuFont, "x " + EntityManager.player.StoredHealth, new Vector2(124, 7), Color.White);
                    
                    LevelManager.DrawTimer(spriteBatch);

                    Fadeout.Draw(spriteBatch);

                    if (LevelManager.GameWon)
                    {
                        string win1 = "You have won the game";
                        string win2 = "defeated evil and saved your home.";
                        string win3 = "But as you now have learned... ";
                        string win4 = "the loop never ends...";
                        float offset1 = TextureManager.smallestFont.MeasureString(win1).X / 2;
                        float offset2 = TextureManager.smallestFont.MeasureString(win2).X / 2;
                        float offset3 = TextureManager.smallestFont.MeasureString(win3).X / 2;
                        float offset4 = TextureManager.smallestFont.MeasureString(win4).X / 2;
                        OutlinedText.DrawOutlinedText(spriteBatch, new Vector2(240 - offset1, 60), TextureManager.smallestFont, win1);
                        OutlinedText.DrawOutlinedText(spriteBatch, new Vector2(240 - offset2, 90), TextureManager.smallestFont, win2);
                        OutlinedText.DrawOutlinedText(spriteBatch, new Vector2(240 - offset3, 120), TextureManager.smallestFont, win3);
                        OutlinedText.DrawOutlinedText(spriteBatch, new Vector2(240 - offset4, 150), TextureManager.smallestFont, win4);
                    }
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
