using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Test_Loopguy
{
    internal class AndroidEnemy : Enemy
    {
        AnimatedSprite gunSprite;
        AnimatedSprite meleeSprite;

        Vector2 targetPosition;
        Vector2 gunDirection;

        float aimAngle;
        float meleeAttackAngle;

        float meleeTelegraphTime;
        const float meleeTelegraphTimeMax = 0.5f;

        int frameTime = 50;
        int meleeRange, maxRange;

        bool isAttacking, isMoving = false;
        bool dying;

        bool aiming;
        bool shooting;

        bool isInMelee;
        bool meleeAttacking;

        public AndroidEnemy(Vector2 position)
            : base(position)
        {
            this.position = position;
            footprintOffset = new Point(12, 24);
            frameSize = new Point(32, 32);

            sprite = new AnimatedSprite(TextureManager.androidEnemySheet, frameSize);
            gunSprite = new AnimatedSprite(TextureManager.evilPistolSheet, new Point(32, 34));
            meleeSprite = new AnimatedSprite(TextureManager.evilMeleeFx, new Point(50, 50));

            xOffset = sprite.size.X / 2 - 6;
            yOffset = sprite.size.Y;

            maxHealth = 10;
            maxSpeed = 100;

            meleeRange = 40;
            maxRange = 120;

            aggroRange = 175;
            damage = 1;
            knockBackDistance = 160;
            knockBackDuration = 160;


            Init();
            aggro = false;

            dying = false;
        }

        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
            gunSprite.Update(gameTime);
            meleeSprite.Update(gameTime);
            sprite.Position = position;
            gunSprite.Position = position;
            meleeSprite.Position = new Vector2(position.X - 9, position.Y - 9);

            targetPosition = EntityManager.player.centerPosition;

            base.Update(gameTime);
            centerPosition = new Vector2(position.X + sprite.size.X / 2, position.Y + sprite.size.Y / 2);
            hitBox = new Rectangle((int)(position.X + sprite.size.X / 4), (int)(position.Y + sprite.size.Y / 4), sprite.size.X / 2, sprite.size.Y / 2);

            aimAngle = (float)Helper.GetAngle(centerPosition, targetPosition, 0);

            if (dying)
            {
                isNotDying = sprite.PlayOnce(10, 12, 50);
            }
            else if (health <= 0)
            {
                dying = true;
                isInMelee = false;
                sprite.ResetAnimation();
                LevelManager.QueueAddObject(new KeyPickup(position, 66, true));
            }
            else
            {
                if (aiming)
                {
                    //shoot cooldown
                }
            }


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
            healthBar.Draw(spriteBatch);

            if (primaryOrientation == Orientation.Up)
            {//if aiming up, draw character sprite on top

                //GUN
                if (aiming)
                {
                    DrawGunAim(spriteBatch);
                    gunSprite.DrawRotation(spriteBatch, aimAngle, new Vector2(16, 2));
                }

                sprite.Draw(spriteBatch);

                if (isInMelee)
                    meleeSprite.Draw(spriteBatch);
            }
            else
            { //if not aiming up, draw gun sprite on top

                sprite.Draw(spriteBatch);

                if (isInMelee)
                    meleeSprite.Draw(spriteBatch);

                //GUN
                if (aiming)
                {
                    DrawGunAim(spriteBatch);
                    gunSprite.DrawRotation(spriteBatch, aimAngle, new Vector2(16, 2));
                }
            }

            //draw hitbox borders
            spriteBatch.Draw(TextureManager.redPixel, new Vector2(hitBox.Left, hitBox.Top), Color.White);
            spriteBatch.Draw(TextureManager.redPixel, new Vector2(hitBox.Right, hitBox.Bottom), Color.White);

            spriteBatch.Draw(TextureManager.redPixel, centerPosition, Color.White);
        }

        protected override void AggroBehavior(float deltaTime)
        {
            //Vector2 toPlayerVector = new Vector2(centerPosition.X - targetPosition.X, centerPosition.Y - targetPosition.Y);
            Vector2 toPlayerVector = position - EntityManager.player.position;

            float toPlayerDistance = toPlayerVector.Length();

            if (dying)
            {
                direction = Vector2.Zero;
            }
            else if (isInMelee)
            {
                MeleeAttackBehaviour(deltaTime);
            }
            else if (toPlayerDistance < meleeRange)
            {
                isInMelee = true;
            }
            else if (toPlayerDistance < maxRange && toPlayerDistance > meleeRange && attackCooldownRemaining <= 0)
            {
                //direction vector = towards player
                direction = toPlayerVector;
                direction.Normalize();
                direction *= -1;

                speed = maxSpeed;
            }
            else if (toPlayerDistance > maxRange)
            {
                //direction vector = towards player
                direction = toPlayerVector;
                direction.Normalize();
                direction *= -1;

                speed = maxSpeed;
            }
        }

        public void MeleeAttackBehaviour(float deltaTime)
        {
            speed = 0;
            isAttacking = true;

            if (meleeAttacking)
            {
                int rowInt = (int)primaryOrientation + 5;
                meleeAttacking = isInMelee = isAttacking = PlayMelee(deltaTime, meleeSprite, rowInt, 35);
                if (MeleeHit(EntityManager.player))
                {
                    EntityManager.player.TakeDamage(damage, DamageType.melee);
                }
            }
            else
            {
                sprite.Frame(0, (int)primaryOrientation + 5);
                meleeSprite.Frame((int)primaryOrientation - 1, 8);

                meleeTelegraphTime += deltaTime;
                if (meleeTelegraphTime >= meleeTelegraphTimeMax)
                {
                    sprite.ResetAnimation();
                    meleeSprite.ResetAnimation();
                    meleeTelegraphTime = 0;
                    meleeAttacking = true;
                }
                else if (meleeTelegraphTime < meleeTelegraphTimeMax / 2)
                    AngleGetOrientation(aimAngle);
            }
        }

        public void RangedAttackBehaviour()
        {

        }


        public override void Movement(float deltaTime)
        {
            int frameTime = 50; 

            if (!isAttacking)
                GetOrientation();

            if (!dying)
            {
                if (!isAttacking)
                {
                    isMoving = true;
                    if (primaryOrientation == Orientation.Up)
                    {
                        sprite.Play(0, 12, frameTime);
                    }
                    if (primaryOrientation == Orientation.Down)
                    {
                        sprite.Play(1, 12, frameTime);
                    }
                    if (primaryOrientation == Orientation.Left)
                    {
                        sprite.Play(2, 12, frameTime);
                    }
                    if (primaryOrientation == Orientation.Right)
                    {
                        sprite.Play(3, 12, frameTime);
                    }
                }

                if (!isMoving)
                {
                    if (primaryOrientation == Orientation.Up)
                    {
                        sprite.Frame(4, 0);
                    }
                    if (primaryOrientation == Orientation.Down)
                    {
                        sprite.Frame(4, 1);
                    }
                    if (primaryOrientation == Orientation.Left)
                    {
                        sprite.Frame(4, 2);
                    }
                    if (primaryOrientation == Orientation.Right)
                    {
                        sprite.Frame(4, 3);
                    }
                }
            }
            
            base.Movement(deltaTime);
        }

        public void DrawGunAim(SpriteBatch spriteBatch)
        {
            AngleGetOrientation(aimAngle);

            if (!shooting)
                gunSprite.Frame((int)primaryOrientation - 1, 0);

            sprite.Frame((int)primaryOrientation - 1, 5);

            gunDirection = new Vector2((float)Math.Sin(aimAngle), (float)Math.Cos(aimAngle));

            DrawLasersight(spriteBatch, gunDirection, TextureManager.greenPixel, TextureManager.greenDot, Game1.rnd);
        }

        public bool PlayMelee(float deltaTime, AnimatedSprite meleeSprite, int rowIntCharacterSprite, int frameTime)
        {
            speed = 50;

            int rowIntPlayer = rowIntCharacterSprite;
            int rowIntSword = (int)primaryOrientation - 1; //Wow dude??

            if (primaryOrientation == Orientation.Up)
            {//UP
                direction.X = 0;
                direction.Y = -1;

                if (secondaryOrientation == Orientation.Left)
                {
                    rowIntSword = 4;
                    rowIntPlayer = 8;

                    direction.X = -1;
                }
                else if (secondaryOrientation == Orientation.Right)
                {
                    rowIntSword = 6;
                    rowIntPlayer = 9;

                    direction.X = 1;
                }

            }
            else if (primaryOrientation == Orientation.Down)
            {//DOWN
                direction.X = 0;
                direction.Y = 1;

                if (secondaryOrientation == Orientation.Left)
                {
                    rowIntSword = 5;
                    rowIntPlayer = 8;

                    direction.X = -1;
                }
                else if (secondaryOrientation == Orientation.Right)
                {
                    rowIntSword = 7;
                    rowIntPlayer = 9;

                    direction.X = 1;
                }
            }
            else if (primaryOrientation == Orientation.Left)
            {//LEFT
                direction.X = -1;
                direction.Y = 0;

                rowIntPlayer = 8;

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

                rowIntPlayer = 9;
                if (secondaryOrientation == Orientation.Up)
                {
                    rowIntSword = 6;
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
            return meleeSprite.PlayOnce(rowIntSword, 4, frameTime);
        }
    }
}
