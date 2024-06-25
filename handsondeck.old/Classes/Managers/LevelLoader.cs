using HandsOnDeck.Classes.Levels;
using HandsOnDeck.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Classes.Managers
{
    internal class LevelLoader
    {
        private List<Level> levels;
        private int currentLevelIndex;

        public LevelLoader()
        {
            levels = new List<Level>();
            InitializeLevels();
            currentLevelIndex = 0;
        }

        public void InitializeLevels()
        {
            

            Level level1 = new Level(1, 1, 1, 1);
            Level level2 = new Level(2, 2, 2, 1);
            Level level3 = new Level(3, 3, 3, 2);

            levels.Add(level1);
            levels.Add(level2);
            levels.Add(level3);
        }

        public void StartNextLevel()
        {
            if (currentLevelIndex < levels.Count)
            {
                Level currentLevel = levels[currentLevelIndex];
                currentLevel.SpawnEntities(); 
                currentLevelIndex++;
            }
            else
            {
                GameOverScreen.GetInstance.HasWon(true);
                GameStateManager.GetInstance.ChangeState(GameState.GameOver);
            }
        }
    }
}
