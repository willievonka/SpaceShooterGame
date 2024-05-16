using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using SpaceShooterGame.Model.PlayerInfo;


namespace SpaceShooterGame.Model.Entities
{
    public class Sprite : ICloneable
    {
        public Texture2D Texture;
        public Vector2 Position;
        public bool IsRemoved;
        public float Speed;
        public float LifeDuration;
        public InputInfo InputInfo;

        public Rectangle CollisionModel
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            }
        }

        public Sprite(Texture2D texture)
        {
            Texture = texture;
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f,
                new Vector2(0, 0), 1f, SpriteEffects.None, 0.001f);
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
