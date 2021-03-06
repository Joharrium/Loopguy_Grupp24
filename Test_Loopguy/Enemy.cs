using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
namespace Test_Loopguy
{
    internal class Enemy : Character
    {
        protected HealthBar healthBar;
        public int damage;

        //range at which enemy will detect player
        protected int aggroRange;
        protected float idleDir;
        protected float idleTime;
        protected int maxSpeed;
        protected int minRange;
        protected int maxRange;

        //Used for healthbar positioning
        protected int xOffset;
        protected int yOffset;

        protected bool aggro = false;
        public bool hitDuringCurrentAttack = false;

        //Used for sending a bool value (using sprite.PlayOnce) to Level.EnemyUpdate, in order to remove enemies after their death animation, double negation bc Sprite.PlayOnce returns false
        public bool isNotDying = true;  //Försökte göra en metod för att motverka redundans i samtliga enemy update(), men work in progress.

        protected int knockBackDistance;
        protected float knockBackDuration;
        protected float knockBackRemaining;
        protected Vector2 knockBackDirection;
        protected float attackCooldown;
        protected float attackCooldownRemaining;
        //doesn't seem to be causing too much performance issues anymore but may be worth keeping around to try and reuse later.
        protected float timeBetweenAICalls;
        
        public Enemy(Vector2 position) : base(position)
        {
            this.position = position;
            health = maxHealth;
            healthBar = new HealthBar(maxHealth);
            footprint = new Rectangle((int)position.X + footprintOffset.X, (int)position.Y + footprintOffset.Y, 32, 8);
        }

        public void Init()
        {
            health = maxHealth;
            healthBar = new HealthBar(maxHealth);
        }

        private void KnockbackCollisionCheck(float deltaTime)
        {
            //same as checkmovement but for knockback
            Vector2 futurePosCalc = position + knockBackDirection * knockBackDistance * deltaTime;
            Rectangle futureFootPrint = new Rectangle((int)futurePosCalc.X + footprintOffset.X, (int)futurePosCalc.Y + footprintOffset.Y, footprint.Width, footprint.Height);

            bool blockX = false;
            bool blockY = false;
            if (!LevelEditor.editingMode)
            {
                if (LevelManager.LevelObjectCollision(futureFootPrint, 0))
                {
                    if (LevelManager.LevelObjectCollision(new Rectangle((int)futurePosCalc.X + footprintOffset.X, (int)position.Y + footprintOffset.Y, footprint.Width, footprint.Height), 0))
                    {
                        blockX = true;
                    }
                    if (LevelManager.LevelObjectCollision(new Rectangle((int)position.X + footprintOffset.X, (int)futurePosCalc.Y + footprintOffset.Y, footprint.Width, footprint.Height), 0))
                    {
                        blockY = true;
                    }
                    if (!blockX && blockY)
                    {
                        traveledDistance += Math.Abs(position.X - futurePosCalc.X);
                        position.X = futurePosCalc.X;

                    }


                    if (!blockY && blockX)
                    {
                        traveledDistance += Math.Abs(position.Y - futurePosCalc.Y);
                        position.Y = futurePosCalc.Y;
                    }
                }
                else
                {
                    Vector2 delta = position - futurePosCalc;
                    traveledDistance += Math.Abs(delta.Length());
                    position += knockBackDirection * knockBackDistance * deltaTime;
                }
            }
            else
            {
                Vector2 delta = position - futurePosCalc;
                traveledDistance += Math.Abs(delta.Length());
                position += knockBackDirection * knockBackDistance * deltaTime;
            }
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
            footprint.Location = position.ToPoint() + footprintOffset;
            healthBar.SetCurrentValue(position + new Vector2(xOffset, yOffset), health);

            if (!aggro)
            {
                //checks if player is in aggro range
                aggro = InAggroRange();
            }
            else
            {
                AggroBehavior(deltaTime);
            }

            if (knockBackRemaining > 0)
            {
                knockBackRemaining -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                KnockbackCollisionCheck(deltaTime);
            }
            else
            {
                Movement(deltaTime);
            }
            attackCooldownRemaining -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

        }

