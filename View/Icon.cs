using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace SpaceShooterGame.View
{
    public class Icon : Component
    {
        public Texture2D Texture { get; private set; }
        public bool IsActive { get; set; }
        public Vector2 Position;
        public float LayerRatio { get; set; }
        public SpriteFont Title { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            }
        }

        public Icon(Texture2D texture)
        {
            Texture = texture;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var iconColor = Color.White;

            if (IsActive)
                iconColor = Color.DarkGray;
            spriteBatch.Draw(Texture, Rectangle, null, iconColor, 0f, new Vector2(0, 0), SpriteEffects.None, 0.05f + LayerRatio);
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
