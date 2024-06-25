using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck2.Classes
{
    public class Animation
    {
        private Texture2D spriteSheet;
        private string spriteSheetName;
        private int spriteIndex;
        private int rowCount;
        private int totalSprites;
        private Vector2 spriteSize;
        private Rectangle sourceRectangle;
        private float speed;
        private bool isLooping;

        private double timeSinceLastFrame = 0;

        public Vector2 SpriteSize => spriteSize;

        public float Speed { get => speed; set => speed = value; }

        public Animation(string spriteSheetName, Vector2 spriteSize, int rowCount, int totalSprites, float speed, bool isLooping)
        {
            this.spriteSheetName = spriteSheetName;
            this.spriteSize = spriteSize;
            this.rowCount = rowCount;
            this.totalSprites = totalSprites;
            this.spriteIndex = 0;
            this.Speed = speed;
            this.isLooping = isLooping;

            this.sourceRectangle = CalculateSourceRectangle(this.spriteIndex);
        }

        public void LoadContent(ContentManager content)
        {
            spriteSheet = content.Load<Texture2D>(spriteSheetName);
        }

        public void Update(GameTime gameTime)
        {
            double elapsedMilliseconds = gameTime.ElapsedGameTime.TotalMilliseconds;
            double frameTime = 1000.0 / Speed;
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
                    else
                    {
                        spriteIndex = totalSprites - 1;
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
