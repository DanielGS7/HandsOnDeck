﻿using HandsOnDeck.Classes.MonogameAccessibility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Numerics;

namespace HandsOnDeck.Classes.Animations
{
    public class Animation
    {
        private Texture2D spriteSheet;
        private String spriteSheetName;
        private int spriteIndex;
        private int rowCount;
        private int totalSprites;
        private Vector2 spriteSize;
        private Rectangle sourceRectangle;
        private float speed;
        private bool isLooping;

        private double timeSinceLastFrame = 0;

        public Animation(String spriteSheetName, Vector2 spriteSize, int spriteIndex, int rowCount, int totalSprites, float speed, bool isLooping)
        {
            this.spriteSheetName = spriteSheetName;
            this.spriteSize = spriteSize;
            this.rowCount = rowCount;
            this.totalSprites = totalSprites;
            this.spriteIndex = spriteIndex;
            this.speed = speed;
            this.isLooping = isLooping;

            this.sourceRectangle = CalculateSourceRectangle(this.spriteIndex);
        }

        internal void LoadContent()
        {
            spriteSheet = ContentLoader.Load<Texture2D>(spriteSheetName);
        }
        public void Update(GameTime gameTime)
        {
            double elapsedMilliseconds = gameTime.ElapsedGameTime.TotalMilliseconds;
            double frameTime = 1000.0 / speed;
            timeSinceLastFrame += elapsedMilliseconds;
            if (timeSinceLastFrame >= frameTime)
            {
                spriteIndex++;

                if (spriteIndex >= totalSprites)
                {
                    if (isLooping)
                    {
                        spriteIndex = 0;
                    }
                }
                timeSinceLastFrame = 0;
                sourceRectangle = CalculateSourceRectangle(spriteIndex);
            }
        }


        public void Draw(WorldCoordinate position)
        {
            SpriteBatchManager.Instance.Draw(
                spriteSheet,
                position.ToVector2(),
                sourceRectangle,
                Color.White
            );
        }

        public void DrawStatic(Vector2 position, float scale, float rotation, Vector2 origin){
            SpriteBatchManager.Instance.Draw(spriteSheet, position, sourceRectangle, Color.White, rotation, origin, scale, SpriteEffects.None, 0);
        }

        public void Draw(WorldCoordinate position, float scale, float rotation, Vector2 origin)
        {
            SpriteBatchManager.Instance.Draw(spriteSheet, position.ToVector2(), sourceRectangle, Color.White, rotation, origin, scale, SpriteEffects.None, 0);
        }

        public void Draw(WorldCoordinate position, Vector2 totalSurface)
        {
            Vector2 extendedSurface = new Vector2(totalSurface.X + sourceRectangle.Width*2, totalSurface.Y + sourceRectangle.Height*2);

            for (int i = 0; i <= extendedSurface.Y; i += sourceRectangle.Height)
            {
                for (int j = 0; j <= extendedSurface.X; j += sourceRectangle.Width)
                {
                    // Adjust for potential negative values in position.X/Y by ensuring it wraps positively
                    int posX = (int)((j - position.X) % 128 + 128) % 128;
                    int posY = (int)((i - position.Y) % 128 + 128) % 128;
                    WorldCoordinate tilePosition = new WorldCoordinate(posX, posY);
                    Draw(tilePosition); // Assuming Draw(Vector2) draws the tile at the position
                }
            }
        }
        private Rectangle CalculateSourceRectangle(int spriteIndex)
        {
            int col = spriteIndex % rowCount;
            int row = spriteIndex / rowCount;

            int x = col * (int)spriteSize.X;
            int y = row * (int)spriteSize.Y;
            return new Rectangle(x, y, (int)spriteSize.X, (int)spriteSize.Y);
        }
    }
}
