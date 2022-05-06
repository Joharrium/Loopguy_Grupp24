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

        protected float bounceCooldown;

        public Projectile Clone()
        {//this is used for making a new playerProjectile from a reflected enemyProjectile (in Level class)
            return (Projectile)MemberwiseClone();
        }

        public Projectile(Vector2 position, Vector2 direction, float angle, float speed) :
            base(position)
        {
            this.position = position;
            this.direction = direction;
            this.speed = speed;

            rand = new Random();

            rotation = angle;

            sprite = new AnimatedSprite(TextureManager.shot, new Point(8, 8));
            sprite.Position = position;

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

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);

            sprite.Position = position;
            sprite.Play(0, 4, 50);
            sprite.Update(gameTime);

            base.Movement(deltaTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.DrawRotation(spriteBatch, rotation);
        }

        public void Bounce()
        {
            float randDeviation = (float)rand.NextDouble() - 0.5f;
            rotation += (float)Math.PI + randDeviation;

            direction = new Vector2(-(float)Math.Sin(rotation), -(float)Math.Cos(rotation));
        }
    }
    internal class Shot : Projectile
    {
        public Shot(Vector2 position, Vector2 direction, float angle, float speed) :
            base(position, direction, angle, speed)
        {
            sprite = new AnimatedSprite(TextureManager.shot, new Point(8, 8));
            sprite.Position = position;

            particleType = ParticleSelection.ShotExplosion;
        }
    }
}
