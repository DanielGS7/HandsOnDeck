using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Classes.Animations
{
    public class Animation
    {
        private Texture2D spriteSheet;
        private int spriteIndex;
        private int rowCount;
        private int totalSprites;
        private Vector2 spriteSize;
        private Rectangle sourceRectangle;
        private float speed;
        private bool isLooping;

        public Animation(Texture2D spriteSheet, Vector2 spriteSize, int spriteIndex, int rowCount, int totalSprites, float speed, bool isLooping)
        {
            this.spriteSheet = spriteSheet;
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
            spriteIndex += (int)(elapsedSeconds * speed);

            if (spriteIndex >= totalSprites)
            {
                spriteIndex = 0;
            }

            sourceRectangle = CalculateSourceRectangle(this.spriteIndex);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(
                spriteSheet,
                position,
                sourceRectangle,
                Color.White
            );
        }

        private Rectangle CalculateSourceRectangle(int spriteIndex)
        {
            int row = spriteIndex / rowCount;
            int col = spriteIndex % rowCount;

            int x = col * (int)spriteSize.X;
            int y = row * (int)spriteSize.Y;

            return new Rectangle(x, y, (int)spriteSize.X, (int)spriteSize.Y);
        }
    }

}
