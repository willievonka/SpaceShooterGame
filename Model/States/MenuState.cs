using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooterGame.Controller;
using SpaceShooterGame.View;
using System;
using System.Collections.Generic;


namespace SpaceShooterGame.Model.States
{
    public class MenuState : State
    {
        private List<Component> components;
        private List<Icon> MenuBackGround;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager contentManager)
            : base(game, graphicsDevice, contentManager)
        {
            Game.IsMouseVisible = true;
        }

        public override void LoadContent()
        {
            var buttonTexture = ContentManager.Load<Texture2D>("button");
            var buttonFont = ContentManager.Load<SpriteFont>("MinecraftFont");

            MenuBackGround = new List<Icon>()
            {
                new(ContentManager.Load<Texture2D>("mainMenu"))
            };

            var startGameButton = new Buttons(buttonTexture, buttonFont, Color.Black)
            {
                Position = new Vector2(Game1.ScreenWidth / 2 - buttonTexture.Width / 2,
                    3 * Game1.ScreenHeight / 4 - buttonTexture.Height / 2),
                Text = "New Game",
            };

            startGameButton.Click += StartGameButton_Click;

            var quitGameButton = new Buttons(buttonTexture, buttonFont, Color.Black)
            {
                Position = new Vector2(Game1.ScreenWidth / 2 - buttonTexture.Width / 2,
                    3 * Game1.ScreenHeight / 4 - buttonTexture.Height / 2 + 200),
                Text = "Quit",
            };

            quitGameButton.Click += QuitGameButton_Click;

            var helpButton = new Buttons(buttonTexture, buttonFont, Color.Black)
            {
                Position = new Vector2(Game1.ScreenWidth / 2 - buttonTexture.Width / 2,
                    3 * Game1.ScreenHeight / 4 - buttonTexture.Height / 2 + 100),
                Text = "Controls",
            };

            helpButton.Click += GetHelpButton_Click;

            components = new List<Component>()
            {
                startGameButton,
                quitGameButton,
                helpButton,
            };
        }

        private void GetHelpButton_Click(object sender, EventArgs e)
        {
            Game.ChangeState(new InfoState(Game, GraphicDevice, ContentManager));
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            Game.Exit();
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            Game.ChangeState(new GameState(Game, GraphicDevice, ContentManager));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (var icon in MenuBackGround)
                icon.Draw(gameTime, spriteBatch);

            foreach (var component in components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in components)
                component.Update(gameTime);
        }
    }
}
