using Microsoft.Xna.Framework.Input;


namespace SpaceShooterGame.Model.PlayerInfo
{
    public class InputInfo
    {
        public Keys Left;
        public Keys Right;
        public Keys Up;
        public Keys Down;
        public Keys Attack;
        public Keys UseShield;
        public Keys Ultimate;
        public Keys Exit;

        public KeyboardState previousKey;
        public KeyboardState currentKey;
    }
}
