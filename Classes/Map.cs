using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace HandsOnDeck2.Classes
{
    public class Map
    {
        private List<Island> islands;
        private Texture2D islandTexture;
        private Random random;
        private int mapWidth;
        private int mapHeight;

        public Map(Texture2D islandTexture, int mapWidth, int mapHeight)
        {
            this.islandTexture = islandTexture;
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            this.islands = new List<Island>();
            this.random = new Random();
            GenerateIslands();
        }

        private void GenerateIslands()
        {
            for (int i = 0; i < 20; i++)
            {
                float x = (float)random.NextDouble() * mapWidth;
                float y = (float)random.NextDouble() * mapHeight;
                islands.Add(new Island(islandTexture, new Vector2(x, y)));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var island in islands)
            {
                island.Draw(spriteBatch);
            }
        }
    }

    public class Island
    {
        private VisualElement visualElement;

        public Island(Texture2D texture, Vector2 position)
        {
            visualElement = new VisualElement(texture, position, new Vector2(texture.Width / 2, texture.Height / 2), 1f, 0f, Color.White, SpriteEffects.None, 0f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            visualElement.Draw(spriteBatch);
        }
    }
}
