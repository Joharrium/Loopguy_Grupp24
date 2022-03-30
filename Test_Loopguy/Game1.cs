using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Test_Loopguy
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        SpriteFont smallFont;

        Camera camera;

        Player player;

        Texture2D blueArc, redPixel;

        RenderTarget2D renderTarget;

        public static Vector2 mousePos;
        public static Rectangle screenRect;
        public static int windowX, windowY, windowScale;

        string infoString;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TexMGR.LoadTextures(Content);
            smallFont = Content.Load<SpriteFont>("smallFont");

            //Resolution and window stuff
            windowX = 480;
            windowY = 270;
            windowScale = 3;
            screenRect = new Rectangle(0, 0, windowScale * windowX, windowScale * windowY);
            renderTarget = new RenderTarget2D(GraphicsDevice, windowX, windowY);
            graphics.PreferredBackBufferWidth = screenRect.Width;
            graphics.PreferredBackBufferHeight = screenRect.Height;
            graphics.ApplyChanges();

            camera = new Camera(GraphicsDevice.Viewport);

            player = new Player(new Vector2(200, 200));

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            else if (InputReader.KeyPressed(Keys.PageUp) && windowScale <= 5)
                ScaleWindow(1);
            else if (InputReader.KeyPressed(Keys.PageDown) && windowScale >= 2)
                ScaleWindow(-1);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            InputReader.Update();

            //Update player position
            player.Update(gameTime);

            //Update camera position
            camera.SmoothPosition(player.cameraPosition);

            //Gets mouse position from window and camera position
            Vector2 windowMousePos = new Vector2(InputReader.mouseState.X / windowScale, InputReader.mouseState.Y / windowScale);
            Vector2 cameraTopLeft = new Vector2(camera.position.X - windowX / 2, camera.position.Y - windowY / 2);
            mousePos = new Vector2(cameraTopLeft.X + windowMousePos.X, cameraTopLeft.Y + windowMousePos.Y);

            //Get angles between player and stuff
            double mouseAngle = Helper.GetAngle(player.centerPosition, mousePos);
            double targetAngle = Helper.GetAngle(player.centerPosition, Vector2.Zero); //change zero vector to target

            //Converts angles from radians double to more readable stuff
            double piRadM = mouseAngle / Math.PI;
            float piRadShortM = (float)Math.Round(piRadM, 2);
            int degShortM = (int)MathHelper.ToDegrees((float)mouseAngle);

            double piRadT = targetAngle / Math.PI;
            float piRadShortT = (float)Math.Round(piRadT, 2);
            int degShortT = (int)MathHelper.ToDegrees((float)targetAngle);

            //Shows player position
            Point playerPosRounded = new Point((int)Math.Round(player.position.X, 0), (int)Math.Round(player.position.Y, 0));

            infoString = "Mouse Angle: " + piRadShortM.ToString() + "pi rad - " + degShortM.ToString() + " degrees \n"
                + "Target Angle: " + piRadShortT.ToString() + "pi rad - " + degShortT.ToString() + " degrees \n"
                + "Player position: " + playerPosRounded;

            Window.Title = player.playerInfoString;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.SlateGray);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.Transform);

            //Draw game stuff here!

            spriteBatch.Draw(TexMGR.bigcheckers, new Vector2(-2000, -2000), Color.White);
            spriteBatch.DrawString(smallFont, infoString, new Vector2(camera.position.X - windowX / 2, camera.position.Y - windowY / 2), Color.White);
            player.Draw(spriteBatch);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(renderTarget, screenRect, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);

            //Show Angles
            //Window.Title = infoString;
        }

        void ScaleWindow(int i)
        {
            windowScale += i;
            screenRect.Width = windowScale * windowX;
            screenRect.Height = windowScale * windowY;
            graphics.PreferredBackBufferWidth = screenRect.Width;
            graphics.PreferredBackBufferHeight = screenRect.Height;
            graphics.ApplyChanges();
        }

    }
}
