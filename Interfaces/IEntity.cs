using Microsoft.Xna.Framework;
using HandsOnDeck2.Classes;

namespace HandsOnDeck2.Interfaces
{
    public interface IEntity : IGameObject
    {
        Vector2 Size { get; set; }
        float Speed { get; set; }
        VisualElement VisualElement { get; set; }
        Vector2 Position { get; set; }
    }
}
