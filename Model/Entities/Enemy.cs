using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace SpaceShooterGame.Model.Entities
{
    public class Enemy : Sprite
    {
        public bool IsDead;

        public Enemy(Texture2D texture) :
            base(texture)
        {
            var rndValue = new Random();
            Position = new Vector2(rndValue.Next(0, Game1.ScreenWidth - Texture.Width), -Texture.Height);
            Speed = rndValue.Next(5, 10);
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            CheckOnCollisions(sprites);

            Position.Y += Speed;

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
    }
}
