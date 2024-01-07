using HandsOnDeck.Classes.UI.UIElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace HandsOnDeck.Classes.UI.Screens
{
    public abstract class UIScreen
    {
        protected List<UIElement> uiElements;
        protected Texture2D buttonTexture;
        public UIScreen()
        {
            uiElements = new List<UIElement>();
        }

        public virtual void Initialize()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var element in uiElements)
            {
                element.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            foreach (var element in uiElements)
            {
                element.Draw(gameTime);
            }
        }

        protected void AddUIElement(UIElement element)
        {
            uiElements.Add(element);
        }

        internal abstract void LoadContent();
    }
}
