using SpaceShooterGame.Model.Entities;
using SpaceShooterGame.Model.PlayerInfo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using SpaceShooterGame.View;


namespace SpaceShooterGame.Model.States
{
    public class GameState : State
    {
        #region Fields

        private List<Sprite> sprites;
        private List<Icon> icons;
        public int PlayersCount { get; private set; }
        public float gameTimer;
        public Score Score;
        public float Difficulty;

        #endregion

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager contentManager)
            : base(game, graphicsDevice, contentManager)
        {
            game.IsMouseVisible = false;
            Score = new Score();
        }

        public override void LoadContent()
        {
            Restart();
        }

        private void Restart()
        {
            var playerTexture = ContentManager.Load<Texture2D>("playerOne");
            Score.Font = ContentManager.Load<SpriteFont>("MinecraftBigFont");

            sprites = new List<Sprite>()
            {
                new Player(playerTexture)
                {
                    Position = new Vector2(Game1.ScreenWidth/2,Game1.ScreenHeight-210),
                    Bullet = new Bullet(ContentManager.Load<Texture2D>("playerShotRight")),
                    InputInfo = new InputInfo()
                    {
                        Left = Keys.A,
                        Right = Keys.D,
                        Up = Keys.W,
                        Down = Keys.S,
                        Attack = Keys.Space,
                        UseShield = Keys.E,
                        Ultimate = Keys.Q,
                        Exit = Keys.Escape,
                    }
                },
            };
            PlayersCount++;

            icons = new List<Icon>()
            {
                new(ContentManager.Load<Texture2D>("gameBackGround"))
                {
                    Position = new Vector2(0,0),
                    LayerRatio = 0.01f,
                },
                new(ContentManager.Load<Texture2D>("GameBar"))
                {
                    Position = new Vector2(0,Game1.ScreenHeight-200),
                    LayerRatio = 0.1f,
                },
                new(ContentManager.Load<Texture2D>("IconUltimate"))
                {
                    Position = new Vector2(50,Game1.ScreenHeight-200+50),
                    LayerRatio = 0.2f,
                    IsActive = true,
                    Title = ContentManager.Load<SpriteFont>("MinecraftFont"),
                },
                new(ContentManager.Load<Texture2D>("Shield_Icon"))
                {
                    Position = new Vector2(Game1.ScreenWidth - 150,Game1.ScreenHeight-200+50),
                    LayerRatio = 0.3f,
                    IsActive = true,
                    Title = ContentManager.Load<SpriteFont>("MinecraftFont"),
                }
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GraphicDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred);
            icons[0].Draw(gameTime, spriteBatch);

            foreach (var sprite in sprites)
                sprite.Draw(spriteBatch);

            for (int i = 1; i < icons.Count; i++)
                icons[i].Draw(gameTime, spriteBatch);
            CreateGameBarTitles(spriteBatch);

            spriteBatch.End();
        }

        private void CreateGameBarTitles(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Score.Font, string.Format("Player points : {0}", Score.TotalScore),
                                    new Vector2(180, Game1.ScreenHeight - 180), Color.White);
            spriteBatch.DrawString(Score.Font, string.Format("Player kills : {0} / 50", Score.AmountOfKills),
                new Vector2(180, Game1.ScreenHeight - 120), Color.White);
            spriteBatch.DrawString(icons[2].Title, string.Format("Enemy spawntime : {0}", Difficulty),
                new Vector2(180, Game1.ScreenHeight - 60), Color.White);
            spriteBatch.DrawString(icons[2].Title, "Ultimate", new Vector2(50, Game1.ScreenHeight - 50), Color.White);
            spriteBatch.DrawString(icons[2].Title, "PowerShield", new Vector2(Game1.ScreenWidth - 150,
                Game1.ScreenHeight - 50), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            gameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var sprite in sprites.ToArray())
            {
                if (sprite is Player)
                {
                    var player = sprite as Player;
                    if (player.PlayerScore.AmountOfKills >= 50)
                        Game.ChangeState(new GameOverState(Game, GraphicDevice, ContentManager, player.PlayerScore));
                    if (player.GoExit)
                        Game.ChangeState(new MenuState(Game, GraphicDevice, ContentManager));
                    CheckOnUsedPercs(player);
                }
                sprite.Update(gameTime, sprites);
            }

            SetUpDifficulty();

            if (gameTimer > Difficulty)
                SpawnEnemy();

