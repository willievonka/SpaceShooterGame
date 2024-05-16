using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooterGame.Model.States;

namespace SpaceShooterGame
{
    public class Game1 : Game
    {
        private State currentState;
        private State nextState;

        public void ChangeState(State state)
        {
            nextState = state;
        }

        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch _spriteBatch;

        public static int ScreenWidth;
        public static int ScreenHeight;

        public float gameTimer;

        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            ScreenWidth = 960;
            ScreenHeight = 960;

            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            currentState = new MenuState(this, GraphicsDevice, Content);
            currentState.LoadContent();
            nextState = null;
        }

        protected override void Update(GameTime gameTime)
        {
            if (nextState != null)
            {
                currentState = nextState;
                currentState.LoadContent();

                nextState = null;
            }

            currentState.Update(gameTime);
            currentState.PostUpdate(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(55, 55, 55));
            currentState.Draw(gameTime, _spriteBatch);
        }
    }
}
