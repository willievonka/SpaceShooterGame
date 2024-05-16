using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace SpaceShooterGame.Model.Entities
{
    public class EnemyBullet : Bullet
    {
        public EnemyBullet(Texture2D texture)
            : base(texture)
        {
            BulletDirection = new Vector2((float)Math.Cos(Math.PI / 2), (float)Math.Sin(Math.PI / 2));
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
        }
    }
}
