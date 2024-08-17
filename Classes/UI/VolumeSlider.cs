using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace HandsOnDeck2.Classes.UI
{
    public class VolumeSlider : UIElement
    {
        private Texture2D backgroundTexture;
        private Texture2D knobTexture;
        private float value;
        private bool isDragging;
        private float knobScale = 1f;
        private float bottomOffset;

        public VolumeSlider(ContentManager content, Vector2 positionPercentage, Vector2 sizePercentage)
            : base(positionPercentage, sizePercentage, 0f)
        {
            backgroundTexture = content.Load<Texture2D>("wood_slider");
            knobTexture = content.Load<Texture2D>("wood_peg");
            value = 0.5f;
            bottomOffset = knobTexture.Height * 0.17f * knobScale;
        }

        public override void Update(GameTime gameTime, Viewport viewport)
        {
            base.Update(gameTime, viewport);
            MouseState mouseState = Mouse.GetState();
            if (bounds.Contains(mouseState.Position))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    isDragging = true;
                }
            }
            if (mouseState.LeftButton == ButtonState.Released)
            {
                isDragging = false;
            }
            if (isDragging)
            {
                value = MathHelper.Clamp((mouseState.X - bounds.X) / (float)bounds.Width, 0, 1);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, bounds, Color.White);

            int knobPosition = (int)(bounds.X + value * bounds.Width);
            int knobY = bounds.Bottom + (int)bottomOffset;

            Vector2 knobOrigin = new Vector2(knobTexture.Width / 2f, knobTexture.Height / 2f);
            Rectangle knobDestination = new Rectangle(knobPosition, knobY, (int)(knobTexture.Width * knobScale), (int)(knobTexture.Height * knobScale));

            spriteBatch.Draw(knobTexture, knobDestination, null, Color.White, 0f, knobOrigin, SpriteEffects.None, 0f);
        }

        public float GetValue()
        {
            return value;
        }
    }
}