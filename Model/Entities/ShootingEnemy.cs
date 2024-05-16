using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace SpaceShooterGame.Model.Entities
{
    public class ShootingEnemy : Enemy
    {
        private readonly float shoodSpeed;
        private float shootPeriod;
        public EnemyBullet EnemyBullet;
        public Vector2 BulletDirection;

        public ShootingEnemy(Texture2D texture)
            : base(texture)
        {
            var rndValue = new Random();
            Position = new Vector2(rndValue.Next(0, Game1.ScreenWidth - Texture.Width), -Texture.Height);
            Speed = rndValue.Next(3, 6);
            shoodSpeed = (float)rndValue.Next(1, 3) - 0.1f;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            shootPeriod += (float)gameTime.ElapsedGameTime.TotalSeconds;

            CheckOnCollisions(sprites);

            Position.Y += Speed;

            if (shootPeriod > shoodSpeed)
            {
                shootPeriod = 0;
                AddEnemyBullet(sprites);
            }

            if (CollisionModel.Top > Game1.ScreenHeight) IsRemoved = true;
        }

        private void CheckOnCollisions(List<Sprite> sprites)
        {
            foreach (var sprite in sprites)
            {
                if (sprite is Enemy) continue;
                if (sprite is EnemyBullet) continue;
                if (sprite.CollisionModel.Intersects(CollisionModel))
                {
                    IsDead = true;
                }
            }
        }

        private void AddEnemyBullet(List<Sprite> sprites)
        {
            var enemyBullet = EnemyBullet.Clone() as EnemyBullet;
            enemyBullet.Position.X = Position.X + 15;
            enemyBullet.Position.Y = Position.Y;
            enemyBullet.Speed = 15;
            enemyBullet.LifeDuration = 2f;

            sprites.Add(enemyBullet);
        }
    }
}
