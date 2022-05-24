using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Test_Loopguy.Content;

namespace Test_Loopguy
{
    internal class Player : Character
    {
        enum Gun
        {
            Pistol,
            Railgun
        }

        Gun equippedGun;

        private int ammo = 90;
        private int maxAmmo = 90;

        private int storedHealthPacks = 0;
        public int StoredHealth
        {
            get { return storedHealthPacks; }
        }

        private const int maxStoredHealthPacks = 3;

        AnimatedSprite pistolSprite;
        AnimatedSprite railgunSprite;
        AnimatedSprite railgunImpactSprite;
        AnimatedSprite meleeSprite;
        AnimatedSprite dashCloudSprite;

        Random random = new Random();
        public List<int> keys = new List<int>();

        Vector2 pistolOrigin, railgunOrigin;
        Vector2 railgunBeamPosition;
        Vector2 railgunImpactPosition;

        Vector2 roomEntrancePosition = new Vector2(64, 64);

        //Wtf
        public Vector2 cameraPosition;
        Vector2 gunDirection;
        Vector2 prevDirection;
        Vector2 dashDirection;
        Vector2 dashPosition;

        float distanceBetweenFootsteps = 35;

        public bool usedGate;
        static public PlayerHealthBar healthBar;
        static public AmmoBar ammoBar;
        static public DashBar dashBar;

        float gunAngle;
        float aimAngle;

        float beamAngle;

        float deltaTime;

        float timeInvincible;
        const float maxTimeInvincible = 0.5f;

        float timeSinceAttack; //counts seconds since last attack
        const float comboWindow = 0.75f; //window of time to follow up attack
        const float attackCooldown = 0.5f; //time you have to wait to attack again if you miss window
        const int maxCombo = 3; //number of times you can attack in quick succession before having to wait for attackCooldown
        int comboCounter; //number of times you've attacked in a quick succession

        float timeShot; //counts seconds you've shot for

        float timeSinceDash;
        const float timeBetweedDashes = 1f;

        float timePressedDash; //counts seconds you've held dash button
        const float timeToPrecisionDash = 0.25f; //seconds you have to hold dash button to initiate precision dash (aimed dash)
        const float maxTimePrecisionDash = 3; //seconds you can hold the dash button before you automatically dash

        float dashSlideTimer; //counts seconds you've slid after dash
        const float maxDashSlideTime = 0.2f; //seconds that you'll slide after a dash, also window for gun drifting

        const int dashRange = 5; //pixels per frame
        const int standardMaxDashFrames = 10; 
        float maxDashFrames; //frames per dash
        int dashFrames; //counts frames you've dashed

        public string playerInfoString;

        bool invincible;
        public bool attacking;
        bool startAttackTimer;
        bool canAttack;
        bool shooting;
        bool canDash;
        public bool dashing;
        bool dashCloud;
        bool dashSlide;

        public bool hasRailgun;

        bool checkDash;

        bool flipMelee;

        public Player(Vector2 position)
            : base(position)
        {
            sprite = new AnimatedSprite(TextureManager.playerSheet, new Point(32, 32));

            pistolSprite = new AnimatedSprite(TextureManager.pistolSheet, new Point(32, 34));
            railgunSprite = new AnimatedSprite(TextureManager.railgunSheet, new Point(32, 48));
            railgunImpactSprite = new AnimatedSprite(TextureManager.railgunImpactSheet, new Point(32, 32));
            meleeSprite = new AnimatedSprite(TextureManager.meleeFx, new Point(50, 50));
            dashCloudSprite = new AnimatedSprite(TextureManager.dashCloud, new Point(42, 24));

            pistolOrigin = new Vector2(pistolSprite.size.X / 2, 2);
            railgunOrigin = new Vector2(railgunSprite.size.X / 2, 2);

            maxHealth = 5;
            health = maxHealth;

            speed = 100; //pixels per second
            maxDashFrames = standardMaxDashFrames;

            footprintOffset = new Point(12, 24);
            primaryOrientation = Orientation.Down;
            secondaryOrientation = Orientation.Down;
            keys.AddRange(ProfileManager.GetKeys());
            healthBar = new PlayerHealthBar(maxHealth);
            ammoBar = new AmmoBar(maxAmmo);
            dashBar = new DashBar(100);
            footprint = new Rectangle((int)position.X, (int)position.Y + 24, 8, 8);

            canAttack = true;
            canDash = false; //if its true you will always dash when loading in from menu lol

            equippedGun = Gun.Pistol;
            hasRailgun = ProfileManager.HasRailgun;
        }
        

