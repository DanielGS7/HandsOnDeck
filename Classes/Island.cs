using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck2.Classes
{
    public class Island
    {
        private VisualElement visualElement;

        public Island(Texture2D texture, Vector2 position)
        {
            visualElement = new VisualElement(texture, position, new Vector2(texture.Width / 2, texture.Height / 2), 1f, 0f, Color.White, SpriteEffects.None, 0f);
        }

        public void Update(GameTime gameTime)
        {
            visualElement.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            visualElement.Draw(spriteBatch);
        }
    }
}
