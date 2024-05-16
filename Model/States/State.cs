using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace SpaceShooterGame.Model.States
{
    public abstract class State
    {
        protected ContentManager ContentManager;
        protected GraphicsDevice GraphicDevice;
        protected Game1 Game;

        public State(Game1 game, GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            Game = game;
            GraphicDevice = graphicsDevice;
            ContentManager = contentManager;
        }

        public abstract void LoadContent();
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
        public abstract void PostUpdate(GameTime gameTime);
    }
}
