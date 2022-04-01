using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Test_Loopguy
{
    public class Game1 : Game
    {
        public static Random rnd = new Random();
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        SpriteFont smallFont;

        public static Camera camera;

        Player player;

        Texture2D blueArc, redPixel;

        RenderTarget2D renderTarget;

        public static Vector2 mousePos;
        public static Rectangle screenRect;
        public static int windowX, windowY, windowScale;

        bool editLevel = false;

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

            LevelManager.LoadLevel(1);

            camera = new Camera(GraphicsDevice.Viewport);
            camera.SetPosition(new Vector2(200, 200));

            player = new Player(new Vector2(64, 64));
            //TileManager.Initialization();
            //WallManager.Initialization();
            

            var frmNewForm = new Form1();
            var newThread = new System.Threading.Thread(frmNewFormThread);

            newThread.SetApartmentState(System.Threading.ApartmentState.STA);
            newThread.Start();

            void frmNewFormThread()
            {
                Application.Run(frmNewForm);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                Exit();
                Application.Exit();
            }
            else if (InputReader.KeyPressed(Microsoft.Xna.Framework.Input.Keys.PageUp) && windowScale <= 5)
                ScaleWindow(1);
            else if (InputReader.KeyPressed(Microsoft.Xna.Framework.Input.Keys.PageDown) && windowScale >= 2)
                ScaleWindow(-1);
            else if (InputReader.KeyPressed(Microsoft.Xna.Framework.Input.Keys.F1))
                editLevel = !editLevel;

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            InputReader.Update();

            //Update player position
            player.Update(gameTime);
            LevelManager.Update(gameTime);
            //Update camera position
            camera.SmoothPosition(player.cameraPosition, deltaTime);

            //Gets mouse position from window and camera position
            Vector2 windowMousePos = new Vector2(InputReader.mouseState.X / windowScale, InputReader.mouseState.Y / windowScale);
            Vector2 cameraTopLeft = new Vector2(camera.position.X - windowX / 2, camera.position.Y - windowY / 2);
            mousePos = new Vector2(cameraTopLeft.X + windowMousePos.X, cameraTopLeft.Y + windowMousePos.Y);

            //Get angles between player and stuff
            double mouseAngle = Helper.GetAngle(player.centerPosition, mousePos, 0);
            double targetAngle = Helper.GetAngle(player.centerPosition, Vector2.Zero , 0); //change zero vector to target

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

            if (editLevel)
            {
                LevelEditor.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.SlateGray);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.Transform);
            //Draw game stuff here!

            LevelManager.Draw(spriteBatch);

            if (editLevel)
            {
                LevelEditor.Draw(spriteBatch);
            }

            player.Draw(spriteBatch);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            
            spriteBatch.Draw(renderTarget, screenRect, Color.White);
            spriteBatch.DrawString(smallFont, infoString, Vector2.Zero, Color.White);
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
