using HandsOnDeck.Classes.Collisions;
using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Enums;
using HandsOnDeck.Interfaces;

namespace HandsOnDeck.Classes.Object
{
    public abstract class CollideableGameObject : GameObject, ICollideable
    {
        public abstract void onCollision(CollideableGameObject other);
        public Hitbox Hitbox { get; set; }
    }
}