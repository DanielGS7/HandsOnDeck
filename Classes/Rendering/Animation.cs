using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck2.Classes.Rendering
{
    public class Animation
    {
        private Texture2D spriteSheet;
        private string spriteSheetName;
        private int spriteIndex;
        private int startSpriteIndex;
        private int endSpriteIndex;
        private int rowCount;
        private Vector2 spriteSize;
        private Rectangle sourceRectangle;
        private float speed;
        private bool isLooping;

        private double timeSinceLastFrame = 0;

        public Vector2 SpriteSize => spriteSize;

        public Animation(string spriteSheetName, Vector2 spriteSize, int rowCount, int totalSprites, float speed, bool isLooping)
        {
            this.spriteSheetName = spriteSheetName;
            this.spriteSize = spriteSize;
            this.rowCount = rowCount;
            startSpriteIndex = 0;
            endSpriteIndex = totalSprites - 1;
            spriteIndex = startSpriteIndex;
            this.speed = speed;
            this.isLooping = isLooping;

            sourceRectangle = CalculateSourceRectangle(spriteIndex);
        }

        public Animation(string spriteSheetName, Vector2 spriteSize, int rowCount, int startSpriteIndex, int endSpriteIndex, float speed, bool isLooping)
        {
            this.spriteSheetName = spriteSheetName;
            this.spriteSize = spriteSize;
            this.rowCount = rowCount;
            this.startSpriteIndex = startSpriteIndex;
            this.endSpriteIndex = endSpriteIndex;
            spriteIndex = startSpriteIndex;
            this.speed = speed;
            this.isLooping = isLooping;

            sourceRectangle = CalculateSourceRectangle(spriteIndex);
        }

        public void LoadContent(ContentManager content)
        {
            spriteSheet = content.Load<Texture2D>(spriteSheetName);
        }

        public void Update(GameTime gameTime)
        {
            double elapsedMilliseconds = gameTime.ElapsedGameTime.TotalMilliseconds;
            double frameTime = 1000.0 / speed;
            timeSinceLastFrame += elapsedMilliseconds;
            if (timeSinceLastFrame >= frameTime)
            {
                spriteIndex++;

                if (spriteIndex > endSpriteIndex)
                {
                    if (isLooping)
                    {
                        spriteIndex = startSpriteIndex;
                    }
                    else
                    {
                        spriteIndex = endSpriteIndex;
                    }
                }
                timeSinceLastFrame = 0;
                sourceRectangle = CalculateSourceRectangle(spriteIndex);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float scale, float rotation, Vector2 origin, Color color, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(spriteSheet, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        public void SetSpeed(float newSpeed)
        {
            speed = newSpeed;
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
