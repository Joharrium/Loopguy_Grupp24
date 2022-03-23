using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite.Documents;
using MonoGame.Aseprite.Graphics;

namespace Test_Loopguy
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        AsepriteDocument aseDoc;
        AnimatedSprite playerSprite;

        Camera camera;

        Texture2D blueArc, redPixel;

        RenderTarget2D renderTarget;
        public static Rectangle screenRect;
        public static int windowX, windowY, windowScale;

        Vector2 playerSpriteSize;
        Vector2 playerDirection;
        Vector2 playerPosition;
        Vector2 playerCenterPosition;

        Vector2 attackTopDirection, attackBottomDirection, attackTop, attackBottom;

        int dirint;
        float attackRange;

        float playerSpeed;
        float diagonalMultiplier;

        Target target;


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

            aseDoc = Content.Load<AsepriteDocument>("HLD");
            playerSprite = new AnimatedSprite(aseDoc);

            playerSpriteSize = new Vector2(32, 32);
            playerSpeed = 100;
            diagonalMultiplier = 0.707f;
            playerPosition = new Vector2(200, 200);

            target = new Target(new Vector2(300, 300));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputReader.Update();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            playerCenterPosition = new Vector2(playerPosition.X + playerSpriteSize.X / 2, playerPosition.Y + playerSpriteSize.Y / 2);

            camera.SetPosition(new Vector2(playerCenterPosition.X, playerCenterPosition.Y));

            playerSprite.Position = playerPosition;
            playerSprite.Update(deltaTime);

            PlayerMovement(deltaTime);

            target.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.Transform);

            playerSprite.Render(spriteBatch);

            target.Draw(spriteBatch);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(renderTarget, screenRect, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);

            //Shows player position
            Point playerPosRounded = new Point((int)Math.Round(playerPosition.X, 0), (int)Math.Round(playerPosition.Y, 0));

            //Shows angle between player and cursor in window title
            double piRadM = MouseAngle(playerCenterPosition) / Math.PI;
            float piRadShortM = (float)Math.Round(piRadM, 2);
            int degShortM = (int)MathHelper.ToDegrees((float)MouseAngle(playerCenterPosition));

            double piRadT = AngleToPlayer(playerCenterPosition, target.centerPosition) / Math.PI;
            float piRadShortT = (float)Math.Round(piRadT, 2);
            int degShortT = (int)MathHelper.ToDegrees((float)AngleToPlayer(playerCenterPosition, target.centerPosition));

            //Show
            Window.Title = "Mouse Angle: " + piRadShortM.ToString() + "π rad - " + degShortM.ToString() + " degrees || " + "Target Angle: " + piRadShortT.ToString() + "π rad - " + degShortT.ToString() + " degrees || " + "Player position: " + playerPosRounded;
        }

        protected void PlayerMovement(float deltaTime)
        {

            if (InputReader.MovementLeft())
            {
                if (InputReader.MovementUp())
                {//LEFT + UP
                    playerDirection.Y = -1 * diagonalMultiplier;
                    playerDirection.X = -1 * diagonalMultiplier;
                }
                else if (InputReader.MovementDown())
                {//LEFT + DOWN
                    playerDirection.Y = 1 * diagonalMultiplier;
                    playerDirection.X = -1 * diagonalMultiplier;
                }
                else
                {//LEFT
                    playerDirection.Y = 0;
                    playerDirection.X = -1;
                }
            }
            else if (InputReader.MovementRight())
            {
                if (InputReader.MovementUp())
                {//RIGHT + UP
                    playerDirection.Y = -1 * diagonalMultiplier;
                    playerDirection.X = 1 * diagonalMultiplier;
                }
                else if (InputReader.MovementDown())
                {//RIGHT + DOWN
                    playerDirection.Y = 1 * diagonalMultiplier;
                    playerDirection.X = 1 * diagonalMultiplier;
                }
                else
                {//RIGHT
                    playerDirection.Y = 0;
                    playerDirection.X = 1;
                }
            }
            else if (InputReader.MovementUp())
            {//UP
                playerDirection.Y = -1;
                playerDirection.X = 0;
            }
            else if (InputReader.MovementDown())
            {//DOWN
                playerDirection.Y = 1;
                playerDirection.X = 0;
            }
            else
            {//Analog Stick movement
                playerDirection.X = InputReader.padState.ThumbSticks.Left.X;
                playerDirection.Y = -InputReader.padState.ThumbSticks.Left.Y;
            }



            //Visual changes depending on direction
            if (playerDirection.Y < 0 && (float)Math.Abs(playerDirection.X) < (float)Math.Abs(playerDirection.Y))
            {
                playerSprite.Play("up");
                dirint = 3;
            }
            else if (playerDirection.Y > 0 && (float)Math.Abs(playerDirection.X) < (float)Math.Abs(playerDirection.Y))
            {
                playerSprite.Play("down");
                dirint = 4;
            }
            else if (playerDirection.X < 0)
            {
                playerSprite.Play("left");
                dirint = 1;
            }
            else if (playerDirection.X > 0)
            {
                playerSprite.Play("right");
                dirint = 2;
            }
            else
            {
                if (dirint == 1)
                    playerSprite.Play("leftidle");
                else if (dirint == 2)
                    playerSprite.Play("rightidle");
                else if (dirint == 3)
                    playerSprite.Play("upidle");
                else
                    playerSprite.Play("downidle");
            }

            playerPosition += playerDirection * playerSpeed * deltaTime;
        }

        protected double MouseAngle(Vector2 playerPos)
        {
            //Adds mouse position to camera position
            Vector2 mousePos = new Vector2(InputReader.mouseState.X / windowScale, InputReader.mouseState.Y / windowScale);
            Vector2 cameraCenterPos = new Vector2(camera.position.X - windowX / 2, camera.position.Y - windowY / 2);
            mousePos = new Vector2(cameraCenterPos.X + mousePos.X, cameraCenterPos.Y + mousePos.Y);

            double v = Math.Atan2(mousePos.X - playerPos.X, mousePos.Y - playerPos.Y);

            //Adjust this according to where the angle is measured from
            v -= Math.PI / 2;

            if (v < 0.0)
                v += Math.PI * 2;

            return v;
        }

        protected double AngleToPlayer(Vector2 playerPos, Vector2 otherPos)
        {
            double v = Math.Atan2(otherPos.X - playerPos.X, otherPos.Y - playerPos.Y);

            v -= Math.PI / 2;

            if (v < 0.0)
                v += Math.PI * 2;

            return v;
        }
    }
}
