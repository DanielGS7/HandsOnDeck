using HandsOnDeck2.Classes.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace HandsOnDeck2.Classes
{
    public class Button : UIElement
    {
        private Texture2D texture;
        private SpriteFont font;
        private string text;
        private const float TextScaleFactor = 0.5f;
        private Action onClick;
        private bool isHovered;
        private bool isPressed;
        private float clickTimer;

        private const float BaseScale = 1.0f;
        private const float HoverScale = 1.1f;
        private const float ClickScale = 0.9f;
        private const float ClickDelay = 0.15f;

        public Button(ContentManager content, string text, Vector2 positionPercentage, Vector2 sizePercentage, float rotation, string textureName, Action onClick)
            : base(positionPercentage, sizePercentage, rotation)
        {
            this.text = text;
            this.onClick = onClick;

            texture = content.Load<Texture2D>(textureName);
            font = content.Load<SpriteFont>("default");

            VisualElement = new VisualElement(texture, Color.White, SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime, Viewport viewport)
        {
            base.Update(gameTime, viewport);

            MouseState mouseState = Mouse.GetState();
            Point mousePosition = mouseState.Position;

            isHovered = bounds.Contains(mousePosition);

            if (clickTimer > 0)
            {
                clickTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (clickTimer <= 0)
                {
                    onClick?.Invoke();
                }
            }
            else if (isHovered)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    isPressed = true;
                    scale = MathHelper.Lerp(scale, ClickScale, 0.25f);
                }
                else if (mouseState.LeftButton == ButtonState.Released && isPressed)
                {
                    isPressed = false;
                    clickTimer = ClickDelay;
                }
                else
                {
                    scale = MathHelper.Lerp(scale, HoverScale, 0.25f);
                }
            }
            else
            {
                isPressed = false;
                scale = MathHelper.Lerp(scale, BaseScale, 0.25f);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            float buttonScale = Math.Min(size.X / texture.Width, size.Y / texture.Height) * scale;

            spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, buttonScale, SpriteEffects.None, 0f);

            Vector2 textSize = font.MeasureString(text);
            float textScaleX = size.X * TextScaleFactor / textSize.X;
            float textScaleY = size.Y * TextScaleFactor / textSize.Y;
            float textScale = Math.Min(textScaleX, textScaleY);
            
            Vector2 textPosition = position;
            Vector2 textOrigin = textSize / 2f;
            spriteBatch.DrawString(font, text, textPosition, Color.Black, rotation, textOrigin, textScale, SpriteEffects.None, 0f);

            if (isHovered)
            {
                spriteBatch.Draw(texture, position, null, Color.White * 0.3f, rotation, origin, buttonScale, SpriteEffects.None, 0f);
            }
        }
    }
}