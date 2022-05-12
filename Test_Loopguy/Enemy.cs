﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

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
        protected bool aggro = false;
        public bool hitDuringCurrentAttack = false;
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
            footprint = new Rectangle((int)position.X, (int)position.Y, 32, 8); //
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
            

            LevelManager.AddEnemyProjectile(new Shot(centerPosition, direction, (float)Helper.GetAngle(centerPosition, EntityManager.player.centerPosition, 0 + Math.PI), 200));
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
            maxRange = 280;
            fleeRange = 80;
            aggroRange = 192;
            damage = 3;
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

        bool isAttacking = false;
        bool directionIsLocked;
        bool isMoving;

        Vector2 attackOrigin;
        Vector2 oldPosition;
        Orientation lockedOrientation;



        public RangedRobotEnemy(Vector2 position) : base(position)
        {
            
            this.position = position;
            this.maxSpeed = 36;
            sprite = new AnimatedSprite(TextureManager.robotEnemySheet, new Point(64, 64));
            sprite.Position = position;
            //attackOrigin = new Vector2(position.X + 25, position.Y + 28);
            maxHealth = 10;
            health = maxHealth;
            //healthBar = new HealthBar(maxHealth);

            minRange = 40;
            maxRange = 100; //för attack
            fleeRange = 20; //dit den flyr innan den börjar attackera
            aggroRange = 192; //när den upptäcker spelaren
            damage = 1;
            knockBackDistance = 180;
            knockBackDuration = 160;
            Init();
            aggro = false;
            attackCooldown = 2000;
            attackCooldownRemaining = 2000;
            accuracy = 0;
            
        }

        protected override void Attack()
        {
            attackCooldownRemaining = attackCooldown;
            Vector2 direction = centerPosition - EntityManager.player.centerPosition;
            direction.X *= (float)Game1.rnd.Next(100 - accuracy, 100 + accuracy) / 100;
            direction.Y *= (float)Game1.rnd.Next(100 - accuracy, 100 + accuracy) / 100;
            direction.Normalize();
            direction *= -1;

            LevelManager.AddEnemyProjectile(new Shot(attackOrigin, direction, (float)Helper.GetAngle(centerPosition, EntityManager.player.centerPosition, 0 + Math.PI), 200));
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

            int frameTime = 50;


            maxSpeed = 0;

            if (!isAttacking)
            {
                lockedOrientation = primaryOrientation;
                isAttacking = true;
            }

            if (lockedOrientation == Orientation.Up)
            {

                if (!sprite.PlayOnce(1, 20, frameTime))
                {
                    Attack();
                    isAttacking = false;
                    attackCooldownRemaining = attackCooldown;
                }

            }
            else if (lockedOrientation == Orientation.Down)
            {
                if (!sprite.PlayOnce(0, 20, frameTime))
                {
                    Attack();
                    isAttacking = false;
                    attackCooldownRemaining = attackCooldown;

                }

            }
            else if (lockedOrientation == Orientation.Left)
            {
                if (!sprite.PlayOnce(2, 20, frameTime))
                {
                    Attack();
                    isAttacking = false;
                    attackCooldownRemaining = attackCooldown;

                }

            }
            else if (lockedOrientation == Orientation.Right)
            {
                if (!sprite.PlayOnce(3, 20, frameTime))
                {
                    Attack();
                    isAttacking = false;
                    attackCooldownRemaining = attackCooldown;
                }

            }
            else if (lockedOrientation == Orientation.Zero)
            {
                isAttacking = false;
            }
        }

        public override void Movement(float deltaTime)
        {
        
            int frameTime = 50;

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
                    if (primaryOrientation == Orientation.Up)
                    {
                        sprite.Frame(11, 1);
                    }
                    else if (primaryOrientation == Orientation.Down)
                    {
                        sprite.Frame(10, 1);
                    }
                    else if (primaryOrientation == Orientation.Right)
                    {
                        sprite.Frame(9, 1);
                    }
                    else if (primaryOrientation == Orientation.Left)
                    {
                        sprite.Frame(8, 1);
                    }
                }

           
            }

            if (!isAttacking)
            {
                maxSpeed = 36;
            }

            if (direction.X < 0 && direction.Y < 0 || direction.X > 0 && direction.Y > 0)
            {
                isMoving = true;
            }
            else if (direction.X == 0 && direction.Y == 0)
            {
                isMoving = false;
            }
  
            base.Movement(deltaTime);
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