        public override void Movement(float deltaTime)
        {
            CheckMovement(deltaTime);
        }

        public override void TakeDamage(int damage, DamageType soundType)
        {
            aggro = true;
            if(!hitDuringCurrentAttack)
            {
                health -= damage;
                Vector2 distance = centerPosition - EntityManager.player.centerPosition;
                distance.Normalize();

                knockBackDirection = distance;
                knockBackRemaining = knockBackDuration;

            }

            //if (soundType == AttackType.melee)
            //{
            //    Audio.PlaySound(Audio.meleeOnMetal);
            //}

            //if (soundType == AttackType.laserGun)
            //{
            //    Audio.PlaySound(Audio.player_hit);
            //}
        }

        protected bool InAggroRange()
        {
            if ((EntityManager.player.centerPosition - centerPosition).Length() <= aggroRange) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            healthBar.Draw(spriteBatch);
        }

        protected virtual void AggroBehavior(float deltaTime)
        {

        }

        protected virtual void ExplosionAttack()
        {

        }
    }

    class RangedEnemy : Enemy
    {
        protected int minRange;
        protected int maxRange;
        //if flee behavior is not desired this should be able to be set to 0
        protected int fleeRange;
        protected bool fleeing;

        //0 accuracy is fully accurate, 100 will go everywhere
        protected int accuracy;

