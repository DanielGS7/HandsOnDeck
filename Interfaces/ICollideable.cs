using Microsoft.Xna.Framework;

namespace HandsOnDeck2.Interfaces
{
    public interface ICollideable : IGameObject
    {
        bool IsColliding { get; set; }
        void OnCollision(ICollideable other);
    }
}