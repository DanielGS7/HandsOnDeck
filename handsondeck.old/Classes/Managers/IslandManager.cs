using HandsOnDeck.Classes.Object.Entity;
using HandsOnDeck.Classes.Object.Static;
using HandsOnDeck.Classes.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Classes.Managers
{
    internal class IslandManager
    {
        private static IslandManager instance;
        private List<Island> islands;
        private Random random = new Random();
        private IslandManager()
        {
            islands = new List<Island>();
            InitializeIslands();
        }

        public static IslandManager GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IslandManager();
                }
                return instance;
            }
        }

        private void InitializeIslands()
        {
            const int islandCount = 16;
            float minDistance = GameScreen.WorldSize.X / 6;
            for (int i = 0; i < islandCount; i++)
            {
                WorldCoordinate position;
                int rotation = random.Next(0, 360);
                do
                {
                    position = new WorldCoordinate(
                        random.Next(0, (int)GameScreen.WorldSize.X),
                        random.Next(0, (int)GameScreen.WorldSize.Y)
                    );
                }
                while (!IsPositionSufficientlyFar(position, (int)minDistance));
                Island island = new Island(position, i, rotation);
                islands.Add(island);
                CollisionManager.GetInstance.AddCollideableObject(island);
            }
        }

        public void LoadContent()
        {
            foreach (var island in islands)
            {
                island.LoadContent();
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var island in islands)
            {
                WorldCoordinate drawPosition = GameScreen.GetInstance.viewportManager.GetDrawPosition(island.position);
                island.Draw(gameTime, drawPosition);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var island in islands)
            {
                island.Update(gameTime);
            }
        }

        private bool IsPositionSufficientlyFar(WorldCoordinate position, int minDistance)
        {
            foreach (var island in islands)
            {
                if (Vector2.Distance(position.ToVector2(), island.position.ToVector2()) < minDistance)
                    return false;
            }
            return true;
        }
    }

}
