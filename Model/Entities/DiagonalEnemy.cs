using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace SpaceShooterGame.Model.Entities
{
    public class DiagonalEnemy : ShootingEnemy
    {
        private readonly float shoodSpeed;
        public bool IsLeft { get; set; }
        private float shootPeriod;
        public EnemyBullet DiagonalBullet;

        public DiagonalEnemy(Texture2D texture, bool turner)
            : base(texture)
        {
            var rndValue = new Random();
            switch (turner)
            {
                case true:
                    {
                        Position = new Vector2(0, rndValue.Next(0, Game1.ScreenHeight / 2 - Texture.Height));
                        break;
                    }
                case false:
                    {
                        Position = new Vector2(Game1.ScreenWidth - Texture.Width,
                            rndValue.Next(0, Game1.ScreenHeight / 2 - Texture.Height));
                        break;
                    }
            }
            shoodSpeed = rndValue.Next(1, 2) - 0.5f;
            Speed = 7;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            shootPeriod += (float)gameTime.ElapsedGameTime.TotalSeconds;

            CheckOnCollisions(sprites);
            GetDiagonalMovement();

            if (shootPeriod > shoodSpeed)
            {
                shootPeriod = 0;
                AddEnemyBullet(sprites);
            }

            if (CollisionModel.Top > Game1.ScreenHeight) IsRemoved = true;
        }

        private void GetDiagonalMovement()
        {
            switch (IsLeft)
            {
                case true:
                    {
                        Position.X += (float)Math.Cos((Math.PI / 4)) * Speed;
                        Position.Y += -(float)Math.Sin(-(Math.PI / 4)) * Speed;
                        break;
                    }
                case false:
                    {
                        Position.X += (float)Math.Cos(3 * Math.PI / 4) * Speed;
                        Position.Y += -(float)Math.Sin(-(3 * Math.PI / 4)) * Speed;
                        break;
                    }
            }
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
