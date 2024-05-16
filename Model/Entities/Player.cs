using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooterGame.Model.PlayerInfo;
using System;
using System.Collections.Generic;


namespace SpaceShooterGame.Model.Entities
{
    public class Player : Sprite
    {
        #region Fields & Properties

        public Bullet Bullet;
        public Score PlayerScore;
        public bool BoughtShield { get; set; }
        public bool BoughtUltimate { get; set; }
        public bool IsDead { get; private set; }
        public bool IsGod { get; private set; }
        private float GodModeTime;
        public bool CanUltimate { get; private set; }
        private float UltimateTime;
        private float UltimateShootTimer;
        public bool GoExit { get; private set; }

        public Player(Texture2D texture) :
            base(texture)
        {
            PlayerScore = new Score();
            Speed = 7;
        }

        #endregion

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            UpdateTimers(gameTime);

            InputInfo.previousKey = InputInfo.currentKey;
            InputInfo.currentKey = Keyboard.GetState();

            MovePlayer();
            CheckOnCollisions(sprites);

            if (InputInfo.currentKey.IsKeyDown(InputInfo.Attack) && InputInfo.previousKey.IsKeyUp(InputInfo.Attack))
                AddBullet(sprites);

            if (InputInfo.currentKey.IsKeyDown(InputInfo.Ultimate) && PlayerScore.TotalScore >= 20)
            {
                CanUltimate = true;
                PlayerScore.TotalScore = 0;
            }

            if (InputInfo.currentKey.IsKeyDown(InputInfo.Exit))
                GoExit = true;

            UseShield();
            UseUltimate(gameTime, sprites);
        }

        private void UpdateTimers(GameTime gameTime)
        {
            UltimateTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            GodModeTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            UltimateShootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void UseUltimate(GameTime _, List<Sprite> sprites)
        {
            if (CanUltimate)
            {
                if (UltimateTime < 5f)
                {
                    if (UltimateShootTimer > 0.25f)
                    {
                        AddUltimateBullet(sprites, Bullet.BulletDirection, 45, 0);
                        AddUltimateBullet(sprites, Bullet.BulletDirection, -15, 0);
                        AddUltimateBullet(sprites, Bullet.BulletDirection, 15, -15);
                        AddUltimateBullet(sprites, new Vector2((float)Math.Cos(3 * Math.PI / 4),
                            (float)Math.Sin(-(3 * Math.PI / 4))), -30, 15);
                        AddUltimateBullet(sprites, new Vector2((float)Math.Cos(Math.PI / 4),
                            (float)Math.Sin(-(Math.PI / 4))), 60, 15);
                        UltimateShootTimer = 0;
                    }
                }
                else
                {
                    UltimateTime = 0;
                    CanUltimate = false;
                    BoughtUltimate = false;
                }
            }
        }

        private void AddUltimateBullet(List<Sprite> sprites, Vector2 bulletDir, float X, float Y)
        {
            var bullet = Bullet.Clone() as Bullet;
            bullet.Position.X = Position.X + X;
            bullet.Position.Y = Position.Y + Y;
            bullet.Speed = Speed * 3;
            bullet.LifeDuration = 2f;
            bullet.BulletDirection = bulletDir;

            sprites.Add(bullet);
        }

        private void UseShield()
        {
            if (InputInfo.currentKey.IsKeyDown(InputInfo.UseShield) && PlayerScore.TotalScore >= 10 && !IsGod)
            {
                IsGod = true;
                PlayerScore.TotalScore = 0;
                GodModeTime = 0;
            }
            if (GodModeTime > 10f)
            {
                IsGod = false;
                GodModeTime = 0;
                BoughtShield = false;
            }
        }

        private void CheckOnCollisions(List<Sprite> sprites)
        {
            foreach (var sprite in sprites)
            {
                if (sprite is Player) continue;
                if (sprite is Bullet && sprite is not EnemyBullet) continue;
                if (sprite.CollisionModel.Intersects(CollisionModel))
                {
                    if (IsGod) continue;
                    else
                        IsDead = true;
                }
            }
        }

        private void MovePlayer()
        {
            if (Keyboard.GetState().IsKeyDown(InputInfo.Left))
                Position.X -= Speed;

            if (Keyboard.GetState().IsKeyDown(InputInfo.Right))
                Position.X += Speed;

            if (Keyboard.GetState().IsKeyDown(InputInfo.Up))
                Position.Y -= Speed;

            if (Keyboard.GetState().IsKeyDown(InputInfo.Down))
                Position.Y += Speed;

            Position.Y = MathHelper.Clamp(Position.Y, 0, Game1.ScreenHeight - 200 - Texture.Height);
            Position.X = MathHelper.Clamp(Position.X, 0, Game1.ScreenWidth - Texture.Width);
        }

        private void AddBullet(List<Sprite> sprites)
        {
            var bullet = Bullet.Clone() as Bullet;
            bullet.Position.X = Position.X + 15;
            bullet.Position.Y = Position.Y;
            bullet.Speed = Speed * 2;
            bullet.LifeDuration = 2f;

            sprites.Add(bullet);
        }
    }
}