        public RangedEnemy(Vector2 position) : base(position)
        {
            // :^)
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected virtual void AttackBehavior()
        {
            if(attackCooldownRemaining <= 0)
            {
                Attack();
            }
        }

        protected virtual void Attack()
        {
            attackCooldownRemaining = attackCooldown;
            Vector2 direction = centerPosition - EntityManager.player.centerPosition;
            //randomizes projectile direction somewhat based on accuracy
            direction.X *= (float)Game1.rnd.Next(100 - accuracy, 100 + accuracy) / 100;
            direction.Y *= (float)Game1.rnd.Next(100 - accuracy, 100 + accuracy) / 100;
            direction.Normalize();
            direction *= -1;
            
            LevelManager.AddEnemyProjectile(new Shot(centerPosition, direction, (float)Helper.GetAngle(centerPosition, EntityManager.player.centerPosition, 0 + Math.PI), 200, damage, false));
        }

        protected override void AggroBehavior(float deltaTime)
        {
            Vector2 thing = centerPosition - EntityManager.player.centerPosition;
            if(!fleeing)
            {
                if (thing.Length() < maxRange && thing.Length() > minRange)
                {
                    speed = 0;
                    AttackBehavior();
                }
                else if(thing.Length() > maxRange)
                {
                    thing.Normalize();
                    thing *= -1;

                    direction = thing;
                    speed = maxSpeed;
                }
                else if(thing.Length() < minRange)
                {
                    thing.Normalize();
                    direction = thing;
                    speed = maxSpeed;
                    fleeing = true;
                }
            }
            else
            {
                if(thing.Length() > fleeRange)
                {
                    fleeing = false;
                }
                thing.Normalize();

                direction = thing;
                speed = maxSpeed;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }

    class TestEnemyRanged : RangedEnemy
    {
        public TestEnemyRanged(Vector2 position) : base(position)
        {
            this.position = position;
            footprintOffset = new Point(0, 24);
            this.texture = TextureManager.slimeo;
            frameSize = new Point(texture.Width, texture.Height);
            xOffset = 6;
            yOffset = texture.Height;
            this.maxHealth = 3;
            this.maxSpeed = 36;
            minRange = 40;
            maxRange = 280;
            fleeRange = 80;
            aggroRange = 192;
            damage = 1;
            knockBackDistance = 180;
            knockBackDuration = 160;
            Init();
            aggro = false;
            attackCooldown = 1000;
            attackCooldownRemaining = 500;
            accuracy = 50;
            
        }

        protected override void Attack()
        {
            attackCooldownRemaining = attackCooldown;
            Vector2 direction = centerPosition - EntityManager.player.centerPosition;
            direction.X *= (float)Game1.rnd.Next(100 - accuracy, 100 + accuracy) / 100;
            direction.Y *= (float)Game1.rnd.Next(100 - accuracy, 100 + accuracy) / 100;
            direction.Normalize();
            direction *= -1;

            LevelManager.AddEnemyProjectile(new Shot(centerPosition, direction, (float)Helper.GetAngle(centerPosition, EntityManager.player.centerPosition, 0 + Math.PI), 150, damage, false));
        }

        public override void TakeDamage(int damage, DamageType soundType)
        {
            if (soundType == DamageType.melee)
            {
                Audio.meleeOnFlesh.PlayRandomSound();
            }
            else if (soundType == DamageType.laserGun)
            {
                Audio.PlaySound(Audio.player_hit);
            }

            base.TakeDamage(damage, soundType);
        }

        public override void Update(GameTime gameTime)
        {
            if (health <= 0)
            {
                isNotDying = false; //Change 'false' to sprite.PlayOnce(/*death animation values*/)
            }
            base.Update(gameTime);
        }
    }

    class RangedRobotEnemy : RangedEnemy
    {       
        bool isAttacking = false;
        bool isMoving;
        bool dying;

        int frameTime = 100;
        Vector2 attackOrigin;
        Orientation lockedOrientation;

        public RangedRobotEnemy(Vector2 position) : base(position)  
        {
            this.position = position;
            footprintOffset = new Point(16, 48);
            footprint = new Rectangle((int)(position.X + footprintOffset.X), (int)(position.Y + footprintOffset.Y), 32, 16);
            frameSize = new Point(64, 64);
            xOffset = frameSize.X / 2;
            yOffset = frameSize.Y;
            maxSpeed = 20;
            
            sprite = new AnimatedSprite(TextureManager.robotEnemySheet, frameSize);
            texture = TextureManager.blankbig;
            
            maxHealth = 5;
            minRange = 40;
            maxRange = 120;
            fleeRange = 20; 
            aggroRange = 192;
            damage = 2;
            knockBackDistance = 0;
            knockBackDuration = 0;
            Init();
            aggro = false;
            attackCooldown = 2000;
            attackCooldownRemaining = 2000;
            accuracy = 0;
        }

        protected override void Attack()
        {
            attackCooldownRemaining = attackCooldown;
            Vector2 direction = attackOrigin - EntityManager.player.centerPosition;
            direction.X *= (float)Game1.rnd.Next(100 - accuracy, 100 + accuracy) / 100;
            direction.Y *= (float)Game1.rnd.Next(100 - accuracy, 100 + accuracy) / 100;
            direction.Normalize();
            direction *= -1;
            LevelManager.AddEnemyProjectile(new RobotEnemyShot(attackOrigin, direction, (float)Helper.GetAngle(attackOrigin, EntityManager.player.centerPosition, 0 + Math.PI), 300, damage));
            Audio.PlaySound(Audio.robotEnemyShot);
        }

        protected override void AttackBehavior()
        {
            maxSpeed = 0;

            if (!isAttacking)
            {
                lockedOrientation = primaryOrientation;
                isAttacking = true;
            }
        }

        protected override void AggroBehavior(float deltaTime)
        {
            Vector2 thing = centerPosition - EntityManager.player.centerPosition;
            if (!fleeing)
            {
                if (thing.Length() < maxRange && thing.Length() > minRange && attackCooldownRemaining <= 0)
                {
                    speed = 0;
                    AttackBehavior();
                }
                else if (thing.Length() > maxRange)
                {
                    thing.Normalize();
                    thing *= -1;

                    direction = thing;
                    speed = maxSpeed;
                }
                else if (thing.Length() < minRange)
                {
                    thing.Normalize();

                    direction = thing;
                    speed = maxSpeed;
                    fleeing = true;
                }
            }
            else
            {

                if (thing.Length() > fleeRange)
                {
                    fleeing = false;
                }
                thing.Normalize();

                direction = thing;
                speed = maxSpeed;
            }
        }

        public override void TakeDamage(int damage, DamageType soundType)
        {
            if (soundType == DamageType.melee && !hitDuringCurrentAttack)
            {
                Audio.meleeOnMetal.PlayRandomSound();
                //Audio.PlaySound(Audio.meleeOnMetal1);
            }
            else if (soundType == DamageType.laserGun && !hitDuringCurrentAttack)
            {
                Audio.PlaySound(Audio.player_hit);
            }

            base.TakeDamage(damage, soundType);
        }

        public override void Movement(float deltaTime)
        {
            DirectionGetOrientation();

            if (!isAttacking && !dying)
            {
                if (primaryOrientation == Orientation.Up)
                {
                    sprite.Play(7, 11, frameTime);
                    attackOrigin = new Vector2(position.X + 30, position.Y + 30);
                }
                else if (primaryOrientation == Orientation.Down)
                {
                    sprite.Play(6, 12, frameTime);
                    attackOrigin = new Vector2(position.X + 30, position.Y + 30);
                }
                else if (primaryOrientation == Orientation.Right)
                {
                    sprite.Play(5, 12, frameTime);
                    attackOrigin = new Vector2(position.X + 33, position.Y + 27);
                }
                else if (primaryOrientation == Orientation.Left)
                {
                    sprite.Play(4, 12, frameTime);
                    attackOrigin = new Vector2(position.X + 26, position.Y + 27);
                }

                if (!isMoving)
                {
                    if (lockedOrientation == Orientation.Up)
                    {
                        sprite.Frame(0, 7);
                    }
                    else if (lockedOrientation == Orientation.Down)
                    {
                        sprite.Frame(0, 6);
                    }
                    else if (lockedOrientation == Orientation.Right)
                    {
                        sprite.Frame(0, 5);
                    }
                    else if (lockedOrientation == Orientation.Left)
                    {
                        sprite.Frame(0, 4);
                    }
                }       
            }

            if (!isAttacking)
            {
                maxSpeed = 36;
            }

            if (speed < 0 || speed > 0)
            {
                isMoving = true;
            }
            else if (speed == 0)
            {
                isMoving = false;
            }

            base.Movement(deltaTime);
        }

        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
            sprite.Position = position;
            
            if (dying)
            {
                isNotDying = sprite.PlayOnce(8, 28, 50);
                maxSpeed = 0;
            }
            else if (health <= 0)
            {
                dying = true;
                isAttacking = false;
                sprite.ResetAnimation();
            }
            else
            {
                if (isAttacking)
                {
                    if (lockedOrientation == Orientation.Up)
                    {
                        if (!sprite.PlayOnce(1, 20, frameTime))
                        {
                            Attack();
                            isAttacking = false;
                        }
                    }
                    else if (lockedOrientation == Orientation.Down)
                    {
                        if (!sprite.PlayOnce(0, 20, frameTime))
                        {
                            Attack();
                            isAttacking = false;
                        }

                    }
                    else if (lockedOrientation == Orientation.Left)
                    {
                        if (!sprite.PlayOnce(2, 20, frameTime))
                        {
                            Attack();
                            isAttacking = false;
                        }

                    }
                    else if (lockedOrientation == Orientation.Right)
                    {
                        if (!sprite.PlayOnce(3, 20, frameTime))
                        {
                            Attack();
                            isAttacking = false;
                        }

                    }
                    else if (lockedOrientation == Orientation.Zero)
                    {
                        isAttacking = false;
                    }
                }
            }
            base.Update(gameTime);

            hitBox = new Rectangle((int)position.X + 16, (int)position.Y + 12, sprite.size.X - 32, sprite.size.Y - 24);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
            healthBar.Draw(spriteBatch);

            //base.Draw(spriteBatch);

            spriteBatch.Draw(TextureManager.redPixel, new Vector2(hitBox.Right, hitBox.Top), Color.White);
            spriteBatch.Draw(TextureManager.redPixel, new Vector2(hitBox.Left, hitBox.Bottom), Color.White);
        }
    }

    class MeleeEnemyWeak : Enemy
    {
        int frameTime = 35;
        bool isAttacking, isMoving, attackDone = false;
        AnimatedSprite explosionSprite;
        Point frameSize2;
        Vector2 distBetweenPlrAndEnemy;
        int explosionRange = 6;

        public MeleeEnemyWeak(Vector2 position) : base(position)
        {
            this.position = position;
            footprint = new Rectangle(0, 0, 16, 16);
            footprintOffset = new Point(0, 16);
            frameSize = new Point(16, 32);
            frameSize2 = new Point(64, 64);
            sprite = new AnimatedSprite(TextureManager.smallFastEnemySheet, frameSize);
            explosionSprite = new AnimatedSprite(TextureManager.explosionSheet, frameSize2);
            this.texture = TextureManager.enemyPlaceholder;
            xOffset = 6;
            yOffset = texture.Height;
            this.maxHealth = 1;
            this.maxSpeed = 95;
            aggroRange = 120;
            damage = 1;
            knockBackDistance = 275;
            knockBackDuration = 180;
            Init();
            aggro = false;

            minRange = 6;
            maxRange = 15;
        }

        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
            sprite.Position = position;
            
            if (health <= 0)
            {
                maxSpeed = 0;
                if(!attackDone)
                {
                    isAttacking = true;
                }
                else
                {
                    isAttacking = false;
                }

                distBetweenPlrAndEnemy = centerPosition - EntityManager.player.centerPosition;
                if (distBetweenPlrAndEnemy.Length() < explosionRange && isAttacking && !hitDuringCurrentAttack)
                {
                    EntityManager.player.TakeDamage(2, DamageType.melee);
                    attackDone = true;
                }
                isNotDying = explosionSprite.PlayOnce(0, 16, frameTime);
                explosionSprite.Position = new Vector2(position.X - 32, position.Y - 32);
                explosionSprite.Update(gameTime);
            }
            base.Update(gameTime);
        }

        protected override void AggroBehavior(float deltaTime)
        {
            distBetweenPlrAndEnemy = centerPosition - EntityManager.player.centerPosition;

            if (distBetweenPlrAndEnemy.Length() <= minRange)
            {
                health = 0;
            }
            else if (distBetweenPlrAndEnemy.Length() > minRange)
            {
                distBetweenPlrAndEnemy.Normalize();
                distBetweenPlrAndEnemy *= -1;
                direction = distBetweenPlrAndEnemy;
                speed = maxSpeed;
            }
        }

        public override void Movement(float deltaTime)
        {
            DirectionGetOrientation();
            if (!isAttacking)
            {
                isMoving = true;
                if (primaryOrientation == Orientation.Up)
                {
                    sprite.Play(1,3, frameTime);
                }
                if (primaryOrientation == Orientation.Down)
                {
                    sprite.Play(3, 3, frameTime);
                }
                if (primaryOrientation == Orientation.Left)
                {
                    sprite.Play(5, 3, frameTime);
                }
                if (primaryOrientation == Orientation.Right)
                {
                    sprite.Play(6, 3, frameTime);
                }
            }

            if (!isMoving)
            {
                if (primaryOrientation == Orientation.Up)
                {
                    sprite.Frame(3, 2);
                }
                if (primaryOrientation == Orientation.Down)
                {
                    sprite.Frame(3, 2);
                }
                if (primaryOrientation == Orientation.Left)
                {
                    sprite.Frame(3, 2);
                }
                if (primaryOrientation == Orientation.Right)
                {
                    sprite.Frame(3, 2);
                }
            }
            base.Movement(deltaTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (health > 0)
            {
                sprite.Draw(spriteBatch);
                healthBar.Draw(spriteBatch);
            }
            explosionSprite.Draw(spriteBatch);
        }
    }   
}