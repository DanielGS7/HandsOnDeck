using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HandsOnDeck2.Classes
{
    public class Checkbox : UIElement
    {
        private Texture2D checkboxTexture;
        private Texture2D checkmarkTexture;
        private bool isChecked;
        private string label;
        private SpriteFont font;
        private bool isMouseOver;
        private bool wasMousePressed;

        public Checkbox(ContentManager content, Vector2 positionPercentage, Vector2 sizePercentage, string label)
            : base(positionPercentage, sizePercentage, 0f)
        {
            this.label = label;
            checkboxTexture = content.Load<Texture2D>("UI\\cannon_empty");
            checkmarkTexture = content.Load<Texture2D>("UI\\cannon_loaded");
            font = content.Load<SpriteFont>("default");
            isChecked = false;
            isMouseOver = false;
            wasMousePressed = false;
        }

        public override void Update(GameTime gameTime, Viewport viewport)
        {
            base.Update(gameTime, viewport);
            MouseState mouseState = Mouse.GetState();

            bool isMousePressed = mouseState.LeftButton == ButtonState.Pressed;
            isMouseOver = bounds.Contains(mouseState.Position);

            if (isMouseOver && isMousePressed && !wasMousePressed)
            {
                isChecked = !isChecked;
            }

            wasMousePressed = isMousePressed;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(checkboxTexture, bounds, Color.White);
            if (isChecked)
            {
                spriteBatch.Draw(checkmarkTexture, bounds, Color.White);
            }
            spriteBatch.DrawString(font, label, new Vector2(bounds.X + bounds.Width + 10, bounds.Y), Color.White);
        }

        public bool IsChecked()
        {
            return isChecked;
        }
    }
}