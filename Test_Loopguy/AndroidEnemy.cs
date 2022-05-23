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

        Random random;

        Vector2 targetPosition;
        Vector2 gunDirection;

        float aimAngle;

        int frameTime = 50;
        int meleeRange, maxRange;

        bool isAttacking, isMoving = false;
        bool shooting;
        bool dying;

        bool aiming;

        public AndroidEnemy(Vector2 position)
            : base(position)
        {
            this.position = position;
            footprintOffset = new Point(12, 24);
            frameSize = new Point(32, 32);

            sprite = new AnimatedSprite(TextureManager.androidEnemySheet, frameSize);
            gunSprite = new AnimatedSprite(TextureManager.evilPistolSheet, new Point(32, 34));

            xOffset = 6;
            yOffset = texture.Height;

            maxHealth = 1;
            maxSpeed = 70;

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
            sprite.Position = position;

            targetPosition = EntityManager.player.centerPosition;

            if (dying)
            {
                isNotDying = false; // PlayOnce death animation
            }
            else if (health <= 0)
            {
                isNotDying = true;
            }
            else
            {
                if (aiming)
                {
                    //shoot cooldown
                }
            }


            base.Update(gameTime);
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
            }
            else
            { //if not aiming up, draw gun sprite on top

                sprite.Draw(spriteBatch);

                //GUN
                if (aiming)
                {
                    DrawGunAim(spriteBatch);
                    gunSprite.DrawRotation(spriteBatch, aimAngle, new Vector2(16, 2));
                }
            }
        }

        protected override void AggroBehavior()
        {
            Vector2 toPlayerVector = centerPosition - targetPosition;
            float toPlayerDistance = toPlayerVector.Length();

            if (toPlayerDistance < maxRange && toPlayerDistance > meleeRange && attackCooldownRemaining <= 0)
            {
                RangedAttackBehaviour();
            }
            else if (toPlayerDistance > maxRange)
            {
                //direction vector = towards player
                direction = toPlayerVector;
                direction.Normalize();
                direction *= -1;

                speed = maxSpeed;
            }
            else if (toPlayerDistance < meleeRange)
            {
                MeleeAttackBehaviour();
            }
        }

        public void MeleeAttackBehaviour()
        {

        }

        public void RangedAttackBehaviour()
        {

        }


        public override void Movement(float deltaTime)
        {
            GetOrientation();

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

            base.Movement(deltaTime);
        }

        public void DrawGunAim(SpriteBatch spriteBatch)
        {
            aimAngle = (float)Helper.GetAngle(centerPosition, targetPosition, 0);
            AngleGetOrientation(aimAngle);

            if (!shooting)
                gunSprite.Frame((int)primaryOrientation - 1, 0);

            sprite.Frame((int)primaryOrientation - 1, 5);

            gunDirection = new Vector2((float)Math.Sin(aimAngle), (float)Math.Cos(aimAngle));

            DrawLasersight(spriteBatch, gunDirection, TextureManager.greenPixel, TextureManager.greenDot, random);
        }
    }
}
