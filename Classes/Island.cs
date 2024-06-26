using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace HandsOnDeck2.Classes
{
    public class Island : IGameObject
    {
        private static Texture2D islandTexture;
        private static readonly Random random = new Random();
        private static readonly float minScale = 0.2f;
        private static readonly float maxScale = 1.2f;
        public static readonly int islandGridSize = 4;
        private static readonly int islandSpriteSize = 512;
        public static readonly int totalIslands = islandGridSize * islandGridSize;
        private static readonly List<string> namePart1 = new List<string> { "Sunny", "Misty", "Rocky" };
        private static readonly List<string> namePart2 = new List<string> { "Isle", "Cove", "Bay" };
        private static readonly List<string> namePart3 = new List<string> { "Haven", "Retreat", "Sanctuary" };
        
        public VisualElement VisualElement { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public float Rotation { get; set; }
        public string Name { get; set; }
        public int SpriteIndex { get; set; }

        public Island(ContentManager content, GraphicsDevice graphicsDevice, int spriteIndex, string name, Vector2 position, float scale, float rotation)
        {
            if (islandTexture == null)
            {
                islandTexture = content.Load<Texture2D>("island");
            }

            this.SpriteIndex = spriteIndex;
            this.Name = name;
            this.Position = position;
            this.Rotation = rotation;
            this.Size = new Vector2(islandSpriteSize, islandSpriteSize) * scale;

            var spriteSize = new Vector2(islandSpriteSize, islandSpriteSize);
            var animation = new Animation("island", spriteSize, islandGridSize, spriteIndex, spriteIndex, 0f, false);
            animation.LoadContent(content);

            VisualElement = new VisualElement(animation, position, new Vector2(spriteSize.X / 2, spriteSize.Y / 2), scale, rotation, Color.White, SpriteEffects.None, 0f);
        }

        public Island(ContentManager content, GraphicsDevice graphicsDevice)
            : this(content, graphicsDevice, random.Next(totalIslands), GenerateName(), GenerateRandomPosition(graphicsDevice), GenerateRandomScale(), GenerateRandomRotation())
        {
        }

        private static string GenerateName()
        {
            return $"{namePart1[random.Next(namePart1.Count)]} {namePart2[random.Next(namePart2.Count)]} {namePart3[random.Next(namePart3.Count)]}";
        }

        private static Vector2 GenerateRandomPosition(GraphicsDevice graphicsDevice)
        {
            int viewportWidth = graphicsDevice.Viewport.Width;
            int viewportHeight = graphicsDevice.Viewport.Height;
            int mapWidth = viewportWidth * 10;
            int mapHeight = viewportHeight * 10;

            float x = (float)random.NextDouble() * (mapWidth - islandSpriteSize);
            float y = (float)random.NextDouble() * (mapHeight - islandSpriteSize);

            return new Vector2(x, y);
        }

        private static float GenerateRandomScale()
        {
            return (float)random.NextDouble() * (maxScale - minScale) + minScale;
        }

        private static float GenerateRandomRotation()
        {
            return (float)random.NextDouble() * MathHelper.TwoPi;
        }

        public void Update(GameTime gameTime)
        {
            VisualElement.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            VisualElement.Draw(spriteBatch);
        }

        public static List<Island> GenerateIslands(ContentManager content, GraphicsDevice graphicsDevice, int count)
        {
            List<Island> islands = new List<Island>();
            HashSet<int> usedIndices = new HashSet<int>();

            for (int i = 0; i < totalIslands; i++)
            {
                Island newIsland;
                bool overlaps;

                do
                {
                    overlaps = false;
                    newIsland = new Island(content, graphicsDevice, i, GenerateName(), GenerateRandomPosition(graphicsDevice), GenerateRandomScale(), GenerateRandomRotation());

                    foreach (var island in islands)
                    {
                        if (Vector2.Distance(newIsland.Position, island.Position) < (newIsland.Size.X + island.Size.X))
                        {
                            overlaps = true;
                            break;
                        }
                    }
                }
                while (overlaps);

                islands.Add(newIsland);
                usedIndices.Add(i);
            }

            return islands;
        }
    }
}