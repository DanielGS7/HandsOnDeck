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

        private void InitializeIslands()
        {
            const int islandCount = 16;
            float minDistance = GameScreen.WorldSize.X / 6;

            for (int i = 0; i < islandCount; i++)
            {
                Vector2 position;
                int rotation = random.Next(0, 360);

                do
                {
                    position = new Vector2(
                        random.Next(0, (int)GameScreen.WorldSize.X),
                        random.Next(0, (int)GameScreen.WorldSize.Y)
                    );
                }
                while (!IsPositionSufficientlyFar(position, (int)minDistance));

                islands.Add(new Island(position, i, rotation));
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
                island.Draw(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var island in islands)
            {
                island.Update(gameTime);
            }
        }

        private bool IsPositionSufficientlyFar(Vector2 position, int minDistance)
        {
            foreach (var island in islands)
            {
                if (Vector2.Distance(position, island.position) < minDistance)
                    return false;
            }
            return true;
        }
    }

}