            PostUpdate(gameTime);
        }

        private void CheckOnUsedPercs(Player player)
        {
            if (player.PlayerScore.TotalScore >= 10 && !player.IsGod) icons[3].IsActive = false;
            if (player.IsGod && !player.BoughtShield)
            {
                Score.TotalScore -= 10;
                player.BoughtShield = true;
                icons[3].IsActive = true;
                player.Texture = ContentManager.Load<Texture2D>("playerOneShield");
            }
            if (!player.IsGod) player.Texture = ContentManager.Load<Texture2D>("playerOne");

            if (player.PlayerScore.TotalScore >= 20 && !player.CanUltimate) icons[2].IsActive = false;
            if (player.CanUltimate && !player.BoughtUltimate)
            {
                Score.TotalScore -= 20;
                player.BoughtUltimate = true;
                icons[2].IsActive = true;
            }
            player.PlayerScore.TotalScore = Score.TotalScore;
            player.PlayerScore.AmountOfKills = Score.AmountOfKills;
        }

        private void SetUpDifficulty()
        {
            switch (Score.AmountOfKills)
            {
                case 0:
                    {
                        Difficulty = 0.7f;
                        break;
                    }
                case 10:
                    {
                        Difficulty = 0.5f;
                        break;
                    }
                case 30:
                    {
                        Difficulty = 0.4f;
                        break;
                    }
                case 100:
                    {
                        Difficulty = 0.3f;
                        break;
                    }
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i] is Enemy enemy)
                {
                    if (enemy.IsDead)
                    {
                        Score.TotalScore += 2;

                        Score.AmountOfKills++;

                        sprites.RemoveAt(i);
                        i--;
                    }
                }

                if (sprites[i] is Player player)
                {
                    if (player.IsDead)
                    {
                        PlayersCount--;
                        if (PlayersCount == 0)
                        {
                            Game.ChangeState(new GameOverState(Game, GraphicDevice, ContentManager, player.PlayerScore));
                            Score.AmountOfKills = 0;
                            Score.TotalScore = 0;
                        }
                        else
                        {
                            sprites.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }

        private void SpawnEnemy()
        {
            gameTimer = 0;
            var rndValue = new Random();
            var currentEnemy = rndValue.Next(0, 3);
            switch (currentEnemy)
            {
                case 0:
                    {
                        if (Score.AmountOfKills < 10)
                        {
                            sprites.Add(new Enemy(ContentManager.Load<Texture2D>("enemyShipOne")));
                        }
                        else
                            SpawnShootingEnemy();
                        break;
                    }
                case 1:
                    {
                        if (Score.AmountOfKills >= 25)
                        {
                            SpawnDiagonalEnemy();
                            break;
                        }
                        break;
                    }
                case 2:
                    {
                        SpawnShootingEnemy();
                        break;
                    }
            }

        }
        private void SpawnShootingEnemy()
        {
            sprites.Add(new ShootingEnemy(ContentManager.Load<Texture2D>("enemyShipOne"))
            {
                EnemyBullet = new EnemyBullet(ContentManager.Load<Texture2D>("enemyShotRight"))
            });
        }

        private void SpawnDiagonalEnemy()
        {
            var rndValue = new Random();
            var sidePlacer = rndValue.Next(-5, 6);
            GetDiagonalToEnemy(sidePlacer);
        }

        private void GetDiagonalToEnemy(int sidePlacer)
        {
            switch (sidePlacer < 0)
            {
                case true:
                    {
                        sprites.Add(new DiagonalEnemy(ContentManager.Load<Texture2D>("enemyShipDiagonal"), true)
                        {
                            IsLeft = true,
                            EnemyBullet = new EnemyBullet(ContentManager.Load<Texture2D>("enemyShotRight"))
                            {
                                BulletDirection = new Vector2((float)Math.Cos((Math.PI / 4)), -(float)Math.Sin(-(Math.PI / 4)))
                            },
                        });
                        break;
                    }
                case false:
                    {
                        sprites.Add(new DiagonalEnemy(ContentManager.Load<Texture2D>("enemyShipDiagonalRight"), false)
                        {
                            IsLeft = false,
                            EnemyBullet = new EnemyBullet(ContentManager.Load<Texture2D>("enemyShotRight"))
                            {
                                BulletDirection = new Vector2((float)Math.Cos(3 * Math.PI / 4), -(float)Math.Sin(-(3 * Math.PI / 4)))
                            }
                        });
                        break;
                    }
            }
        }
    }
}
