using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck2.Classes
{
    public class Background
    {
        private static Background instance;
        private VisualElement visualElement;
        private Animation animation;
        private float rotation = 0f;
        private float scale = 0.5f;
        private int spriteWidth = 128;
        private int spriteHeight = 128;
        private int columns = 6;
        private int totalSprites = 39;
        private float speed = 5f;
        private Vector2 position;
        private bool isLooping = true;
        private Vector2 offset = Vector2.Zero;

        private Background() { }

        public static Background Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Background();
                }
                return instance;
            }
        }

        public void Initialize(ContentManager content, GraphicsDevice graphicsDevice)
        {
            var spriteSheetName = "water";
            var spriteSize = new Vector2(spriteWidth, spriteHeight);

            animation = new Animation(spriteSheetName, spriteSize, columns, totalSprites, speed, isLooping);
            animation.LoadContent(content);

            visualElement = new VisualElement(animation, Color.White, SpriteEffects.None, 0f);
        }

        public void SetRotation(float rotationDegrees)
        {
            rotation = MathHelper.ToRadians(rotationDegrees);
        }

        public void SetScale(float newScale)
        {
            scale = newScale;
        }

        public void SetAnimationSpeed(float newSpeed)
        {
            animation.SetSpeed(newSpeed);
        }

        public float GetScale()
        {
            return scale;
        }

        public float GetRotation()
        {
            return MathHelper.ToDegrees(rotation);
        }

        public void Update(GameTime gameTime, Vector2 cameraOffset)
        {
            offset = cameraOffset;
            visualElement.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera, Viewport viewport)
        {
            var viewportSize = new Vector2(viewport.Width, viewport.Height);
            var tileSize = new Vector2(spriteWidth, spriteHeight) * scale;
            var bufferSize = tileSize * 2;

            var cameraPosition = camera.Position;
            var startX = (int)((cameraPosition.X - bufferSize.X) / tileSize.X) * tileSize.X;
            var startY = (int)((cameraPosition.Y - bufferSize.Y) / tileSize.Y) * tileSize.Y;
            var endX = (int)((cameraPosition.X + viewportSize.X + bufferSize.X) / tileSize.X) * tileSize.X;
            var endY = (int)((cameraPosition.Y + viewportSize.Y + bufferSize.Y) / tileSize.Y) * tileSize.Y;

            for (var y = startY; y <= endY; y += (int)tileSize.Y)
            {
                for (var x = startX; x <= endX; x += (int)tileSize.X)
                {
                    position = new Vector2(x, y);
                    Vector2 drawPosition = position - offset;
                    visualElement.Draw(spriteBatch, drawPosition, new Vector2(spriteWidth/2, spriteHeight/2), scale, rotation);
                }
            }
        }
    }
}