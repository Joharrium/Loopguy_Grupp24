using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    internal class Shot : MovingObject
    {
        AnimSprite sprite;
        float rotation;

        public Shot(Vector2 position, Vector2 direction, float angle) :
            base(position)
        {
            this.position = position;
            this.direction = direction;
            
            rotation = angle;

            sprite = new AnimSprite(TexMGR.shot, new Point(8, 8));
            sprite.Position = position;

            speed = 200;
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
            sprite.Draw(spriteBatch);
        }

        public void DrawRotation(SpriteBatch spriteBatch)
        {
            sprite.DrawRotation(spriteBatch, rotation);
        }
    }
}
