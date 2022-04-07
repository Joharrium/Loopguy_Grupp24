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
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;



        public void LoadCamera()
        {

            //graphics = new GraphicsDeviceManager(this);



            //camera = new Camera(GraphicsDevice.Viewport);
            //camera.SetPosition(new Vector2(200, 200));
        }

        //public void LoadCamera()
        //{

        //    spriteBatch = new SpriteBatch(GraphicsDevice);

        //    camera = new Camera(GraphicsDevice.Viewport);
        //    camera.SetPosition(new Vector2(200, 200));
        //}

        public static void Update(GameTime gameTime)
        {
            //float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //camera.SmoothPosition(EntityManager.player.cameraPosition, deltaTime);

            //Vector2 windowMousePos = new Vector2(InputReader.mouseState.X / Game1.windowScale, InputReader.mouseState.Y / Game1.windowScale);
            //Vector2 cameraTopLeft = new Vector2(camera.position.X - Game1.windowX / 2, camera.position.Y - Game1.windowY / 2);


        }
    }
}
