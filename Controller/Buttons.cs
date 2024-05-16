using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooterGame.View;
using System;


namespace SpaceShooterGame.Controller
{
    public class Buttons : Component
    {
        #region Fields

        private readonly Texture2D Texture;
        private readonly SpriteFont Font;
        private MouseState CurrentState;
        private MouseState PreviousState;
        private bool IsHovering;

        #endregion

        #region Properties

        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public event EventHandler Click;
        public bool Clicked { get; private set; }
        public Color PenColor { get; set; }
        public string Text { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            }
        }

        #endregion

        public Buttons(Texture2D texture, SpriteFont font, Color color)
        {
            Texture = texture;
            Font = font;
            PenColor = color;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var buttonColor = Color.White;

            if (IsHovering)
                buttonColor = Color.Gray;

            spriteBatch.Draw(Texture, Rectangle, buttonColor);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2) - Font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2) - Font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(Font, Text, new Vector2(x, y), PenColor);
            }
        }

        public override void Update(GameTime gameTime)
        {
            PreviousState = CurrentState;
            CurrentState = Mouse.GetState();

            var mouseRectangle = new Rectangle(CurrentState.X, CurrentState.Y, 1, 1);

            IsHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                IsHovering = true;

                if (CurrentState.LeftButton == ButtonState.Released && PreviousState.LeftButton == ButtonState.Pressed)
                    Click?.Invoke(this, new EventArgs());
            }
        }
    }
}
