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

        protected void Draw9Slice(Texture2D texture, Rectangle destination, Rectangle? source = null, int borderSize = 0)
        {
            int textureWidth = source?.Width ?? texture.Width;
            int textureHeight = source?.Height ?? texture.Height;
            int centerWidth = textureWidth - borderSize * 2;
            int centerHeight = textureHeight - borderSize * 2;
            int rightSide = textureWidth - borderSize;
            int bottomSide = textureHeight - borderSize;
            int destCenterWidth = destination.Width - borderSize * 2;
            int destCenterHeight = destination.Height - borderSize * 2;

            Rectangle topLeftSource = new Rectangle(source?.X ?? 0, source?.Y ?? 0, borderSize, borderSize);
            Rectangle topRightSource = new Rectangle((source?.X ?? 0) + rightSide, source?.Y ?? 0, borderSize, borderSize);
            Rectangle bottomLeftSource = new Rectangle(source?.X ?? 0, (source?.Y ?? 0) + bottomSide, borderSize, borderSize);
            Rectangle bottomRightSource = new Rectangle((source?.X ?? 0) + rightSide, (source?.Y ?? 0) + bottomSide, borderSize, borderSize);
            Rectangle leftEdgeSource = new Rectangle(source?.X ?? 0, (source?.Y ?? 0) + borderSize, borderSize, centerHeight);
            Rectangle rightEdgeSource = new Rectangle((source?.X ?? 0) + rightSide, (source?.Y ?? 0) + borderSize, borderSize, centerHeight);
            Rectangle topEdgeSource = new Rectangle((source?.X ?? 0) + borderSize, source?.Y ?? 0, centerWidth, borderSize);
            Rectangle bottomEdgeSource = new Rectangle((source?.X ?? 0) + borderSize, (source?.Y ?? 0) + bottomSide, centerWidth, borderSize);
            Rectangle centerSource = new Rectangle((source?.X ?? 0) + borderSize, (source?.Y ?? 0) + borderSize, centerWidth, centerHeight);

            // corners
            SpriteBatchManager.Instance.Draw(texture, new Rectangle(destination.X, destination.Y, borderSize, borderSize), topLeftSource, Color.White);
            SpriteBatchManager.Instance.Draw(texture, new Rectangle(destination.Right - borderSize, destination.Y, borderSize, borderSize), topRightSource, Color.White);
            SpriteBatchManager.Instance.Draw(texture, new Rectangle(destination.X, destination.Bottom - borderSize, borderSize, borderSize), bottomLeftSource, Color.White);
            SpriteBatchManager.Instance.Draw(texture, new Rectangle(destination.Right - borderSize, destination.Bottom - borderSize, borderSize, borderSize), bottomRightSource, Color.White);

            // edges
            SpriteBatchManager.Instance.Draw(texture, new Rectangle(destination.X + borderSize, destination.Y, destCenterWidth, borderSize), topEdgeSource, Color.White);
            SpriteBatchManager.Instance.Draw(texture, new Rectangle(destination.X + borderSize, destination.Bottom - borderSize, destCenterWidth, borderSize), bottomEdgeSource, Color.White);
            SpriteBatchManager.Instance.Draw(texture, new Rectangle(destination.X, destination.Y + borderSize, borderSize, destCenterHeight), leftEdgeSource, Color.White);
            SpriteBatchManager.Instance.Draw(texture, new Rectangle(destination.Right - borderSize, destination.Y + borderSize, borderSize, destCenterHeight), rightEdgeSource, Color.White);

            // center
            SpriteBatchManager.Instance.Draw(texture, new Rectangle(destination.X + borderSize, destination.Y + borderSize, destCenterWidth, destCenterHeight), centerSource, Color.White);
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
