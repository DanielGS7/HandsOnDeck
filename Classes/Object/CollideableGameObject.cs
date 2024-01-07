using HandsOnDeck.Classes.Collisions;
using HandsOnDeck.Enums;
using HandsOnDeck.Interfaces;

namespace HandsOnDeck.Classes.Object
{
    public abstract class CollideableGameObject : GameObject, ICollideable
    {
        public CollisionHandler CollisionHandler { get; set; }
        public Hitbox Hitbox { get; set; }
        public HitboxType Type { get; set; }
    }
}