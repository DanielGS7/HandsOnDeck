using Microsoft.Xna.Framework;
using HandsOnDeck2.Classes;

namespace HandsOnDeck2.Interfaces
{
    public interface IEntity : IGameObject
    {
        float Speed { get; set; }
    }
}