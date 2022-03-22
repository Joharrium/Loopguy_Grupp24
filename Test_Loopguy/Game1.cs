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

            blueArc = Content.Load<Texture2D>("blueArc");
            redPixel = Content.Load<Texture2D>("redPixel");

            //Resolution and window stuff
            windowX = 480;
            windowY = 270;
            windowScale = 2;
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.Transform);

            playerSprite.Render(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(renderTarget, screenRect, Color.White);
            spriteBatch.End();

            //Shows angle between player and cursor in window title
            double piRad = MouseAngle(playerCenterPosition) / Math.PI;
            float piRadShort = (float)Math.Round(piRad, 2);
            int degShort = (int)MathHelper.ToDegrees((float)MouseAngle(playerCenterPosition));
            Window.Title = "Mouse Angle: " + piRadShort.ToString() + "π rad - " + degShort.ToString() + " degrees";
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
            Vector2 mousePos = new Vector2(InputReader.mouseState.X / windowScale, InputReader.mouseState.Y / windowScale);
            double v = Math.Atan2(mousePos.X - playerPos.X, mousePos.Y - playerPos.Y);

            //Adjust according to where the angle is measured from
            v -= Math.PI / 2;

            if (v < 0.0)
                v += Math.PI * 2;

            return v;
        }
    }
}
