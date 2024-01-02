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
                float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
                spriteIndex += (int)(elapsedSeconds / speed/7);

                if (spriteIndex >= totalSprites)
                {
                spriteIndex = 0;
                }
                sourceRectangle = CalculateSourceRectangle(this.spriteIndex);
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

        public void Draw(Vector2 position, Vector2 totalSurface)
        {
            for(int i= 0; i < totalSurface.Y; i += sourceRectangle.Height)
            {
                for (int j = 0; j < totalSurface.X; j += sourceRectangle.Width)
                {
                    Draw(new Vector2(j, i));
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
