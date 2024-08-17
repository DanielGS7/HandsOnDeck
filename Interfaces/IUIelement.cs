using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using HandsOnDeck2.Classes.Rendering;

namespace HandsOnDeck2.Interfaces
{
    public interface IUIElement
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        VisualElement VisualElement { get; set; }
    }
}