        public override void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            centerPosition = new Vector2(position.X + sprite.size.X / 2, position.Y + sprite.size.Y / 2);
            hitBox = new Rectangle((int)(position.X + sprite.size.X / 4), (int)(position.Y + sprite.size.Y / 4), sprite.size.X / 2, sprite.size.Y / 2);
            footprint = new Rectangle((int)position.X + 12, (int)position.Y + 24, 8, 8);

            //USING HEALTHPACK
            if(InputReader.KeyPressed(Keys.H) || InputReader.ButtonPressed(Buttons.Y))
                UseHealthPack();

            //SWITCHING GUN
            if (InputReader.SwitchGun() && hasRailgun)
            {
                equippedGun += 1;
                if ((int)equippedGun >= Enum.GetValues(typeof(Gun)).Length)
                    equippedGun = 0;
            }

            //DASH BAR
            if (timeSinceDash != 0)
                dashBar.SetCurrentValue(footprint.Location.ToVector2(), Math.Min((int)(timeSinceDash * 100), 100));
            if(canDash)
                dashBar.SetCurrentValue(footprint.Location.ToVector2(), 100);
            
            //HEALTH AND AMMO BAR
            healthBar.UpdateBar(health);
            ammoBar.SetCurrentValue(new Vector2(2, 38), ammo);

            //This method runs the timers and cooldowns
            Timers(deltaTime);
            
            if (checkDash) //DIRECTING DASH
            {
                timePressedDash += deltaTime;

                //This is for "sliding" to a stop when holding dash button, prevents awkward stop when doing a quick dash and also looks cool
                SlideStop(deltaTime, 200);

                if (!InputReader.Dash() || timePressedDash > maxTimePrecisionDash)
                {
                    Audio.PlaySound(Audio.dash);
                    dashPosition = new Vector2(footprint.Center.X - 21, footprint.Center.Y - 12);
                    dashing = true;
                    canDash = false;
                    checkDash = false;
                }
                else if (timePressedDash > timeToPrecisionDash)
                {
                    maxDashFrames += deltaTime * 4; // <-- increase time coefficient to make precision dash range increase faster, decrease to make slower

                    aimAngle = GetAim(0);

                    if (InputReader.MovingLeftStick())
                    {
                        direction.X = InputReader.padState.ThumbSticks.Left.X;
                        direction.Y = -InputReader.padState.ThumbSticks.Left.Y;
                    }
                    else
                    {
                        direction = new Vector2((float)Math.Sin(aimAngle), (float)Math.Cos(aimAngle));
                    }
                }

            } //ATTACKING
            else if (attacking)
            {
                speed = 75;
                int rowInt = 6 + (int)primaryOrientation - 1;
                attacking = PlayEnergySwordMelee(deltaTime, meleeSprite, rowInt, 50, flipMelee);
            }
            else if (dashing) //DASHING
            {
                //dash method is run in Draw because it needs spritebatch, yes it is wack
                timePressedDash = 0;
                dashSlide = true;

                speed = 150;
            }
            else //NOT DASHING OR ATTACKING
            {
                //this is to prevent moonwalking
                sprite.flipHorizontally = false;

                maxDashFrames = standardMaxDashFrames;

                //AIMING GUN
                if (InputReader.Aim() || shooting)
                {
                    gunAngle = GetAim(0);
                    AngleGetOrientation(aimAngle);

                    SlideyMovement(deltaTime, 2);
                    SlideStop(deltaTime, 200);

                    cameraPosition = centerPosition + gunDirection * 50;
                    Game1.camera.stabilize = true;

                    //PRESSING SHOOT
                    if (InputReader.Shoot() && !shooting)
                    {
                        railgunImpactSprite.ResetAnimation();
                        railgunBeamPosition = centerPosition;
                        beamAngle = aimAngle;

                        if (equippedGun == Gun.Pistol && ammo > 0)
                        {
                            pistolSprite.ResetAnimation();

                            Vector2 shotPosition = new Vector2(centerPosition.X + gunDirection.X * 20 - 4, centerPosition.Y + gunDirection.Y * 20 - 6);
                            float shotAngle = aimAngle + pi;
                            Shot shot = new Shot(shotPosition, gunDirection, shotAngle, 300, 1);
                            LevelManager.AddPlayerProjectile(shot);
                            Audio.lasergun.PlayRandomSound();

                            ammo--;

                            shooting = true;
                        }
                        else if (equippedGun == Gun.Railgun && ammo > 2)
                        {
                            railgunSprite.ResetAnimation();

                            ammo -= 3;

                            shooting = true;
                        }
                    }

                    //SHOOTING
                    if (shooting)
                    {
                        int frameTime = 25;

                        if (equippedGun == Gun.Pistol)
                            shooting = pistolSprite.PlayOnce((int)primaryOrientation, 5, frameTime);
                        else
                            shooting = railgunSprite.PlayOnce((int)primaryOrientation, 5, frameTime);

                        railgunImpactSprite.Update(gameTime);
                        timeShot += deltaTime;
                    }
                    else
                        timeShot = 0;
                }
                else //NOT AIMING GUN (WALKING OR IDLING)
                {
                    Game1.camera.stabilize = false;

                    if (direction == Vector2.Zero)
                        cameraPosition = centerPosition + prevDirection * 30;
                    else
                        cameraPosition = centerPosition + direction * 30;


                    if (!InputReader.MovementInput() || sprite.currentFrame.Y == 5)
                    {
                        if (primaryOrientation == Orientation.Up)
                            sprite.Frame(0, 4);
                        else if (primaryOrientation == Orientation.Down)
                            sprite.Frame(1, 4);
                        else if (primaryOrientation == Orientation.Left)
                            sprite.Frame(2, 4);
                        else
                            sprite.Frame(3, 4);
                    }

                    if (direction != Vector2.Zero && !InputReader.Aim())
                        gunDirection = Vector2.Zero;

                    Movement(deltaTime);

                    if (InputReader.Attack() && !attacking && canAttack && comboCounter < maxCombo)
                    {
                        //for melee combo
                        comboCounter++;
                        timeSinceAttack = 0;
                        startAttackTimer = true;

                        meleeSprite.ResetAnimation();
                        sprite.ResetAnimation();

                        //flip melee attack animation every attack
                        flipMelee = !flipMelee;

                        attacking = true;
                        Audio.PlaySound(Audio.swing);
                    }
                    else if (canDash && InputReader.Dash())
                    {
                        checkDash = true;
                    }
                    else
                    {
                        timePressedDash = 0;
                    }
                }
            }

