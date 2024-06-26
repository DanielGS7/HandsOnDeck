using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck2.Classes
{
    public class VisualElement
    {
        private Texture2D texture;
        private Vector2 position;
        private Vector2 origin;
        private float scale;
        private float rotation;
        private Color color;
        private SpriteEffects spriteEffects;
        private float layerDepth;
        private Rectangle? sourceRectangle;
        private Animation animation;

        public Vector2 Origin => origin;
        public float Rotation => rotation;

        public Vector2 Position { get => position; set => position = value; }
        public float Scale { get => scale; set => scale = value; }

        public VisualElement(Texture2D texture, Vector2 position, Vector2 origin, float scale, float rotation, Color color, SpriteEffects spriteEffects, float layerDepth, Rectangle? sourceRectangle = null)
        {
            this.texture = texture;
            this.Position = position;
            this.origin = origin;
            this.Scale = scale;
            this.rotation = rotation;
            this.color = color;
            this.spriteEffects = spriteEffects;
            this.layerDepth = layerDepth;
            this.sourceRectangle = sourceRectangle;
        }

        public VisualElement(Animation animation, Vector2 position, Vector2 origin, float scale, float rotation, Color color, SpriteEffects spriteEffects, float layerDepth)
        {
            this.animation = animation;
            this.Position = position;
            this.origin = origin;
            this.Scale = scale;
            this.rotation = rotation;
            this.color = color;
            this.spriteEffects = spriteEffects;
            this.layerDepth = layerDepth;
        }

        public void Update(GameTime gameTime)
        {
            animation?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (animation != null)
            {
                animation.Draw(spriteBatch, Position, Scale, rotation, origin, color, spriteEffects, layerDepth);
            }
            else
            {
                spriteBatch.Draw(texture, Position, sourceRectangle, color, rotation, origin, Scale, spriteEffects, layerDepth);
            }
        }

        public void SetPosition(Vector2 newPosition)
        {
            Position = newPosition;
        }

        public void SetRotation(float newRotation)
        {
            rotation = newRotation;
        }

        public void SetScale(float newScale)
        {
            Scale = newScale;
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
