using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Test_Loopguy.Content;

namespace Test_Loopguy
{
    internal class Player : Character
    {
        private int ammo = 5;
        private int maxAmmo = 5;

        private int storedHealthPacks = 0;
        public int StoredHealth
        {
            get { return storedHealthPacks; }
        }

        private const int maxStoredHealthPacks = 3;


        AnimatedSprite gunSprite;
        AnimatedSprite meleeSprite;
        AnimatedSprite dashCloudSprite;

        Random random = new Random();
        public List<int> keys = new List<int>();

        Vector2 roomEntrancePosition = new Vector2(64, 64);

        //Wtf
        public Vector2 cameraPosition;
        Vector2 gunDirection;
        Vector2 prevDirection;
        Vector2 dashDirection;
        Vector2 dashPosition;

        float traveledDistance = 35;
        float distanceBetweenFootsteps = 35;

        public bool usedGate;
        static public PlayerHealthBar healthBar;
        static public AmmoBar ammoBar;
        static public DashBar dashBar;

        float gunAngle;
        float aimAngle;
        const float pi = (float)Math.PI;

        float deltaTime;

        float timeSinceDash;
        const float timeBetweedDashes = 1f;

        const int meleeRange = 22;

        const int dashRange = 5; //pixels per frame
        const int maxDashFrames = 10; //frames per dash
        int dashFrames;

        public string playerInfoString;

        public bool attacking;
        bool shooting;
        bool canDash;
        public bool dashing;
        bool dashCloud;


        public Player(Vector2 position)
            : base(position)
        {
            sprite = new AnimatedSprite(TextureManager.playerSheet, new Point(32, 32));
            gunSprite = new AnimatedSprite(TextureManager.gunSheet, new Point(64, 64));
            meleeSprite = new AnimatedSprite(TextureManager.meleeFx, new Point(48, 48));
            dashCloudSprite = new AnimatedSprite(TextureManager.dashCloud, new Point(42, 24));

            maxHealth = 5;
            health = maxHealth;

            speed = 100; //pixels per second

            primaryOrientation = Orientation.Down;
            secondaryOrientation = Orientation.Down;
            LoadKeys();
            healthBar = new PlayerHealthBar(maxHealth);
            ammoBar = new AmmoBar(maxAmmo);
            dashBar = new DashBar(100);
            footprint = new Rectangle((int)position.X, (int)position.Y + 24, 8, 8);

            canDash = true;
        }
        
        private void LoadKeys()
        {
            if (File.Exists(@"saves\keys.txt"))
            {
                List<string> lines = new List<string>();
                foreach (string line in System.IO.File.ReadLines(@"saves\keys.txt"))
                {
                    lines.Add(line);
                }

                List<int> keysToAdd = new List<int>();
                foreach(string l in lines)
                {
                    keysToAdd.Add(Int32.Parse(l));
                }
                


                keys = keysToAdd;
            }
        }