            //POSITIONS AND SPRITES
            FootstepCalculation();
            sprite.Position = position;
            pistolSprite.Position = position;
            railgunSprite.Position = new Vector2(position.X, position.Y - 7);
            meleeSprite.Position = new Vector2(position.X - 9, position.Y - 9);

            pistolSprite.Update(gameTime);
            railgunSprite.Update(gameTime);
            meleeSprite.Update(gameTime);
            dashCloudSprite.Update(gameTime);
            sprite.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //draw footprint
            //spriteBatch.Draw(TextureManager.black_screen, footprint, Color.White);

            if (dashCloud)
            {
                dashCloudSprite.Position = dashPosition;
                dashCloud = DrawDashCloud(spriteBatch);
            }

            if (checkDash)
            {
                if (timePressedDash > timeToPrecisionDash)
                    DrawDashAim(spriteBatch);

                sprite.Frame((int)primaryOrientation - 1, 12);
                sprite.Draw(spriteBatch, invincible);
            }
            else if (attacking)
            {
                sprite.Draw(spriteBatch, invincible);
                meleeSprite.Draw(spriteBatch);
            }
            else
            {
                if (primaryOrientation == Orientation.Up)
                { //if aiming up, draw player sprite on top

                    if (dashing)
                    {
                        if (dashFrames <= maxDashFrames)
                        {
                            Dash(spriteBatch);
                        }
                        else
                        {
                            dashing = false;
                            dashFrames = 0;
                        }
                    }
                    else if (InputReader.Aim() || shooting)
                    {
                        DrawGunAim(spriteBatch);

                        if (equippedGun == Gun.Pistol)
                            pistolSprite.DrawRotation(spriteBatch, gunAngle, pistolOrigin);
                        else if (equippedGun == Gun.Railgun)
                        {
                            if (shooting)
                            {
                                RailGunBeam(spriteBatch, timeShot, 25, beamAngle, railgunBeamPosition);
                            }

                            railgunSprite.DrawRotation(spriteBatch, gunAngle, railgunOrigin);
                        }
                    }

                    sprite.Draw(spriteBatch, invincible);
                }
                else
                { //if not aiming up, draw gun sprite on top

                    sprite.Draw(spriteBatch, invincible);

                    if (dashing)
                    {
                        Dash(spriteBatch);
                    }
                    else if (InputReader.Aim() || shooting)
                    {
                        DrawGunAim(spriteBatch);

                        if (equippedGun == Gun.Pistol)
                            pistolSprite.DrawRotation(spriteBatch, gunAngle, pistolOrigin);
                        else if (equippedGun == Gun.Railgun)
                        {
                            if (shooting)
                            {
                                RailGunBeam(spriteBatch, timeShot, 25, beamAngle, railgunBeamPosition);
                            }

                            railgunSprite.DrawRotation(spriteBatch, gunAngle, railgunOrigin);
                        }
                    }
                }
            }

