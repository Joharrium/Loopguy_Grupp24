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
            base.Update(gameTime);
            healthBar.SetCurrentValue(position, health);
            if(!aggro)
            {
                aggro = InAggroRange();
            }
            
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            Vector2 thing = centerPosition - EntityManager.player.centerPosition;
            thing.Normalize();
            position = thing * knockBackDistance;
        }

        protected bool InAggroRange()
        {
            if (Math.Pow(EntityManager.player.centerPosition.X - centerPosition.X, 2) + Math.Pow(EntityManager.player.centerPosition.Y - centerPosition.Y, 2) == Math.Pow(aggroRange, 2))
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

        }
        //Placeholder klass för testning av EntityManagers listhantering
    }
    class MeleeEnemy : Enemy
    {
        protected int range;
        public MeleeEnemy(Vector2 position) : base(position)
        {

        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
            if(aggro)
            {
                AggroBehavior();
            }
            Movement(deltaTime);
        }

        protected void AggroBehavior()
        {
            Vector2 thing = centerPosition - EntityManager.player.centerPosition;
            thing.Normalize();
            thing *= -1;

            direction = thing;
            speed = maxSpeed;
        }
    }

    class TestEnemy : MeleeEnemy
    {
        
        public TestEnemy(Vector2 position) : base(position)
        {
            this.position = position;
            this.texture = TexMGR.enemyPlaceholder;
            this.maxHealth = 5;
            this.maxSpeed = 20;
            aggroRange = 1600;
            damage = 1;
            knockBackDistance = 2;
            knockBackDuration = 60;
            Init();
            aggro = true;
    }
    }
}
