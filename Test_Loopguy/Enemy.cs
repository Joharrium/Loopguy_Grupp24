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
        protected int aggroRange;
        protected float idleDir;
        protected float idleTime;
        protected int maxSpeed;

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

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
            footprint.Location = position.ToPoint() + footprintOffset;
            healthBar.SetCurrentValue(position + new Vector2(xOffset, yOffset), health);

            //if(timeBetweenAICalls < 0)
            //{
                if (!aggro)
                {
                    aggro = InAggroRange();
                }
                else
                {
                    AggroBehavior();
                }
                //timeBetweenAICalls = 0.7f;
            //}

            //timeBetweenAICalls -= deltaTime;

            if (knockBackRemaining > 0)
            {
                knockBackRemaining -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                position += knockBackDirection * knockBackDistance * deltaTime;
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

        public override void TakeDamage(int damage)
        {
            aggro = true;
            if(!hitDuringCurrentAttack)
            {
                health -= damage;
                Vector2 thing = centerPosition - EntityManager.player.centerPosition;
                thing.Normalize();

                knockBackDirection = thing;
                knockBackRemaining = knockBackDuration;

                //position += thing * knockBackDistance;
            }
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

        protected virtual void AggroBehavior()
        {

        }
    }

    class MeleeEnemy : Enemy
    {
        protected int range;

        public MeleeEnemy(Vector2 position) : base(position)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (health <= 0)
            {
                isNotDying = false; //Change 'false' to sprite.PlayOnce(/*death animation values*/)
            }
            base.Update(gameTime);
            //Movement(deltaTime);
        }

        protected override void AggroBehavior()
        {
            Vector2 distBetweenPlrAndEnemy = centerPosition - EntityManager.player.centerPosition;
            Debug.WriteLine(distBetweenPlrAndEnemy.ToString());
            distBetweenPlrAndEnemy.Normalize();
            distBetweenPlrAndEnemy *= -1;

            direction = distBetweenPlrAndEnemy;
            speed = maxSpeed;

        }

        public virtual void MeleeAttack()
        {
            /// Pseudo code:
            /// if enemy is close enough to player:
            /// make melee attack and then check if he is still close enough
            /// if he is, make another attack
            /// dont forget to have timer between attack so it dosent destroy player.
        }
    }

    class RangedEnemy : Enemy
    {
        protected int minRange;
        protected int maxRange;
        protected int fleeRange;
        protected bool fleeing;

        //0 accuracy is fully accurate, 100 will go everywhere
        protected int accuracy;

        //protected projectile 
        
        //shit idk

        public RangedEnemy(Vector2 position) : base(position)
        {

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
            direction.X *= (float)Game1.rnd.Next(100 - accuracy, 100 + accuracy) / 100;
            direction.Y *= (float)Game1.rnd.Next(100 - accuracy, 100 + accuracy) / 100;
            direction.Normalize();
            direction *= -1;
            

            LevelManager.AddEnemyProjectile(new Shot(centerPosition, direction, (float)Helper.GetAngle(centerPosition, EntityManager.player.centerPosition, 0 + Math.PI), 200, damage));
        }

        protected override void AggroBehavior()
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
            this.texture = TextureManager.enemyPlaceholder;
            frameSize = new Point(texture.Width, texture.Height);
            xOffset = 6;
            yOffset = texture.Height;
            this.maxHealth = 4;
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
            attackCooldown = 620;
            attackCooldownRemaining = 320;
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

            LevelManager.AddEnemyProjectile(new Shot(centerPosition, direction, (float)Helper.GetAngle(centerPosition, EntityManager.player.centerPosition, 0 + Math.PI), 150, damage));
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
            damage = 3;
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
        }

        protected override void AggroBehavior()
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

        protected override void AttackBehavior()
        {
            maxSpeed = 0;

            if (!isAttacking)
            {
                lockedOrientation = primaryOrientation;
                isAttacking = true;
            }
        }

        public override void Movement(float deltaTime)
        {

            GetOrientation();

            if (!isAttacking)
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
                        sprite.Frame(0, 11);
                    }
                    else if (lockedOrientation == Orientation.Down)
                    {
                        sprite.Frame(0, 10);
                    }
                    else if (lockedOrientation == Orientation.Right)
                    {
                        sprite.Frame(0, 9);
                    }
                    else if (lockedOrientation == Orientation.Left)
                    {
                        sprite.Frame(0, 8);
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

            if (health <= 0)
            {
                isNotDying = sprite.PlayOnce(12, 23, 50);
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
            healthBar.Draw(spriteBatch);
 
            if (isAttacking)
            {
                Line enemyLaserLine = new Line(attackOrigin, EntityManager.player.position);

                LevelManager.LevelObjectCollision(enemyLaserLine, 9);

                Line newEnemyLaserLine = new Line(attackOrigin, enemyLaserLine.IntersectionPoint);
                Vector2 laserVector = new Vector2(newEnemyLaserLine.P2.X - newEnemyLaserLine.P1.X, newEnemyLaserLine.P2.Y - newEnemyLaserLine.P1.Y);
                int laserLength = (int)laserVector.Length();

                Random rnd = new Random();
                for (int i = 16; i < laserLength; i++)
                {
                    spriteBatch.Draw(TextureManager.cyanPixel, EntityManager.player.position, Color.White);
                }
            }
            //base.Draw(spriteBatch);
        }
    }

    class TestEnemy : MeleeEnemy
    {
        
        public TestEnemy(Vector2 position) : base(position)
        {
            this.position = position;
            footprintOffset = new Point(0, 24);
            this.texture = TextureManager.enemyPlaceholder;
            xOffset = 6;
            yOffset = texture.Height;
            this.maxHealth = 5;
            this.maxSpeed = 40;
            aggroRange = 176;
            damage = 1;
            knockBackDistance = 160;
            knockBackDuration = 160;
            Init();
            aggro = false;
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
}