            //draw hitbox borders
            //spriteBatch.Draw(TextureManager.redPixel, new Vector2(hitBox.Left, hitBox.Top), Color.White);
            //spriteBatch.Draw(TextureManager.redPixel, new Vector2(hitBox.Right, hitBox.Bottom), Color.White);

            //spriteBatch.Draw(TextureManager.redPixel, centerPosition, Color.White);

            //healthBar.Draw(spriteBatch);

        }

        public void Dash(SpriteBatch spriteBatch)
        {
            if (dashFrames <= maxDashFrames)
            {
                DashFrame(spriteBatch);
                dashDirection = direction;
                dashCloud = true;
                dashFrames++;
            }
            else
            {
                dashing = false;
                dashFrames = 0;
            }
        }

        public void DashFrame(SpriteBatch spriteBatch)
        {
            sprite.Frame((int)primaryOrientation - 1, 13);

            if (direction == Vector2.Zero)
            {
                if (primaryOrientation == Orientation.Up)
                    direction.Y = -1;
                else if (primaryOrientation == Orientation.Down)
                    direction.Y = 1;
                else if (primaryOrientation == Orientation.Left)
                    direction.X = -1;
                else
                    direction.X = 1;
            }

            Vector2 viablePos = centerPosition + new Vector2(0, 12);
;

            for (int i = 1; i < dashRange + 1; i++)
            {
                Vector2 dashPos = new Vector2(centerPosition.X + i * direction.X, centerPosition.Y + i * direction.Y) + new Vector2(0, 12);
                
                Rectangle futureFootPrint = new Rectangle((int)dashPos.X - footprint.Width / 2, (int)dashPos.Y - footprint.Height / 2, footprint.Width, footprint.Height);


                if (LevelManager.LevelObjectCollision(futureFootPrint, 0) || LevelManager.WallCollision(dashPos))
                {
                    break;
                }
                else
                {
                    sprite.DrawElsewhere(spriteBatch, new Vector2(dashPos.X - sprite.size.X / 2, dashPos.Y - sprite.size.Y / 2 - 12), 40);
                    viablePos = dashPos;
                }
            }

            viablePos += new Vector2(0, - 12);
            viablePos = new Vector2(viablePos.X - sprite.size.X / 2, viablePos.Y - sprite.size.Y / 2);
            position = viablePos;

        }

        public override void Movement(float deltaTime)
        {
            float absDirection = Math.Abs(direction.X) + Math.Abs(direction.Y);

            //Changes frame rate depending on direction vector

            if (absDirection > 1)
                absDirection = 1;
            else if (absDirection != 0 && absDirection < 0.2f)
                absDirection = 0.2f;

            int frameTime = 0;

            if (absDirection != 0)
            {
                frameTime = (int)(50 / absDirection);
            }

            if (InputReader.MovementInput() && !dashSlide)
            {
                direction.Y = 0;
                direction.X = 0;

                if (InputReader.MovingLeftStick())
                {
                    speed = InputReader.LeftStickLength() * 100;
                    direction.X = InputReader.padState.ThumbSticks.Left.X;
                    direction.Y = -InputReader.padState.ThumbSticks.Left.Y;
                }
                else
                {
                    speed = 100;
                    if (InputReader.MovementLeft())
                        direction.X -= 1;
                    if (InputReader.MovementRight())
                        direction.X += 1;
                    if (InputReader.MovementUp())
                        direction.Y -= 1;
                    if (InputReader.MovementDown())
                        direction.Y += 1;
                }

                if (primaryOrientation == Orientation.Up)
                {
                    sprite.Play(0, 12, frameTime);
                }
                else if (primaryOrientation == Orientation.Down)
                {
                    sprite.Play(1, 12, frameTime);
                }
                else if (primaryOrientation == Orientation.Left)
                {
                    sprite.Play(2, 12, frameTime);
                }
                else
                {
                    sprite.Play(3, 12, frameTime);
                }

                GetOrientation();
            }
            else if (dashSlide)
            {

                if (speed > 0)
                {
                    speed -= deltaTime * 300; // <-- increase time coefficient to make slide stop faster, decrease to slide longer
                }

                dashSlideTimer += deltaTime;
                if (dashSlideTimer >= maxDashSlideTime)
                {
                    dashSlideTimer = 0;
                    dashSlide = false;
                }

                SlideyMovement(deltaTime, 2);
            }
            else
            {
                speed = 0;
                direction = Vector2.Zero;
            }
            

            //This normalizes the direction Vector so that movement is consistent in all directions. If it normalizes a Vector of 0,0 it gets fucky though
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
                prevDirection = direction;
            }

