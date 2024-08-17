using HandsOnDeck2.Classes.Global;
using HandsOnDeck2.Classes.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck2.Interfaces
{
    public interface IGameObject
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);

        VisualElement VisualElement { get; set; }
        SeaCoordinate Position { get; set; }
        Vector2 Size { get; set; }
        Vector2 Origin { get; set; }
        float Scale { get; set; }
        float Rotation { get; set; }
    }
}