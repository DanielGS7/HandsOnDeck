using HandsOnDeck2.Classes;
using HandsOnDeck2.Classes.Collisions;
using Microsoft.Xna.Framework;

namespace HandsOnDeck2.Interfaces
{
    public interface ICollideable : IGameObject
    {
        Collider Collider { get;set; }
        void OnCollision(ICollideable other);
        void OnTriggerEnter(ICollideable other);
        void OnTriggerExit(ICollideable other);
    }
}