        public override void Update(GameTime gameTime)
        {
            hitBox = new Rectangle((int)(position.X + (sprite.size.X * 0.375)), (int)(position.Y + sprite.size.Y / 4), sprite.size.X / 4, sprite.size.Y / 2);
            footprint = new Rectangle((int)position.X + 12, (int)position.Y + 24, 8, 8);

            if(InputReader.KeyPressed(Microsoft.Xna.Framework.Input.Keys.H) || InputReader.ButtonPressed(Microsoft.Xna.Framework.Input.Buttons.Y))
            {
                UseHealthPack();
            }
            
            centerPosition = new Vector2(position.X + sprite.size.X / 2, position.Y + sprite.size.Y / 2);

            if(timeSinceDash != 0)
            {
                dashBar.SetCurrentValue(footprint.Location.ToVector2(), Math.Min((int)(timeSinceDash * 100), 100));
            }
            if(canDash)
            {
                dashBar.SetCurrentValue(footprint.Location.ToVector2(), 100);
            }
            
            healthBar.UpdateBar(health);
            ammoBar.SetCurrentValue(new Vector2(2, 38), ammo);

            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

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

            if (attacking)
            {
                Melee(deltaTime);

                
                //Here is where you would use the MeleeHit method, I think
                //However, keep in mind that the this will run as long as the attack animation runs,
                //which is 200 ms right now (multiple hits will occur)

                //Another way to do it is that the MeleeHit method only runs once per attack,
                //although that would prevent an enemy walking in to the attack animation from taking damage
                //Idk mang

                
            }
            else if (dashing)
            {

            }
            else
            {
                if (InputReader.Aim() || shooting)
                {
                    cameraPosition = centerPosition + gunDirection * 50;
                    Game1.camera.stabilize = true;

                    //SHOOTING
                    if (InputReader.Shoot() && !shooting && ammo > 0)
                    {
                        gunSprite.currentFrame.X = 0;
                        gunSprite.timeSinceLastFrame = 0;

                        Vector2 shotPosition = new Vector2(centerPosition.X + gunDirection.X * 20 - 4, centerPosition.Y + gunDirection.Y * 20 - 6);
                        float shotAngle = aimAngle + pi;
                        Shot shot = new Shot(shotPosition, gunDirection, shotAngle, 300);
                        LevelManager.AddPlayerProjectile(shot);
                        //shots.Add(shot);
                        //Audio.PlaySound(Audio.meepmerp);
                        Audio.lasergun.PlayRandomSound();
                        shooting = true;
                        ammo--; ;
                    }

                    if (shooting)
                    {
                        Shoot(25);
                    }
                }
                else
                {
                    Game1.camera.stabilize = false;

                    if (direction == Vector2.Zero)
                        cameraPosition = centerPosition + prevDirection * 30;
                    else
                        cameraPosition = centerPosition + direction * 30;

                    gunDirection = Vector2.Zero;
                    Movement(deltaTime);
                    //Implement orientation stuff in a higher class "Character"
                    GetOrientation();

                    if (InputReader.Attack() && !attacking)
                    {
                        meleeSprite.currentFrame.X = 0;
                        meleeSprite.timeSinceLastFrame = 0;
                        sprite.currentFrame.X = 0;
                        sprite.timeSinceLastFrame = 0;

                        attacking = true;
                        Audio.PlaySound(Audio.swing);
                    }
                    else if (InputReader.Dash() && canDash)
                    {
                        Audio.PlaySound(Audio.dash);
                        dashPosition = new Vector2(footprint.Center.X - 21, footprint.Center.Y - 12);
                        dashing = true;
                        canDash = false;
                    }
                }
            }

            FootstepCalculation();
            sprite.Position = position;
            gunSprite.Position = new Vector2(position.X - 16, position.Y - 16);
            meleeSprite.Position = new Vector2(position.X - 8, position.Y - 8);

            gunSprite.Update(gameTime);
            meleeSprite.Update(gameTime);
            dashCloudSprite.Update(gameTime);
            sprite.Update(gameTime);
        }

        public void EnterRoom(Vector2 position)
        {
            roomEntrancePosition = position;
            this.position = position;
        }

        private void FootstepCalculation()
        {
            if(traveledDistance >= distanceBetweenFootsteps)
            {
                traveledDistance = 0;
                Tile tile = LevelManager.GetTile(footprint.Center.ToVector2());
                if(tile.footsteps != null)
                {
                    tile.footsteps.PlayRandomSound();
                }
                
            }
        }

        public void EnteredHazard()
        {
            Audio.PlaySound(Audio.splash);
            position = roomEntrancePosition;
            TakeDamage(1);
            Fadeout.HazardFade();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //draw footprint
            spriteBatch.Draw(TextureManager.black_screen, footprint, Color.White);

            if (dashCloud)
            {
                dashCloudSprite.Position = dashPosition;
                dashCloud = DrawDashCloud(spriteBatch);
            }

            if (attacking)
            {
                sprite.Draw(spriteBatch);
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
                        DrawAim(spriteBatch);
                        DrawGun(spriteBatch);
                    }

                    sprite.Draw(spriteBatch);
                }
                else
                { //if not aiming up, draw gun sprite on top

                    sprite.Draw(spriteBatch);

                    if (dashing)
                    {
                        Dash(spriteBatch);
                    }
                    else if (InputReader.Aim() || shooting)
                    {
                        DrawAim(spriteBatch);
                        DrawGun(spriteBatch);
                    }
                }
            }

