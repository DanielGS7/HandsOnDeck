using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Classes.Levels
{
    internal class Level
    {
        public int LevelNumber { get; }
        public int ExplosiveBarrels { get; }
        public int EnemyBoats { get; }
        public int KamikazeBoats { get; }

        public Level(int levelNumber, int explosiveBarrels, int enemyBoats, int kamikazeBoats)
        {
            LevelNumber = levelNumber;
            ExplosiveBarrels = explosiveBarrels;
            EnemyBoats = enemyBoats;
            KamikazeBoats = kamikazeBoats;
        }

        public void SpawnEntities()
        {
            
        }
    }
}
