using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System.Diagnostics;

namespace Test_Loopguy
{
    public class Game1 : Game
    {
        public static Random rnd = new Random();
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        SpriteFont smallFont;

        public static Camera camera;
        public static Game1 game1;
        public static Form1 editorFrm;

        //Player player;

        Texture2D blueArc, redPixel;

        RenderTarget2D renderTarget;

        public static Vector2 mousePos;
        public static Rectangle screenRect;
        public static int windowX, windowY, windowScale;

        public static bool editLevel = false;

        string infoString;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            game1 = this; //Used for Exit() in menuManager
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ProfileManager.Init();

            TextureManager.LoadTextures(Content);
         
            EntityManager.PlayerInitialization();
            Audio.Load(Content);
            smallFont = Content.Load<SpriteFont>("smallFont");

            InputReader.editMode = editLevel;

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
            LevelManager.EntranceLoad();
            MenuManager.LoadMenuButtons();

            camera = new Camera();
            camera.SetPosition(new Vector2(200, 200));

            editorFrm = new Form1();
            Thread newThread = new System.Threading.Thread(frmNewFormThread);

            newThread.SetApartmentState(System.Threading.ApartmentState.STA);
            newThread.Start();

            void frmNewFormThread()
            {
                Application.Run(editorFrm);
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
            {
                LevelManager.RefreshEdges();
                editLevel = !editLevel;
                InputReader.editMode = editLevel;
                if(editLevel)
                {

                }
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            InputReader.Update();
            Fadeout.Update(gameTime);

            //Update player position
            //player.Update(gameTime);
            //Update camera position
            camera.SmoothPosition(EntityManager.player.cameraPosition, deltaTime);

            //Gets mouse position from window and camera position
            Vector2 windowMousePos = new Vector2(InputReader.mouseState.X / windowScale, InputReader.mouseState.Y / windowScale);
            Vector2 cameraTopLeft = new Vector2(camera.clampedPosition.X, camera.clampedPosition.Y);
            if(!camera.yClamped)
            {
                cameraTopLeft.Y = camera.position.Y - windowY/2;
            }
            else if (cameraTopLeft.Y != 0)
            {
                cameraTopLeft.Y *= -1;
            }

            if (!camera.xClamped)
            {
                cameraTopLeft.X = camera.position.X - windowX/2;
            }
            else if(cameraTopLeft.X != 0)
            {
                cameraTopLeft.X *= -1;
            }

            mousePos = new Vector2(cameraTopLeft.X + windowMousePos.X, cameraTopLeft.Y + windowMousePos.Y);

            //Get angles between player and stuff
            double mouseAngle = Helper.GetAngle(EntityManager.player.centerPosition, mousePos, 0);

            //Converts angles from radians double to more readable stuff
            double piRadM = mouseAngle / Math.PI;
            float piRadShortM = (float)Math.Round(piRadM, 2);
            int degShortM = (int)MathHelper.ToDegrees((float)mouseAngle);

            //Shows player position
            Point playerPosRounded = new Point((int)Math.Round(EntityManager.player.position.X, 0), (int)Math.Round(EntityManager.player.position.Y, 0));

            infoString = "Mouse Angle: " + piRadShortM.ToString() + "pi rad - " + degShortM.ToString() + " degrees \n"
                + "Player position: " + playerPosRounded;

            Window.Title = EntityManager.player.playerInfoString;

            StateManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.SlateGray);

            StateManager.Draw(spriteBatch);

            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            
            spriteBatch.Draw(renderTarget, screenRect, Color.White);
            Fadeout.Draw(spriteBatch);
            spriteBatch.DrawString(smallFont, infoString + "\n" + EntityManager.player.playerInfoString, Vector2.Zero, Color.White);
            spriteBatch.DrawString(smallFont, "current level = " + LevelManager.GetCurrentId().ToString(), new Vector2(0, 64), Color.White);
            spriteBatch.DrawString(smallFont, "x clamp = " + camera.xClamped.ToString(), new Vector2(0, 80), Color.White);
            spriteBatch.DrawString(smallFont, "y clamp = " + camera.yClamped.ToString(), new Vector2(0, 96 ), Color.White);

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
