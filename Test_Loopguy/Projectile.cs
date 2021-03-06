using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Test_Loopguy.Content;

namespace Test_Loopguy
{
    internal class Projectile : MovingObject
    {
        protected Random rand;
        protected AnimatedSprite sprite;
        protected float rotation;
        protected ParticleSelection particleType;

        public int damage;

        protected float bounceCooldown;

        public Projectile Clone()
        {//this is used for making a new playerProjectile from a reflected enemyProjectile (in Level class)
            return (Projectile)MemberwiseClone();
        }

        public Projectile(Vector2 position, Vector2 direction, float angle, float speed, int damage) :
            base(position)
        {
            this.position = position;
            this.direction = direction;
            this.speed = speed;

            rand = new Random();

            rotation = angle;

            particleType = ParticleSelection.ShotExplosion;
        }

        public bool CheckCollision(GameObject obj)
        {
            if (obj.hitBox.Intersects(hitBox))
            {
                ParticleManager.NewParticle(particleType, position);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.DrawRotation(spriteBatch, rotation);
        }

        public void Bounce(float angle)
        {
            rotation = angle;

            //code bellow is for adding a slight random deviation to bounce, probably won't use
            //float randDeviation = (float)rand.NextDouble() - 0.5f;
            //rotation += (float)Math.PI + randDeviation;

            direction = new Vector2(-(float)Math.Sin(rotation), -(float)Math.Cos(rotation));
        }
    }
    internal class Shot : Projectile
    {

        public Shot(Vector2 position, Vector2 direction, float angle, float speed, int damage, bool evil) :
            base(position, direction, angle, speed, damage)
        {
            this.damage = damage;

            if (evil)
            {
                sprite = new AnimatedSprite(TextureManager.evilShot, new Point(8, 8));
                particleType = ParticleSelection.ShotExplosion;
            }
            else
            {
                sprite = new AnimatedSprite(TextureManager.shot, new Point(8, 8));
                particleType = ParticleSelection.EvilShotExplosion;
            }
            
            sprite.Position = position;   
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);

            sprite.Position = position;
            sprite.Play(0, 4, 50);
            sprite.Update(gameTime);

            base.Movement(deltaTime);
        }
    }

    
    internal class RobotEnemyShot : Projectile
    {
        public RobotEnemyShot(Vector2 position, Vector2 direction, float angle, float speed, int damage) :
            base(position, direction, angle, speed, damage)
        {
            this.damage = damage;

            sprite = new AnimatedSprite(TextureManager.robotEnemyShot, new Point(9, 9));
            sprite.Position = position;
            particleType = ParticleSelection.ShotExplosion;
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);

            sprite.Position = position;
            sprite.Play(0, 8, 50);
            sprite.Update(gameTime);

            base.Movement(deltaTime);
        }
    }
}
