using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    class CameraManager
    {
        public static Camera camera;
        //private GraphicsDeviceManager graphics;
        //private SpriteBatch spriteBatch;

        //static RenderTarget2D renderTarget;


        public static void LoadCamera()
        {

            //graphics = new GraphicsDeviceManager(this);

            camera = new Camera();
            camera.SetPosition(new Vector2(200, 200));


          
        }

        //public void LoadCamera()
        //{

        //    spriteBatch = new SpriteBatch(GraphicsDevice);

        //    camera = new Camera(GraphicsDevice.Viewport);
        //    camera.SetPosition(new Vector2(200, 200));
        //}

        public static void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Update camera position
            camera.SmoothPosition(EntityManager.player.cameraPosition, deltaTime);

            //Vector2 windowMousePos = new Vector2(InputReader.mouseState.X / Game1.windowScale, InputReader.mouseState.Y / Game1.windowScale);
            //Vector2 cameraTopLeft = new Vector2(camera.position.X - Game1.windowX / 2, camera.position.Y - Game1.windowY / 2);

            //Gets mouse position from window and camera position
            Vector2 windowMousePos = new Vector2(InputReader.mouseState.X / Game1.windowScale, InputReader.mouseState.Y / Game1.windowScale);
            Vector2 cameraTopLeft = new Vector2(camera.clampedPosition.X, camera.clampedPosition.Y);
            if (!camera.yClamped)
            {
                cameraTopLeft.Y = camera.position.Y - Game1.windowY / 2;
            }
            else if (cameraTopLeft.Y != 0)
            {
                cameraTopLeft.Y *= -1;
            }

            if (!camera.xClamped)
            {
                cameraTopLeft.X = camera.position.X - Game1.windowX / 2;
            }
            else if (cameraTopLeft.X != 0)
            {
                cameraTopLeft.X *= -1;
            }

        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.renderTarget, Game1.screenRect, Color.White);
            Fadeout.Draw(spriteBatch);
            //spriteBatch.DrawString(TextureManager.smallFont, infoString, Vector2.Zero, Color.White);
            spriteBatch.DrawString(TexMGR.UI_smallFont, camera.xClamped.ToString(), new Vector2(0, 80), Color.White);
            spriteBatch.DrawString(TexMGR.UI_smallFont, camera.yClamped.ToString(), new Vector2(0, 92), Color.White);
            spriteBatch.End();
        }
    }
}
