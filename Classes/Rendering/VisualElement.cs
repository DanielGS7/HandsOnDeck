using HandsOnDeck2.Classes.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck2.Classes.Rendering
{
    public class VisualElement
    {
        private Texture2D texture;
        private Color color;
        private SpriteEffects spriteEffects;
        private float layerDepth;
        private Rectangle? sourceRectangle;
        internal Animation animation;

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

        public void Draw(SpriteBatch spriteBatch, SeaCoordinate position, Vector2 origin, float scale, float rotation)
        {
            Vector2[] drawPositions = GetDrawPositions(position);

            foreach (Vector2 drawPosition in drawPositions)
            {
                if (animation != null)
                {
                    animation.Draw(spriteBatch, drawPosition, scale, rotation, origin, color, spriteEffects, layerDepth);
                }
                else
                {
                    spriteBatch.Draw(texture, drawPosition, sourceRectangle, color, rotation, origin, scale, spriteEffects, layerDepth);
                }
            }
        }

        private Vector2[] GetDrawPositions(SeaCoordinate position)
        {
            int mapWidth = Map.MapWidth;
            int mapHeight = Map.MapHeight;
            Vector2 pos = position.ToVector2();

            return new Vector2[]
            {
                pos,
                new Vector2(pos.X - mapWidth, pos.Y),
                new Vector2(pos.X + mapWidth, pos.Y),
                new Vector2(pos.X, pos.Y - mapHeight),
                new Vector2(pos.X, pos.Y + mapHeight),
                new Vector2(pos.X - mapWidth, pos.Y - mapHeight),
                new Vector2(pos.X + mapWidth, pos.Y - mapHeight),
                new Vector2(pos.X - mapWidth, pos.Y + mapHeight),
                new Vector2(pos.X + mapWidth, pos.Y + mapHeight)
            };
        }

        public void SetSourceRectangle(Rectangle? newSourceRectangle)
        {
            sourceRectangle = newSourceRectangle;
        }
    }
}