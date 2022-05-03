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
        protected AnimatedSprite sprite;
        protected float rotation;
        protected ParticleSelection particleType;

        public Projectile(Vector2 position, Vector2 direction, float angle) :
            base(position)
        {
            this.position = position;
            this.direction = direction;

            rotation = angle;

            sprite = new AnimatedSprite(TextureManager.shot, new Point(8, 8));
            sprite.Position = position;

            speed = 300;
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
    }
    internal class Shot : Projectile
    {
        public Shot(Vector2 position, Vector2 direction, float angle) :
            base(position, direction, angle)
        {
            sprite = new AnimatedSprite(TextureManager.shot, new Point(8, 8));
            sprite.Position = position;

            speed = 300;
            particleType = ParticleSelection.ShotExplosion;
        }
    }
}
