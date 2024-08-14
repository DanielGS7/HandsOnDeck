using System;
using HandsOnDeck2.Enums;

namespace HandsOnDeck2.Classes.Global
{
    public class DifficultySettings
    {
        private static DifficultySettings instance;
        private static readonly object padlock = new object();

        public static DifficultySettings Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new DifficultySettings();
                        }
                    }
                }
                return instance;
            }
        }

        private Difficulty currentDifficulty;

        private DifficultySettings()
        {
            currentDifficulty = Difficulty.Normal;
        }

        public void SetDifficulty(Difficulty difficulty)
        {
            currentDifficulty = difficulty;
        }

        public float GetEnemySpawnInterval()
        {
            switch (currentDifficulty)
            {
                case Difficulty.Easy: return 5f;
                case Difficulty.Normal: return 3f;
                case Difficulty.Hard: return 1.5f;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public float GetSirenPullingPower()
        {
            switch (currentDifficulty)
            {
                case Difficulty.Easy: return 0.005f;
                case Difficulty.Normal: return 0.01f;
                case Difficulty.Hard: return 0.02f;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public float GetEnemyShootInterval()
        {
            switch (currentDifficulty)
            {
                case Difficulty.Easy: return 3f;
                case Difficulty.Normal: return 2f;
                case Difficulty.Hard: return 1f;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public int GetMaxEnemiesInGame()
        {
            switch (currentDifficulty)
            {
                case Difficulty.Easy: return 5;
                case Difficulty.Normal: return 8;
                case Difficulty.Hard: return 12;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}