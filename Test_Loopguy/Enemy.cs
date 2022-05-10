using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    internal class Enemy : MovingObject
    {
        public AnimatedSprite animSprite;
        public int health;
        public int maxHealth;
        protected HealthBar healthBar;
        public int damage;
        protected int aggroRange;
        protected float idleDir;
        protected float idleTime;
        protected int maxSpeed;
        protected bool aggro = false;
        public bool hitDuringCurrentAttack = false;
        protected int knockBackDistance;
        protected float knockBackDuration;
        protected float knockBackRemaining;
        protected Vector2 knockBackDirection;
        protected float attackCooldown;
        protected float attackCooldownRemaining;
        protected float timeBetweenAICalls;
        public Rectangle footprint;
        public Enemy(Vector2 position) : base(position)
        {
            this.position = position;
            health = maxHealth;
            healthBar = new HealthBar(maxHealth);
            footprint = new Rectangle((int)position.X, (int)position.Y, 32, 8);
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
            footprint.Location = position.ToPoint();
            healthBar.SetCurrentValue(position + new Vector2(6, texture.Height), health);

            if(timeBetweenAICalls < 0)
            {
                if (!aggro)
                {
                    aggro = InAggroRange();
                }
                else
                {
                    AggroBehavior();
                }
                timeBetweenAICalls = 0.7f;
            }

            timeBetweenAICalls -= deltaTime;

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

        public virtual void Movement(float deltaTime)
        {
            Vector2 futurePosCalc = position + direction * speed * deltaTime;
            Rectangle futureFootPrintCalc = new Rectangle(footprint.X + (int)futurePosCalc.X, footprint.Y + (int)futurePosCalc.Y, footprint.Width, footprint.Height);
            if(LevelManager.LevelObjectCollision(futureFootPrintCalc, 0))
            {
                if (!LevelManager.LevelObjectCollision(new Rectangle((int)futurePosCalc.X, (int)position.Y, footprint.Width, footprint.Height), 0))
                {
                    position.X = futurePosCalc.X;
                }


                if (!LevelManager.LevelObjectCollision(new Rectangle((int)position.X, (int)futurePosCalc.Y, footprint.Width, footprint.Height), 0))
                {
                    position.Y = futurePosCalc.Y;
                }
            }
            else
            {
                position += direction * speed * deltaTime;
            }
        }

        public void TakeDamage(int damage)
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
            
            base.Update(gameTime);
            //Movement(deltaTime);

        }

        protected override void AggroBehavior()
        {
            Vector2 thing = centerPosition - EntityManager.player.centerPosition;
            thing.Normalize();
            thing *= -1;

            direction = thing;
            speed = maxSpeed;
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


            LevelManager.AddEnemyProjectile(new Shot(centerPosition, direction, (float)((float)Helper.GetAngle(centerPosition, EntityManager.player.centerPosition, 0) + Math.PI)));
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
            this.texture = TextureManager.enemyPlaceholder;
            this.maxHealth = 4;
            this.maxSpeed = 36;
            minRange = 40;
            maxRange = 128;
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
    }

    class RangedRobotEnemy : RangedEnemy
    {
        
        AnimatedSprite sprite;  //Bör läggas in i RangedEnemy I guess
        bool isAttacking;

        public RangedRobotEnemy(Vector2 position) : base(position)
        {
            
            this.position = position;
            this.maxSpeed = 36;
            sprite = new AnimatedSprite(TextureManager.robotEnemySheet, new Point(64, 64));
            sprite.Position = position;
            maxHealth = 10;
            health = maxHealth;
            healthBar = new HealthBar(maxHealth);

            minRange = 128;
            maxRange = 128;
            fleeRange = 128;
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

        public enum FacingDirection
        {
            Right,
            Left,
            Up,
            Down
        }

        public override void Movement(float deltaTime)
        {
            bool turnedRight, turnedLeft, turnedUp, turnedDown;
            


            FacingDirection currentDirection = FacingDirection.Down;

            float absDirection = Math.Abs(direction.X) + Math.Abs(direction.Y);

            int frameTime = 0;

            if (absDirection != 0)
            {
                frameTime = (int)(50 / absDirection);
            }


            if (direction.X > 0)
            {
                sprite.Play(5, 12, frameTime);
                currentDirection = FacingDirection.Right;
            }
            else if (direction.X < 0)
            {
                sprite.Play(4, 12, frameTime);
                currentDirection = FacingDirection.Left;
            }
            else if (direction.Y > 0)
            {
                sprite.Play(7, 11, frameTime);
                currentDirection = FacingDirection.Up;
            }
            else if (direction.Y < 0)
            {
                sprite.Play(6, 12, frameTime);
                currentDirection = FacingDirection.Down;
            }
            else if (direction.X == 0 && currentDirection == FacingDirection.Down)
            {

            }

            if (minRange == 128)
            {
             
                isAttacking = sprite.PlayOnce(2, 20, frameTime);
            }


            if (isAttacking)
            {
                maxSpeed = 0;
            }



            base.Movement(deltaTime);
        }

        protected override void AggroBehavior()
        {
            Vector2 thing = centerPosition - EntityManager.player.centerPosition;
            thing.Normalize();
            thing *= -1;

            direction = thing;
            speed = maxSpeed;
        }

        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
            sprite.Position = position;


            //base.Draw(spriteBatch);
        }


    }

    class TestEnemy : MeleeEnemy
    {
        
        public TestEnemy(Vector2 position) : base(position)
        {
            this.position = position;
            this.texture = TextureManager.enemyPlaceholder;
            this.maxHealth = 5;
            this.maxSpeed = 40;
            aggroRange = 176;
            damage = 1;
            knockBackDistance = 160;
            knockBackDuration = 160;
            Init();
            aggro = false;
        }
    }

    
}
