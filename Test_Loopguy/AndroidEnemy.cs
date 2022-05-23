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
        int frameTime = 50;
        bool isAttacking, isMoving = false;

        public AndroidEnemy(Vector2 position)
            : base(position)
        {
            this.position = position;
            footprintOffset = new Point(12, 24);
            frameSize = new Point(32, 32);
            sprite = new AnimatedSprite(TextureManager.androidEnemySheet, frameSize);
            this.texture = TextureManager.enemyPlaceholder;
            xOffset = 6;
            yOffset = texture.Height;
            this.maxHealth = 1;
            this.maxSpeed = 95;
            aggroRange = 175;
            damage = 1;
            knockBackDistance = 160;
            knockBackDuration = 160;
            Init();
            aggro = false;
        }

        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
            sprite.Position = position;
            if (health <= 0)
            {
                isNotDying = false; //Change 'false' to sprite.PlayOnce(/*death animation values*/)
            }
            base.Update(gameTime);
        }

        protected override void AggroBehavior()
        {
            Vector2 distBetweenPlrAndEnemy = centerPosition - EntityManager.player.centerPosition;
            distBetweenPlrAndEnemy.Normalize();
            distBetweenPlrAndEnemy *= -1;

            direction = distBetweenPlrAndEnemy;
            speed = maxSpeed;
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
            healthBar.Draw(spriteBatch);
        }

    }
}
