using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    internal class Enemy : MovingObject
    {
        public AnimSprite wahoo;
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
        public Enemy(Vector2 position) : base(position)
        {
            this.position = position;
            health = maxHealth;
            healthBar = new HealthBar(maxHealth);
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
            healthBar.SetCurrentValue(position + new Vector2(6, texture.Height), health);
            if(!aggro)
            {
                aggro = InAggroRange();
            }
            else
            {
                AggroBehavior();
            }
            if (knockBackRemaining > 0)
            {
                knockBackRemaining -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                position += knockBackDirection * knockBackDistance * deltaTime;
            }
            else
            {
                Movement(deltaTime);
            }
        }

        public virtual void Movement(float deltaTime)
        {
            Vector2 futurePosCalc = position + direction * speed * deltaTime;
            if(LevelManager.LevelObjectCollision(futurePosCalc))
            {
                if (!LevelManager.LevelObjectCollision(new Vector2(futurePosCalc.X, position.Y)))
                {
                    position.X = futurePosCalc.X;
                }


                if (!LevelManager.LevelObjectCollision(new Vector2(position.X, futurePosCalc.Y)))
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

        //protected projectile 
        protected float cooldown;
        //shit idk

        public RangedEnemy(Vector2 position) : base(position)
        {

        }

        protected virtual void AttackBehavior()
        {

        }

        protected virtual void Attack()
        {

        }

        protected override void AggroBehavior()
        {
            Vector2 thing = centerPosition - EntityManager.player.centerPosition;
            if(!fleeing)
            {
                if (thing.Length() < maxRange && thing.Length() > minRange)
                {
                    speed = 0;
                    //Attack
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
    }

    class TestEnemyRanged : RangedEnemy
    {

        public TestEnemyRanged(Vector2 position) : base(position)
        {
            this.position = position;
            this.texture = TexMGR.enemyPlaceholder;
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
        }
    }

    class TestEnemy : MeleeEnemy
    {
        
        public TestEnemy(Vector2 position) : base(position)
        {
            this.position = position;
            this.texture = TexMGR.enemyPlaceholder;
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
