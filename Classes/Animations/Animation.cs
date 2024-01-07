using HandsOnDeck.Classes.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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


        public void Draw(Vector2 position)
        {
            SpriteBatchManager.Instance.Draw(
                spriteSheet,
                position,
                sourceRectangle,
                Color.White
            );
        }

        public void Draw(Vector2 position, float scale,float rotation)
        {
            SpriteBatchManager.Instance.Draw(spriteSheet, position, sourceRectangle, Color.White, rotation, Vector2.Zero, scale, SpriteEffects.None, 1);
        }
        public void Draw(Vector2 position, float scale, float rotation, Vector2 origin)
        {
            SpriteBatchManager.Instance.Draw(spriteSheet, position, sourceRectangle, Color.White, rotation, origin, scale, SpriteEffects.None, 1);
        }

        public void Draw(Vector2 position, Vector2 totalSurface)
        {
            Vector2 extendedSurface = new Vector2(totalSurface.X + sourceRectangle.Width*2, totalSurface.Y + sourceRectangle.Height*2);

            for (int i = 0; i <= extendedSurface.Y; i += sourceRectangle.Height)
            {
                for (int j = 0; j <= extendedSurface.X; j += sourceRectangle.Width)
                {
                    Vector2 tilePosition = new Vector2(j, i) - position-new Vector2(128,128);
                    Draw(tilePosition);
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

        internal void LoadContent()
        {
            spriteSheet = ContentLoader.Load<Texture2D>(spriteSheetName);
        }
    }

}
