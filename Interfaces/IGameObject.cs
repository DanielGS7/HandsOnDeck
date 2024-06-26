using HandsOnDeck2.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck2.Interfaces
{
    public interface IGameObject
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        VisualElement VisualElement { get; set; }
        Vector2 Position { get; set; }
        Vector2 Size { get; set; }
    }
}
