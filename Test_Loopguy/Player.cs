using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Test_Loopguy
{
    internal class Player : MovingObject
    {
        public int health = 5;
        private int maxHealth = 5;

        private int ammo = 5;
        private int maxAmmo = 5;
        AnimatedSprite sprite;
        AnimatedSprite gunSprite;
        AnimatedSprite meleeSprite;

        Random random = new Random();
        public List<int> keys = new List<int>();

        //Wtf
        public Vector2 cameraPosition;
        Vector2 gunDirection;
        Vector2 prevDirection;

        public bool usedGate;
        static public PlayerHealthBar healthBar;
        static public AmmoBar ammoBar;

        float gunAngle;
        float aimAngle;
        const float pi = (float)Math.PI;

        int dirInt;
        const int meleeRange = 22;
        const int dashRange = 40;

        public string playerInfoString;

        public bool attacking;
        bool shooting;
        bool dashing;

        //unused I think so should be cleaned up
        List<Shot> shots;

        //footprint rectangle
        Rectangle footprint;

        public Player(Vector2 position)
            : base(position)
        {
            sprite = new AnimatedSprite(TextureManager.playerSheet, new Point(32, 32));
            gunSprite = new AnimatedSprite(TextureManager.gunSheet, new Point(64, 64));
            meleeSprite = new AnimatedSprite(TextureManager.meleeFx, new Point(48, 48));

            speed = 100;

            dirInt = 2;
            LoadKeys();
            healthBar = new PlayerHealthBar(5);
            ammoBar = new AmmoBar(5);
            footprint = new Rectangle((int)position.X, (int)position.Y + 24, 8, 8);
            
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

            shots = new List<Shot>();
        }

        public override void Update(GameTime gameTime)
        {
            hitBox = new Rectangle((int)position.X, (int)position.Y, sprite.size.X, sprite.size.Y);
            footprint = new Rectangle((int)position.X + 4, (int)position.Y + 24, 8, 8);
            
            centerPosition = new Vector2(position.X + sprite.size.X / 2, position.Y + sprite.size.Y / 2);

            healthBar.UpdateBar(health);
            ammoBar.SetCurrentValue(new Vector2(2, 40), ammo);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

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
                dashing = false;
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
                        Shot shot = new Shot(shotPosition, gunDirection, shotAngle);
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

                    if (InputReader.Attack() && !attacking)
                    {
                        meleeSprite.currentFrame.X = 0;
                        meleeSprite.timeSinceLastFrame = 0;
                        sprite.currentFrame.X = 0;
                        sprite.timeSinceLastFrame = 0;

                        attacking = true;
                        Audio.PlaySound(Audio.swing);
                    }
                    else if (InputReader.Dash())
                    {
                        Audio.PlaySound(Audio.dash);
                        dashing = true;
                    }
                }
            }

            foreach (Shot shot in shots)
            {
                shot.Update(gameTime);
            }

            sprite.Position = position;
            gunSprite.Position = new Vector2(position.X - 16, position.Y - 16);
            meleeSprite.Position = new Vector2(position.X - 8, position.Y - 8);

            gunSprite.Update(gameTime);
            meleeSprite.Update(gameTime);
            sprite.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (attacking)
            {
                sprite.Draw(spriteBatch);
                meleeSprite.Draw(spriteBatch);
            }
            else
            {
                if (dirInt == 1)
                { //if aiming up, draw player sprite on top

                    if (InputReader.Aim() || shooting)
                    {
                        DrawAim(spriteBatch);
                        DrawGun(spriteBatch);
                    }
                    else if (dashing)
                    {
                        Dash(spriteBatch);
                    }

                    sprite.Draw(spriteBatch);
                }
                else
                { //if not aiming up, draw gun sprite on top

                    sprite.Draw(spriteBatch);

                    if (InputReader.Aim() || shooting)
                    {
                        DrawAim(spriteBatch);
                        DrawGun(spriteBatch);
                    }
                    else if (dashing)
                    {
                        Dash(spriteBatch);
                    }
                }
            }

            foreach(Shot shot in shots)
            {
                shot.DrawRotation(spriteBatch);
            }

            //healthBar.Draw(spriteBatch);
        }

        public void Reset(Vector2 position)
        {
            this.position = position;
            health = maxHealth; 
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

        public bool HealDamage(int healing)
        {
            if (health >= maxHealth)
            {
                return false;

            }
            else
            {
                Audio.PlaySound(Audio.healing);
                health += healing;
                if (health > maxHealth)
                {
                    health = maxHealth;
                }
                return true;
            }
        }

        public void AddAmmo(int ammoToAdd)
        {
            ammo += ammoToAdd;
            if(ammo > maxAmmo)
            {
                ammo = maxAmmo;
            }
        }

        public void Melee(float deltaTime)
        {
            int rowInt = dirInt - 1; //Wow dude??
            int frameTime = 50;

            if (dirInt == 1)
            {//UP
                sprite.Play(6, 4, frameTime);
                direction.X = 0;
                direction.Y = -1;
            }
            else if (dirInt == 2)
            {//DOWN
                sprite.Play(7, 4, frameTime);
                direction.X = 0;
                direction.Y = 1;
            }
            else if (dirInt == 3)
            {//LEFT
                sprite.Play(8, 4, frameTime);
                direction.X = -1;
                direction.Y = 0;
            }
            else
            {//RIGHT
                sprite.Play(9, 4, frameTime);
                direction.X = 1;
                direction.Y = 0;
            }

            Vector2 futurepos = centerPosition + direction * speed * deltaTime + new Vector2(0, 12);
            Rectangle futureFootPrint = new Rectangle((int)futurepos.X, (int)futurepos.Y + 24, footprint.Width, footprint.Height);

            if (LevelManager.LevelObjectCollision(futureFootPrint, 0) || LevelManager.WallCollision(futurepos))
            {

            }
            else
            {
                position += direction * speed / 2 * deltaTime;
            }
            
            //The PlayOnce method returns false when the animation is done playing!!!
            attacking = meleeSprite.PlayOnce(rowInt, 4, frameTime);
        }

        public void Dash(SpriteBatch spriteBatch)
        {
            Vector2 viablePos = centerPosition + new Vector2(0, 12);

            if(direction == Vector2.Zero)
            {
                if (dirInt == 1)
                    direction.Y = -1;
                else if (dirInt == 2)
                    direction.Y = 1;
                else if (dirInt == 3)
                    direction.X = -1;
                else
                    direction.X = 1;
            }

            for (int i = 1; i < dashRange + 1; i++)
            {
                Vector2 dashPos = new Vector2(centerPosition.X + i * direction.X, centerPosition.Y + i * direction.Y) + new Vector2(0, 12);
                Rectangle futureFootPrint = new Rectangle((int)dashPos.X, (int)dashPos.Y, footprint.Width, footprint.Height);


                if (LevelManager.LevelObjectCollision(futureFootPrint, 0) || LevelManager.WallCollision(dashPos))
                {
                    break;
                }
                else
                {
                    sprite.DrawElsewhere(spriteBatch, new Vector2(dashPos.X - sprite.size.X / 2, dashPos.Y - sprite.size.Y / 2 - 12));
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
                    position.X = futurePosCalc.X;
                }


                if (!bongus && bingus)
                {
                    position.Y = futurePosCalc.Y;
                }
            }
            else
            {
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
            float absDirectionX = Math.Abs(direction.X);
            float absDirectionY = Math.Abs(direction.Y);


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

            //Visual changes depending on direction
            if (direction.Y < 0 && absDirectionX < absDirectionY)
            {//UP
                sprite.Play(0, 12, frameTime);
                dirInt = 1;
            }
            else if (direction.Y > 0 && absDirectionX < absDirectionY)
            {//DOWN
                sprite.Play(1, 12, frameTime);
                dirInt = 2;
            }
            else if (direction.X < 0)
            {//LEFT
                sprite.Play(2, 12, frameTime);
                dirInt = 3;
            }
            else if (direction.X > 0)
            {//RIGHT
                sprite.Play(3, 12, frameTime);
                dirInt = 4;
            }
            else
            {
                if (dirInt == 1)
                    sprite.Frame(0, 4);
                else if (dirInt == 2)
                    sprite.Frame(1, 4);
                else if (dirInt == 3)
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
            playerInfoString = absDirShort.ToString() + " || " + frameTime.ToString() + " || " + playerVelocityShort.ToString();
        }
        
        public void DrawAim(SpriteBatch spriteBatch)
        {

            if (aimAngle > pi * 1.75f || aimAngle < pi * 0.25f)
            {//DOWN
                dirInt = 2;
            }
            else if (aimAngle < pi * 0.75f)
            {//RIGHT
                dirInt = 4;
            }
            else if (aimAngle < pi * 1.25f)
            {//UP
                dirInt = 1;
            }
            else
            {//LEFT
                dirInt = 3;
            }

            if (!shooting)
                gunSprite.Frame(dirInt - 1, 0);

            sprite.Frame(dirInt - 1, 5);

            if (!InputReader.MovingLeftStick())
            {
                aimAngle = (float)Helper.GetAngle(centerPosition, Game1.mousePos, 0);
            }
            else
            {
                aimAngle = InputReader.LeftStickAngle(0);
            }

            gunDirection = new Vector2((float)Math.Sin(aimAngle), (float)Math.Cos(aimAngle));

            Vector2 dotPos = centerPosition;

            for (int i = 16; i < 580; i++)
            {
                Vector2 aimPoint = new Vector2(centerPosition.X + i * gunDirection.X, centerPosition.Y + i * gunDirection.Y);
                Rectangle aimRect = new Rectangle(aimPoint.ToPoint(), new Point(1,1));

                //Stops laser sight on collision with object
                if (LevelManager.LevelObjectCollision(aimRect, 9) || LevelManager.WallCollision(aimPoint))
                {
                    break;
                }
                else
                {
                    dotPos = new Vector2(aimPoint.X + (gunDirection.X * 5) - 1, aimPoint.Y + (gunDirection.Y * 5) - 1.5f);
                }

                spriteBatch.Draw(TextureManager.cyanPixel, aimPoint, Helper.RandomTransparency(random, 0, 90));
            }

            spriteBatch.Draw(TextureManager.blueDot, dotPos, Color.White);
        }

        public void Shoot(int frameTime)
        {
            if (aimAngle > pi * 1.75f || aimAngle < pi * 0.25f)
                dirInt = 2;
            else if (aimAngle < pi * 0.75f)
                dirInt = 4;
            else if (aimAngle < pi * 1.25f)
                dirInt = 1;
            else
                dirInt = 3;

            shooting = gunSprite.PlayOnce(dirInt, 5, frameTime);
        }

        public void DrawGun(SpriteBatch spriteBatch)
        {
            double angleOffset;

            //These are ordered in a way that makes perfect sense, shut up
            if (dirInt == 2)//down
                angleOffset = 0;
            else if (dirInt == 3)//right
                angleOffset = -1.5 * pi;
            else if (dirInt == 1)//up
                angleOffset = -1 * pi;
            else//left
                angleOffset = -0.5 * pi;

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

                    if (dirInt == 2)
                    { //DOWN
                        if (angle >= pi * 1.75f || angle < pi * 0.25f)
                            return true;
                    }
                    else if (dirInt == 4)
                    { //RIGHT
                        if (angle >= pi * 0.25f && angle < pi * 0.75f)
                            return true;
                    }
                    else if (dirInt == 1)
                    { //UP
                        if (angle >= pi * 0.75f && angle < pi * 1.25f)
                            return true;
                    }
                    else
                    { //LEFT
                        if (angle >= pi * 1.25f && angle < pi * 1.75f)
                            return true;
                    }
                }
            }
            
            

            return false;
        }
    }
}