            //draw hitbox borders
            spriteBatch.Draw(TextureManager.redPixel, new Vector2(hitBox.Left, hitBox.Top), Color.White);
            spriteBatch.Draw(TextureManager.redPixel, new Vector2(hitBox.Right, hitBox.Bottom), Color.White);

            //healthBar.Draw(spriteBatch);

        }

        public void Reset(Vector2 position)
        {
            this.position = position;
            health = maxHealth;
            ammo = maxAmmo;
            keys.Clear(); 
            LoadKeys();
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            Audio.PlaySound(Audio.player_hit);
            if( health <= 0)
            {
                LevelManager.Reset();
            }
        }

        public void UseHealthPack()
        {
            if(storedHealthPacks > 0 && health < maxHealth)
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
                if(storedHealthPacks < maxStoredHealthPacks)
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
                ParticleManager.NewParticle(ParticleSelection.HealEffect, position + new Vector2(4,4));
                if (health > maxHealth)
                {
                    health = maxHealth;
                }
                return true;
            }
        }

        public bool AddAmmo(int ammoToAdd)
        {
            if(ammo >= maxAmmo)
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

        public void Melee(float deltaTime)
        {
            int rowIntPlayer = 6 + (int)primaryOrientation - 1; //Melee sprites are 6 rows down on player sprite sheet
            int rowIntSword = (int)primaryOrientation - 1; //Wow dude??
            int frameTime = 50;

            if (primaryOrientation == Orientation.Up)
            {//UP
                direction.X = 0;
                direction.Y = -1;

                if (secondaryOrientation == Orientation.Left)
                {
                    rowIntPlayer = 8;
                    rowIntSword = 4;
                    direction.X = -1;
                }
                else if (secondaryOrientation == Orientation.Right)
                {
                    rowIntPlayer = 9;
                    rowIntSword = 6;
                    direction.X = 1;
                }

            }
            else if (primaryOrientation == Orientation.Down)
            {//DOWN
                direction.X = 0;
                direction.Y = 1;

                if (secondaryOrientation == Orientation.Left)
                {
                    rowIntPlayer = 8;
                    rowIntSword = 5;
                    direction.X = -1;
                }
                else if (secondaryOrientation == Orientation.Right)
                {
                    rowIntPlayer = 9;
                    rowIntSword = 7;
                    direction.X = 1;
                }
            }
            else if (primaryOrientation == Orientation.Left)
            {//LEFT
                direction.X = -1;
                direction.Y = 0;

                if (secondaryOrientation == Orientation.Up)
                {
                    rowIntSword = 4;
                    direction.Y = -1;
                }
                else if (secondaryOrientation == Orientation.Down)
                {
                    rowIntSword = 5;
                    direction.Y = 1;
                }
            }
            else
            {//RIGHT
                direction.X = 1;
                direction.Y = 0;

                if (secondaryOrientation == Orientation.Up)
                {
                    rowIntSword = 6;
                    direction.Y = -1;
                }
                else if (secondaryOrientation == Orientation.Down)
                {
                    rowIntSword = 7;
                    direction.Y = 1;
                }
            }

            direction.Normalize();

            CheckMovement(deltaTime);

            sprite.Play(rowIntPlayer, 4, frameTime);

            //The PlayOnce method returns false when the animation is done playing!!!
            attacking = meleeSprite.PlayOnce(rowIntSword, 4, frameTime);
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
            sprite.Frame((int)primaryOrientation - 1, 10);

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

        private void CheckMovement(float deltaTime)
        {
            Vector2 futurePosCalc = position + direction * speed * deltaTime;
            Rectangle futureFootPrint = new Rectangle((int)futurePosCalc.X + 12, (int)futurePosCalc.Y + 24, footprint.Width, footprint.Height);

            bool bingus = false;
            bool bongus = false;
            if (LevelManager.LevelObjectCollision(futureFootPrint, 0))
            {
                if(LevelManager.LevelObjectCollision(new Rectangle((int)futurePosCalc.X + 12, (int)position.Y + 24, footprint.Width, footprint.Height), 0))
                {
                    bingus = true;
                }
                if(LevelManager.LevelObjectCollision(new Rectangle((int)position.X + 12, (int)futurePosCalc.Y + 24, footprint.Width, footprint.Height), 0))
                {
                    bongus = true;
                }
                if (!bingus && bongus)
                {
                    traveledDistance += Math.Abs(position.X - futurePosCalc.X);
                    position.X = futurePosCalc.X;

                }


                if (!bongus && bingus)
                {
                    traveledDistance += Math.Abs(position.Y - futurePosCalc.Y);
                    position.Y = futurePosCalc.Y;
                }
            }
            else
            {
                Vector2 delta = position - futurePosCalc;
                traveledDistance += Math.Abs(delta.Length());
                position += direction * speed * deltaTime;
            }
        }

        public override void Movement(float deltaTime)
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

            if (absDirection > 0)
            {
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
            }
            else
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

        public void DrawAim(SpriteBatch spriteBatch)
        {

            if (aimAngle > pi * 1.75f || aimAngle < pi * 0.25f)
                primaryOrientation = Orientation.Down;
            else if (aimAngle < pi * 0.75f)
                primaryOrientation = Orientation.Right;
            else if (aimAngle < pi * 1.25f)
                primaryOrientation = Orientation.Up;
            else
                primaryOrientation = Orientation.Left;

            if (!shooting)
                gunSprite.Frame((int)primaryOrientation - 1, 0);

            sprite.Frame((int)primaryOrientation - 1, 5);

            if (!InputReader.MovingLeftStick())
            {
                aimAngle = (float)Helper.GetAngle(centerPosition, Game1.mousePos, 0);
            }
            else
            {
                aimAngle = InputReader.LeftStickAngle(0);
            }

            gunDirection = new Vector2((float)Math.Sin(aimAngle), (float)Math.Cos(aimAngle));

            //USE LINE COLLISION FOR LASER SIGHT INSTEEEEAAAAAD
            Line laserLine = new Line(centerPosition, new Vector2(centerPosition.X + 580 * gunDirection.X, centerPosition.Y + 580 * gunDirection.Y));
            //how tho

            LevelManager.LevelObjectCollision(laserLine, 9);

            Line newLaserLine = new Line(centerPosition, laserLine.IntersectionPoint);
            Vector2 laserVector = new Vector2(newLaserLine.P2.X - newLaserLine.P1.X, newLaserLine.P2.Y - newLaserLine.P1.Y);
            int laserLength = (int)laserVector.Length();

            for (int i = 16; i < laserLength; i++)
            {
                Vector2 aimPoint = new Vector2(centerPosition.X + i * gunDirection.X, centerPosition.Y + i * gunDirection.Y);
                spriteBatch.Draw(TextureManager.cyanPixel, aimPoint, Helper.RandomTransparency(random, 0, 90));
            }

            //OUTDATED LAGGY CODE BELLOW
            Vector2 dotPos = laserLine.IntersectionPoint;

            //OUTDATED LAGGY CODE BELLOW
            //Vector2 dotPos = centerPosition;

            //for (int i = 16; i < 580; i++)
            //{
            //    Vector2 aimPoint = new Vector2(centerPosition.X + i * gunDirection.X, centerPosition.Y + i * gunDirection.Y);
            //    Rectangle aimRect = new Rectangle(aimPoint.ToPoint(), new Point(1,1));

            //    //Stops laser sight on collision with object
            //    if (LevelManager.LevelObjectCollision(aimRect, 9))
            //    {
            //        break;
            //    }
            //    else
            //    {
            //        dotPos = new Vector2(aimPoint.X + (gunDirection.X * 5) - 1, aimPoint.Y + (gunDirection.Y * 5) - 1.5f);
            //    }

            //    spriteBatch.Draw(TextureManager.cyanPixel, aimPoint, Helper.RandomTransparency(random, 0, 90));
            //}

            spriteBatch.Draw(TextureManager.blueDot, dotPos, Color.White);
        }

        public void Shoot(int frameTime)
        {
            if (aimAngle > pi * 1.75f || aimAngle < pi * 0.25f)
                primaryOrientation = Orientation.Down;
            else if (aimAngle < pi * 0.75f)
                primaryOrientation = Orientation.Right;
            else if (aimAngle < pi * 1.25f)
                primaryOrientation = Orientation.Up;
            else
                primaryOrientation = Orientation.Left;

            shooting = gunSprite.PlayOnce((int)primaryOrientation, 5, frameTime);
        }

        public void DrawGun(SpriteBatch spriteBatch)
        {
            double angleOffset;

            //These are ordered in a way that makes perfect sense, shut up
            if (primaryOrientation == Orientation.Down)
                angleOffset = 0;
            else if (primaryOrientation == Orientation.Right)
                angleOffset = -0.5 * pi;
            else if (primaryOrientation == Orientation.Up)
                angleOffset = -1 * pi;
            else//left
                angleOffset = -1.5 * pi;

            if (!InputReader.MovingLeftStick())
                gunAngle = (float)Helper.GetAngle(centerPosition, Game1.mousePos, angleOffset);
            else
                gunAngle = InputReader.LeftStickAngle((float)angleOffset);


            gunSprite.DrawRotation(spriteBatch, gunAngle);
        }

        public bool MeleeHit(GameObject obj)
        {
            foreach (Vector2 v in LevelManager.GetPointsOfObject(obj))
            {
                if (Vector2.Distance(centerPosition, v) <= meleeRange)
                {

                    float angle = (float)Helper.GetAngle(centerPosition, v, 0);

                    if (primaryOrientation == Orientation.Down)
                    { 
                        if (secondaryOrientation == Orientation.Down)
                        {
                            if (angle >= pi * 1.75f || angle < pi * 0.25f)
                                return true;
                        }
                        else if (secondaryOrientation == Orientation.Left)
                        {
                            if (angle >= pi * 1.5f)
                                return true;
                        }
                        else if (secondaryOrientation == Orientation.Right)
                        {
                            if (angle < pi * 0.5f)
                                return true;
                        }
                    }
                    else if (primaryOrientation == Orientation.Right)
                    { 
                        if (secondaryOrientation == Orientation.Right)
                        {
                            if (angle >= pi * 0.25f && angle < pi * 0.75f)
                                return true;
                        }
                        else if (secondaryOrientation == Orientation.Up)
                        {
                            if (angle >= pi * 0.5f && angle < pi)
                                return true;
                        }
                        else if (secondaryOrientation == Orientation.Down)
                        {
                            if (angle < pi * 0.5f)
                                return true;
                        }
                    }
                    else if (primaryOrientation == Orientation.Up)
                    { 
                        if (secondaryOrientation == Orientation.Up)
                        {
                            if (angle >= pi * 0.75f && angle < pi * 1.25f)
                                return true;
                        }
                        else if (secondaryOrientation == Orientation.Left)
                        {
                            if (angle >= pi && angle < pi * 1.5f)
                                return true;
                        }
                        else if (secondaryOrientation == Orientation.Right)
                        {
                            if (angle >= pi * 0.5f && angle < pi)
                                return true;
                        }

                    }
                    else
                    { //LEFT
                        if (secondaryOrientation == Orientation.Left)
                        {
                            if (angle >= pi * 1.25f && angle < pi * 1.75f)
                                return true;
                        }
                        else if (secondaryOrientation == Orientation.Up)
                        {
                            if (angle >= pi && angle < pi * 1.5f)
                                return true;
                        }
                        else if (secondaryOrientation == Orientation.Down)
                        {
                            if (angle >= pi * 1.5f)
                                return true;
                        }
                    }
                }
            }
            
            

            return false;
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
    }
}