            CheckMovement(deltaTime);

            LevelManager.CheckGate(this);
            


            Vector2 velocity = direction * speed;
            float playerVelocityShort = velocity.Length();

            float absDirShort = (float)Math.Round(absDirection, 2);
            float dirXshort = (float)Math.Round(direction.X, 2);
            float dirYshort = (float)Math.Round(direction.Y, 2);
            playerInfoString = absDirShort.ToString() + " || " + frameTime.ToString() + " || " + playerVelocityShort.ToString() + "\n\n\n\n\n\n\n Dir X: " + dirXshort + "\n Dir Y: " + dirYshort;
        }

        public void SlideyMovement(float deltaTime, int timeCoefficient)
        {
            if (InputReader.MovingLeftStick())
            {
                direction.X += InputReader.padState.ThumbSticks.Left.X * deltaTime * timeCoefficient;
                direction.Y -= InputReader.padState.ThumbSticks.Left.Y * deltaTime * timeCoefficient;
            }
            else
            {
                if (InputReader.MovementLeft())
                    direction.X -= 1 * deltaTime * timeCoefficient;
                if (InputReader.MovementRight())
                    direction.X += 1 * deltaTime * timeCoefficient;
                if (InputReader.MovementUp())
                    direction.Y -= 1 * deltaTime * timeCoefficient;
                if (InputReader.MovementDown())
                    direction.Y += 1 * deltaTime * timeCoefficient;
            }

            if (direction != Vector2.Zero)
                direction.Normalize();
        }

        public void SlideStop(float deltaTime, int timeCoefficient)
        {
            if (direction == Vector2.Zero || speed < 0) //no sliding when standing still and pressing dash, and no negative speed however small (will slide slightly backwards)
            {
                speed = 0;
            }
            else if (speed > 0)
            {
                speed -= deltaTime * timeCoefficient; // <-- increase time coefficient to make slide stop faster, decrease to slide longer
            }
            CheckMovement(deltaTime);
            LevelManager.CheckGate(this);
        }

        public void DrawDashAim(SpriteBatch spriteBatch)
        {
            float fullDashRange = dashRange * maxDashFrames;

            Line dashLine = new Line(centerPosition, new Vector2(centerPosition.X + fullDashRange * direction.X, centerPosition.Y + fullDashRange * direction.Y));

            LevelManager.LevelObjectCollision(dashLine, 0);

            Line newDashLine = new Line(centerPosition, new Vector2(dashLine.intersectionPoint.X, dashLine.intersectionPoint.Y));

            Vector2 dashVector = newDashLine.ToVector();

            //Vector2 dashGhost1Pos = new Vector2(centerPosition.X + (dashVector.X * 0.25f) - sprite.size.X / 2, centerPosition.Y + dashVector.Y * 0.25f - sprite.size.Y / 2);
            Vector2 dashGhost2Pos = new Vector2(centerPosition.X + (dashVector.X * 0.5f) - sprite.size.X / 2, centerPosition.Y + dashVector.Y * 0.5f - sprite.size.Y / 2);
            //Vector2 dashGhost3Pos = new Vector2(centerPosition.X + (dashVector.X * 0.75f) - sprite.size.X / 2, centerPosition.Y + dashVector.Y * 0.75f - sprite.size.Y / 2);
            Vector2 finalGhostPos = new Vector2(dashLine.intersectionPoint.X - sprite.size.X / 2, dashLine.intersectionPoint.Y - sprite.size.Y / 2);

            sprite.Frame((int)primaryOrientation - 1, 13);
            //sprite.DrawElsewhere(spriteBatch, dashGhost1Pos, 50);
            sprite.DrawElsewhere(spriteBatch, dashGhost2Pos, 50);
            //sprite.DrawElsewhere(spriteBatch, dashGhost3Pos, 50);

            sprite.Frame((int)primaryOrientation - 1, 4);
            sprite.DrawElsewhere(spriteBatch, finalGhostPos, 120);
        }

        public void DrawGunAim(SpriteBatch spriteBatch)
        {
            AnimatedSprite gunSprite = pistolSprite;

            if (equippedGun == Gun.Railgun)
                gunSprite = railgunSprite;

            aimAngle = GetAim(0);

            if (!shooting)
                gunSprite.Frame((int)primaryOrientation - 1, 0);

            sprite.Frame((int)primaryOrientation - 1, 5);

            gunDirection = new Vector2((float)Math.Sin(aimAngle), (float)Math.Cos(aimAngle));

            DrawLasersight(spriteBatch, gunDirection, TextureManager.cyanPixel, TextureManager.blueDot, random);
        }

        public float GetAim(float offset)
        {
            float angle;

            if (!InputReader.MovingLeftStick())
            {
                angle = (float)Helper.GetAngle(centerPosition, Game1.mousePos, offset);
            }
            else
            {
                angle = InputReader.LeftStickAngle(offset);
            }

            AngleGetOrientation(angle);

            return angle;
        }

        public void RailGunBeam(SpriteBatch spriteBatch, float timePlayed, int frameTime, float beamAngle, Vector2 startPosition)
        {
            Vector2 beamStartPosition = startPosition;

            if (primaryOrientation == Orientation.Left || primaryOrientation == Orientation.Right)
                beamStartPosition = new Vector2(startPosition.X, startPosition.Y - 2);

            Vector2 beamDirection = new Vector2((float)Math.Sin(beamAngle), (float)Math.Cos(beamAngle));

            //if it doesnt work check if gundirection is fucky
            Line beamLine = new Line(beamStartPosition, new Vector2(beamStartPosition.X + 580 * beamDirection.X, beamStartPosition.Y + 580 * beamDirection.Y));

            LevelManager.RailgunCollision(beamLine);

            Line newBeamLine = new Line(beamStartPosition, beamLine.intersectionPoint);
            int beamLength = (int)newBeamLine.Length();

            //Fade out effect, yes it is wacky
            int alpha = (int)(255 - timePlayed * 1000);
            if (alpha > 255)
                alpha = 255;

            Color color = new Color(alpha, alpha, alpha, alpha);

            Rectangle sourceRect = new Rectangle(0, 0, 4, 4);

            for (int i = 21; i < beamLength; i += 4)
            {
                //if (i > timePlayed * 2000) //this fades the beam from back to front, faster the higher the coeficcient
                //{
                //    if (timePlayed > 0.1f)
                //        sourceRect = new Rectangle(1, 0, 2, 4);

                //    Vector2 beamPoint = new Vector2(beamStartPosition.X + i * beamDirection.X, beamStartPosition.Y + i * beamDirection.Y);
                //    spriteBatch.Draw(TextureManager.railgunBeam, beamPoint, sourceRect, color, -beamAngle, new Vector2(2, 2), 1, SpriteEffects.None, 0);
                //}

                if (timePlayed > 0.1f)
                    sourceRect = new Rectangle(1, 0, 2, 4);

                Vector2 beamPoint = new Vector2(beamStartPosition.X + i * beamDirection.X, beamStartPosition.Y + i * beamDirection.Y);
                spriteBatch.Draw(TextureManager.railgunBeam, beamPoint, sourceRect, color, -beamAngle, new Vector2(2, 2), 1, SpriteEffects.None, 0);
            }

            railgunImpactPosition = new Vector2(beamLine.intersectionPoint.X - 16, beamLine.intersectionPoint.Y - 16);
            railgunImpactSprite.Position = railgunImpactPosition;
            railgunImpactSprite.PlayOnce(0, 5, frameTime);
            railgunImpactSprite.DrawRotation(spriteBatch, beamAngle, new Vector2(16, 32));
        }

        public bool DrawDashCloud(SpriteBatch spriteBatch)
        {
            float rotation = (float)Math.Atan2(dashDirection.X, dashDirection.Y);
            rotation -= pi / 2;

            Vector2 origin = new Vector2(41, 12);
            dashCloudSprite.DrawRotation(spriteBatch, rotation, origin);
            bool playAnimation = dashCloudSprite.PlayOnce(0, 13, 50);
            return playAnimation;
        }

        public void Timers(float deltaTime)
        {
            //INVINCIBILTY TIMER
            if (invincible)
            {
                timeInvincible += deltaTime;

                if (timeInvincible >= maxTimeInvincible)
                {
                    invincible = false;
                    timeInvincible = 0;
                }
            }

            //ATTACK TIMER
            if (startAttackTimer)
            {
                timeSinceAttack += deltaTime;

                if (timeSinceAttack >= comboWindow)
                {
                    startAttackTimer = false;
                    canAttack = false;
                    comboCounter = 0;
                    timeSinceAttack = 0;
                }
            }

            //ATTACK COOLDOWN
            if (!canAttack)
            {
                timeSinceAttack += deltaTime;

                if (timeSinceAttack >= attackCooldown)
                {
                    canAttack = true;
                    timeSinceAttack = 0;
                }
            }

            //DASH TIMER
            if (!canDash)
            {
                timeSinceDash += deltaTime;

                if (timeSinceDash >= timeBetweedDashes)
                {
                    canDash = true;
                    timeSinceDash = 0;
                    Audio.PlaySound(Audio.ping);
                }
            }
        }

        public void Reset(Vector2 position)
        {
            this.position = position;
            health = maxHealth;
            ammo = maxAmmo;
            keys.Clear();
            keys.AddRange(ProfileManager.GetKeys());
            hasRailgun = ProfileManager.HasRailgun;
        }
        public void UseHealthPack()
        {
            if (storedHealthPacks > 0 && health < maxHealth)
            {
                storedHealthPacks--;
                Audio.PlaySound(Audio.healing);
                health += 1;
                ParticleManager.NewParticle(ParticleSelection.HealEffect, position + new Vector2(4, 4));
            }
            else
            {
                Audio.PlaySound(Audio.meepmerp);
            }

        }
        public bool HealDamage(int healing)
        {
            if (health >= maxHealth)
            {
                if (storedHealthPacks < maxStoredHealthPacks)
                {
                    storedHealthPacks++;
                    return true;
                }
                else
                {
                    return false;
                }


            }
            else
            {
                Audio.PlaySound(Audio.healing);
                health += healing;
                ParticleManager.NewParticle(ParticleSelection.HealEffect, position + new Vector2(4, 4));
                if (health > maxHealth)
                {
                    health = maxHealth;
                }
                return true;
            }
        }
        public bool AddAmmo(int ammoToAdd)
        {
            if (ammo >= maxAmmo)
            {
                return false;
            }
            else
            {
                Audio.PlaySound(Audio.reload);
                ammo += ammoToAdd;
                if (ammo > maxAmmo)
                {
                    ammo = maxAmmo;
                }
                return true;
            }

        }
        public void EnterRoom(Vector2 position)
        {
            roomEntrancePosition = position;
            this.position = position;
        }
        private void FootstepCalculation()
        {
            if (traveledDistance >= distanceBetweenFootsteps)
            {
                traveledDistance = 0;
                Tile tile = LevelManager.GetTile(footprint.Center.ToVector2());
                if (tile.footsteps != null)
                {
                    tile.footsteps.PlayRandomSound();
                }
            }
        }
        public void EnteredHazard()
        {
            Audio.PlaySound(Audio.splash);
            position = roomEntrancePosition;
            TakeDamage(1, DamageType.Hazard);
            Fadeout.HazardFade();
        }
        public override void TakeDamage(int damage, DamageType soundType)
        {
            if (!invincible && !dashing)
            {
                invincible = true;

                if (soundType == DamageType.melee)
                {
                    Audio.PlaySound(Audio.player_hit);
                }
                else if (soundType == DamageType.laserGun)
                {
                    Audio.PlaySound(Audio.player_hit);
                }
                else if (soundType == DamageType.Hazard)
                {
                    Audio.PlaySound(Audio.player_hit);
                }
                else if (soundType == DamageType.Electricity)
                {
                    Audio.PlaySound(Audio.hitByElectricity);
                }

                base.TakeDamage(damage, soundType);
            }
            
        }

    }
}
