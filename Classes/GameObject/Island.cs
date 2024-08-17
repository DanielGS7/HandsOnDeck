using HandsOnDeck2.Classes.Collision;
using HandsOnDeck2.Classes.GameObject.Entity;
using HandsOnDeck2.Classes.Global;
using HandsOnDeck2.Classes.Rendering;
using HandsOnDeck2.Enums;
using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace HandsOnDeck2.Classes.GameObject
{
    public class Island : IGameObject, ICollideable
    {
        public VisualElement VisualElement { get; set; }
        public SeaCoordinate Position { get; set; }
        public Vector2 Size { get; set; }
        public float Rotation { get; set; }
        public string Name { get; set; }
        public int SpriteIndex { get; set; }
        public Vector2 Origin { get; set; }
        public float Scale { get; set; }
        public bool IsColliding { get; set; }
        Siren Siren { get; set; }

        private static Texture2D islandTexture;
        private static readonly Random random = new Random();
        private static readonly float minScale = 0.3f;
        private static readonly float maxScale = 1f;
        public static readonly int islandGridSize = 4;
        private static readonly int islandSpriteSize = 512;
        public static readonly int totalIslands = islandGridSize * islandGridSize;
        private static readonly float colliderSizeMultiplier = 0.6f;
        public Vector2 ColliderSize => Size * Scale * colliderSizeMultiplier;

        public Island(ContentManager content, int spriteIndex, string name, SeaCoordinate position, float scale, float rotation)
        {
            if (islandTexture == null)
            {
                islandTexture = content.Load<Texture2D>("island");
            }

            SpriteIndex = spriteIndex;
            Name = name;
            Position = position;
            Rotation = rotation;
            Size = new Vector2(islandSpriteSize, islandSpriteSize);
            Scale = scale;
            Origin = new Vector2(islandSpriteSize / 2, islandSpriteSize / 2);
            var animation = new Animation("island", Size, islandGridSize, spriteIndex, spriteIndex, 0f, false);
            animation.LoadContent(content);

            VisualElement = new VisualElement(animation, Color.White, SpriteEffects.None, 0f);
        }

        public void Update(GameTime gameTime)
        {
            VisualElement.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            VisualElement.Draw(spriteBatch, Position, Origin, Scale, Rotation);
            Siren?.Draw(spriteBatch);
        }

        public static List<Island> GenerateIslands(ContentManager content, int count)
        {
            List<Island> islands = new List<Island>();

            for (int i = 0; i < count; i++)
            {
                Island newIsland;
                bool overlaps;

                do
                {
                    overlaps = false;
                    newIsland = new Island(content, i, GenerateName(), GenerateRandomPosition(), GenerateRandomScale(), 0);

                    foreach (var island in islands)
                    {
                        if (newIsland.Position.DistanceTo(island.Position) < (newIsland.Size.X + island.Size.X) * 0.5f * Math.Max(newIsland.Scale, island.Scale))
                        {
                            overlaps = true;
                            break;
                        }
                    }
                }
                while (overlaps);
                newIsland.SpawnSiren(content, GetSirenSpawnChance());
                islands.Add(newIsland);
            }
            foreach (ICollideable island in islands) CollisionManager.Instance.AddCollideable(island);
            return islands;
        }

        private static float GetSirenSpawnChance()
        {
            switch (GlobalInfo.Instance.CurrentDifficulty)
            {
                case Difficulty.Easy: return 0.1f;
                case Difficulty.Normal: return 0.3f;
                case Difficulty.Hard: return 0.5f;
                default: return 0.3f;
            }
        }

        private static SeaCoordinate GenerateRandomPosition()
        {
            return new SeaCoordinate(
                (float)new Random().NextDouble() * Map.MapWidth,
                (float)new Random().NextDouble() * Map.MapHeight
            );
        }
        public void OnCollision(ICollideable other)
        {
        }

        private static string GenerateName()
        {
            string[] namePart1 = { "Sunny", "Misty", "Rocky" };
            string[] namePart2 = { "Isle", "Cove", "Bay" };
            string[] namePart3 = { "Haven", "Retreat", "Sanctuary" };

            return $"{namePart1[random.Next(namePart1.Length)]} {namePart2[random.Next(namePart2.Length)]} {namePart3[random.Next(namePart3.Length)]}";
        }

        private static float GenerateRandomScale()
        {
            return (float)random.NextDouble() * (maxScale - minScale) + minScale;
        }

        public void SpawnSiren(ContentManager content, float spawnChance)
        {
            if (new Random().NextDouble() < spawnChance)
            {
                Siren = new Siren(content, Position);
                Map.Instance.AddSiren(Siren);
            }
        }
    }
}