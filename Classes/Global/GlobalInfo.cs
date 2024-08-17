namespace HandsOnDeck2.Classes
{
    public class GlobalInfo
    {
        private static GlobalInfo instance;
        public static GlobalInfo Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GlobalInfo();
                }
                return instance;
            }
        }

        public Difficulty CurrentDifficulty { get; private set; }
        public static int Score { get; set; }
        public static float MusicVolume { get; set; } = 0.25f;
        public static float SfxVolume { get; set; } = 0.5f;
        public static bool IsMusicEnabled { get; set; } = true;
        public static bool IsSfxEnabled { get; set; } = true;
        private GlobalInfo()
        {
            ResetGame();
        }

        public void ResetGame()
        {
            Score = 0;
            CurrentDifficulty = Difficulty.Normal;
        }
    }

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }
}