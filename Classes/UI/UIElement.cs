using HandsOnDeck2.Classes.CodeAccess;
using HandsOnDeck2.Classes.Rendering;
using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HandsOnDeck2.Classes
{
    public abstract class UIElement : IUIInteractable
    {
        protected Vector2 positionPercentage;
        protected Vector2 sizePercentage;
        protected Vector2 position;
        protected Vector2 size;
        protected Rectangle bounds;
        protected float rotation;
        protected float scale;

        public VisualElement VisualElement { get; set; }

        protected UIElement(Vector2 positionPercentage, Vector2 sizePercentage, float rotation)
        {
            this.positionPercentage = positionPercentage;
            this.sizePercentage = sizePercentage;
            this.rotation = rotation;
            this.scale = 1f;
        }

        public virtual void Update(GameTime gameTime, Viewport viewport)
        {
            position = new Vector2(
                viewport.Width * positionPercentage.X,
                viewport.Height * positionPercentage.Y
            );
            size = new Vector2(
                viewport.Width * sizePercentage.X,
                viewport.Height * sizePercentage.Y
            );

            bounds = new Rectangle(
                (int)(position.X - size.X / 2),
                (int)(position.Y - size.Y / 2),
                (int)size.X,
                (int)size.Y
            );
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        public virtual void HandleInput(MouseState mouseState) { }

        public virtual void Update(GameTime gameTime)
        {
            Update(gameTime, GraphDev.GetInstance.Viewport);
        }
    }
}