using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck2.Classes
{
    public class VisualElement
    {
        private Texture2D texture;
        private Color color;
        private SpriteEffects spriteEffects;
        private float layerDepth;
        private Rectangle? sourceRectangle;
        private Animation animation;

        public VisualElement(Texture2D texture, Color color, SpriteEffects spriteEffects, float layerDepth, Rectangle? sourceRectangle = null)
        {
            this.texture = texture;
            this.color = color;
            this.spriteEffects = spriteEffects;
            this.layerDepth = layerDepth;
            this.sourceRectangle = sourceRectangle;
        }

        public VisualElement(Animation animation, Color color, SpriteEffects spriteEffects, float layerDepth)
        {
            this.animation = animation;
            this.color = color;
            this.spriteEffects = spriteEffects;
            this.layerDepth = layerDepth;
        }

        public void Update(GameTime gameTime)
        {
            animation?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 origin, float scale, float rotation)
        {
            if (animation != null)
            {
                animation.Draw(spriteBatch, position, scale, rotation, origin, color, spriteEffects, layerDepth);
            }
            else
            {
                spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, spriteEffects, layerDepth);
            }
        }

        public void SetColor(Color newColor)
        {
            color = newColor;
        }

        public void SetSourceRectangle(Rectangle? newSourceRectangle)
        {
            sourceRectangle = newSourceRectangle;
        }
    }
}
