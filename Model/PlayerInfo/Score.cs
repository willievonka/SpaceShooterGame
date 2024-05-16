using Microsoft.Xna.Framework.Graphics;


namespace SpaceShooterGame.Model.PlayerInfo
{
    public class Score
    {
        public int TotalScore { get; set; }
        public int AmountOfKills { get; set; }
        public SpriteFont Font { get; set; }
        public Score()
        {
            AmountOfKills = 0;
            TotalScore = 0;
        }
    }
}
