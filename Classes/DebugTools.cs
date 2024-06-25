using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck2.Interfaces;

namespace HandsOnDeck2.Classes
{
    public static class DebugTools
    {
        private static SpriteFont debugFont;
        private static Texture2D whiteTexture;

        public static void Initialize(GraphicsDevice graphicsDevice, ContentManager content)
        {
            debugFont = content.Load<SpriteFont>("default");
            whiteTexture = new Texture2D(graphicsDevice, 1, 1);
            whiteTexture.SetData(new[] { Color.White });
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(whiteTexture, rectangle, color * 0.5f); // Semi-transparent
        }

        public static void DrawText(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(debugFont, text, position, color);
        }

        public static void DrawObjectInfo(SpriteBatch spriteBatch, Vector2 position, string info, Color color)
        {
            string positionText = $"Pos: {position.X:F2}, {position.Y:F2}";
            spriteBatch.DrawString(debugFont, positionText, new Vector2(10, 10), color);
            spriteBatch.DrawString(debugFont, info, new Vector2(10, 30), color);
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, IEntity entity, Color color)
        {
            var position = entity.Position;
            var size = entity.Size * entity.VisualElement.Scale;
            var rotation = entity.VisualElement.Rotation;

            Vector2 origin = new Vector2(size.X / 2, size.Y / 2);

            // Adjust position to account for origin
            var adjustedPosition = position;

            Rectangle rect = new Rectangle((int)adjustedPosition.X, (int)adjustedPosition.Y, (int)size.X, (int)size.Y);

            // Draw the rectangle with the correct origin and rotation
            spriteBatch.Draw(whiteTexture, adjustedPosition, rect, color * 0.5f, rotation, origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}
