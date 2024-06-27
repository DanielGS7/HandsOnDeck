using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HandsOnDeck2.Classes.UI
{
    public class Button : IUIInteractable
    {
        private Texture2D texture;
        private Rectangle bounds;
        private bool isHovered;
        private Color color;

        public VisualElement VisualElement { get  ; set  ; }
        public Vector2 Position { get  ; set  ; }
        public Vector2 Size { get  ; set  ; }

        public Button(ContentManager content, Rectangle bounds, Vector2 position)
        {
            this.texture = content.Load<Texture2D>("button_rectangle_depth_gloss");
            this.bounds = bounds;
            this.isHovered = false;
            this.color = Color.White;
            this.Position = position;
            this.Size = new Vector2(texture.Width, texture.Height);

            VisualElement = new VisualElement(texture, Position, new Vector2(bounds.X/2,bounds.Y/2), 1.0f, 0.0f, Color.White, SpriteEffects.None, 0.0f);
        }

        public void HandleInput(MouseState mouseState)
        {
            if (IsHovered(mouseState))
            {
                color = Color.Gray; // Change color on hover
                isHovered = true;

                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    // Handle click event
                }
            }
            else
            {
                color = Color.White;
                isHovered = false;
            }
        }

        public bool IsHovered(MouseState mouseState)
        {
            return bounds.Contains(mouseState.Position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            VisualElement.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            VisualElement.Update(gameTime);
        }
    }
}
