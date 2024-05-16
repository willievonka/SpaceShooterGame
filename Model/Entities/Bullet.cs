using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace SpaceShooterGame.Model.Entities
{
    public class Bullet : Sprite
    {
        public float timer;
        public Vector2 BulletDirection;

        public Bullet(Texture2D texture) :
            base(texture)
        {
            BulletDirection = new Vector2((float)Math.Cos(Math.PI / 2), -(float)Math.Sin(Math.PI / 2));
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer > LifeDuration)
            {
                IsRemoved = true;
                timer = 0;
            }

            Position += BulletDirection * Speed;

            if (CollisionModel.Bottom < 0) IsRemoved = true;
        }
    }
}
