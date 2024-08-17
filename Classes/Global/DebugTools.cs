using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck2.Interfaces;

namespace HandsOnDeck2.Classes.Global
{
    public static class DebugTools
    {
        private static SpriteFont debugFont;
        private static Texture2D whiteTexture;
        private static int verticalOffset;

        public static void Initialize(GraphicsDevice graphicsDevice, ContentManager content)
        {
            debugFont = content.Load<SpriteFont>("default");
            whiteTexture = new Texture2D(graphicsDevice, 1, 1);
            whiteTexture.SetData(new[] { Color.White });
            verticalOffset = 0;
        }

        public static void ResetVerticalOffset()
        {
            verticalOffset = 0;
        }

        public static void IncrementVerticalOffset(int amount)
        {
            verticalOffset += amount;
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(whiteTexture, rectangle, color * 0.5f);
        }

        public static void DrawText(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(debugFont, text, position, color);
        }

        public static void DrawObjectInfo(SpriteBatch spriteBatch, Vector2 position, string info, Color color)
        {
            int lineHeight = 20;
            string positionText = $"Pos: {position.X:F2}, {position.Y:F2}";
            spriteBatch.DrawString(debugFont, positionText, new Vector2(10, 10 + verticalOffset), color);
            spriteBatch.DrawString(debugFont, info, new Vector2(10, 10 + verticalOffset + lineHeight), color);
            IncrementVerticalOffset(lineHeight * 2);
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, IGameObject gameObject, Color color)
        {
            var position = gameObject.Position;
            var size = gameObject.Size * gameObject.Scale;
            var rotation = gameObject.Rotation;

            Vector2 origin = new Vector2(size.X / 2, size.Y / 2);

            var adjustedPosition = position;

            Rectangle rect = new Rectangle((int)adjustedPosition.X, (int)adjustedPosition.Y, (int)size.X, (int)size.Y);

            spriteBatch.Draw(whiteTexture, adjustedPosition, rect, color * 0.5f, rotation, origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}
