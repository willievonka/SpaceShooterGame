using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooterGame.Controller;
using SpaceShooterGame.Model.PlayerInfo;
using SpaceShooterGame.View;
using System;
using System.Collections.Generic;


namespace SpaceShooterGame.Model.States
{
    public class GameOverState : State
    {
        private List<Component> components;
        private SpriteFont GameOverTextFont;
        private readonly Score Score;

        public GameOverState(Game1 game, GraphicsDevice graphicsDevice, ContentManager contentManager, Score score) : base(game, graphicsDevice, contentManager)
        {
            Game.IsMouseVisible = true;
            Score = score;
        }

        public override void LoadContent()
        {
            GameOverTextFont = ContentManager.Load<SpriteFont>("MinecraftBigFont");
            var buttonTexture = ContentManager.Load<Texture2D>("button");
            var buttonFont = ContentManager.Load<SpriteFont>("MinecraftFont");

            var restartGameButton = new Buttons(buttonTexture, buttonFont, Color.Black)
            {
                Position = new Vector2(Game1.ScreenWidth / 2 - buttonTexture.Width / 2,
                    3 * Game1.ScreenHeight / 4 - buttonTexture.Height / 2),
                Text = "Restart",
            };

            restartGameButton.Click += RestartGameButton_Click;

            var goToMenuButton = new Buttons(buttonTexture, buttonFont, Color.Black)
            {
                Position = new Vector2(Game1.ScreenWidth / 2 - buttonTexture.Width / 2,
                    3 * Game1.ScreenHeight / 4 - buttonTexture.Height / 2 + 100),
                Text = "Go to menu",
            };

            goToMenuButton.Click += GoToMenuButton_Click;

            components = new List<Component>()
            {
                restartGameButton,
                goToMenuButton
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (Score.AmountOfKills >= 50)
                spriteBatch.DrawString(GameOverTextFont, "Victory!", new Vector2(100, 100), Color.White);
            else
                spriteBatch.DrawString(GameOverTextFont, "Game Over!", new Vector2(100, 100), Color.White);
            spriteBatch.DrawString(GameOverTextFont, string.Format("Your score is: {0}", Score.TotalScore),
                                    new Vector2(100, 200), Color.White);
            spriteBatch.DrawString(GameOverTextFont, string.Format("Player kills: {0}", Score.AmountOfKills),
                                    new Vector2(100, 300), Color.White);

            foreach (var component in components)
                component.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        private void RestartGameButton_Click(object sender, EventArgs e)
        {
            Game.ChangeState(new GameState(Game, GraphicDevice, ContentManager));
        }

        private void GoToMenuButton_Click(object sender, EventArgs e)
        {
            Game.ChangeState(new MenuState(Game, GraphicDevice, ContentManager));
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
