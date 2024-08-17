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
                case Difficulty.Hard:
                default:
                return 1.5f;
            }
        }

        public float GetSirenPullingPower()
        {
            switch (currentDifficulty)
            {
                case Difficulty.Easy: return 0.005f;
                case Difficulty.Normal: return 0.01f;
                case Difficulty.Hard: 
                default:
                return 0.02f;
            }
        }

        public float GetEnemyShootInterval()
        {
            switch (currentDifficulty)
            {
                case Difficulty.Easy: return 3f;
                case Difficulty.Normal: return 2f;
                case Difficulty.Hard: 
                default:
                return 1f;
            }
        }

        public int GetMaxEnemiesInGame()
        {
            switch (currentDifficulty)
            {
                case Difficulty.Easy: return 5;
                case Difficulty.Normal: return 8;
                case Difficulty.Hard: 
                default:
                return 10;
            }
        }

        internal float GetInvincibilityDuration()
        {
            switch (currentDifficulty)
            {
                case Difficulty.Normal:
                    return 0.5f;
                case Difficulty.Hard:
                    return 0.2f;
                case Difficulty.Easy:
                default:
                    return 1f;
            }
        }

        internal float GetWaterIncreaseRate()
        {
            switch (currentDifficulty)
            {
                case Difficulty.Normal:
                    return 0.016f;
                case Difficulty.Hard:
                    return 0.024f;
                case Difficulty.Easy:
                default:
                    return 0.008f;
            }
        }
    }
}