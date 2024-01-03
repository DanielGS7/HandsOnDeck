using HandsOnDeck.Classes.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Classes.UI
{
    internal class Button : UIInteractable
    {
        private static Texture2D defaultTexture;

        private Rectangle bounds;
        private string text;
        private Action onPress;

        public static void LoadDefaultTexture(Texture2D texture)
        {
            defaultTexture = texture;
        }

        public Button(Vector2 centerPosition, float buttonHeight, string text, Action onPress)
        {
            this.text = text;
            this.onPress = onPress;

            Vector2 textSize = Game1.DefaultFont.MeasureString(text);

            this.bounds = new Rectangle(
                (int)(centerPosition.X - (textSize.X + 20) / 2),
                (int)(centerPosition.Y - buttonHeight / 2),
                (int)(textSize.X + 20),
                (int)buttonHeight);
        }

        public override void Update(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && bounds.Contains(Mouse.GetState().Position))
            {
                onPress?.Invoke();
            }
        }

        public override void Draw()
        {
            Renderer.GetInstance._spriteBatch.Draw(defaultTexture, bounds, Color.White);
            Vector2 textPosition = new Vector2(bounds.X + (bounds.Width - Game1.DefaultFont.MeasureString(text).X) / 2, bounds.Y + (bounds.Height - Game1.DefaultFont.MeasureString(text).Y) / 2);
            Renderer.GetInstance._spriteBatch.DrawString(Game1.DefaultFont, text, textPosition, Color.Black);
        }
    }
}
