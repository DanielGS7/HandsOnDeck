using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck2.Classes
{
    public class TextElement : UIElement
    {
        private SpriteFont font;
        private string text;
        private Color color;
        private float textScale;

        public TextElement(ContentManager content, string text, Vector2 positionPercentage, float rotation, Color color, float textScale = 1f)
            : base(positionPercentage, Vector2.Zero, rotation)
        {
            this.text = text;
            this.color = color;
            this.textScale = textScale;
            font = content.Load<SpriteFont>("MyCatholicon");
        }

        public override void Update(GameTime gameTime, Viewport viewport)
        {
            base.Update(gameTime, viewport);
            size = font.MeasureString(text) * textScale;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = size / 4f;
            spriteBatch.DrawString(font, text, position, color, rotation, origin, textScale, SpriteEffects.None, 0f);
        }

        public void SetText(string newText)
        {
            text = newText;
        }

        public void SetColor(Color newColor)
        {
            color = newColor;
        }

        public void SetScale(float newScale)
        {
            textScale = newScale;
        }
    }
}