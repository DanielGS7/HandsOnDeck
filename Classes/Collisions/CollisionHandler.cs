using HandsOnDeck.Classes.Object;
using System.Collections.Generic;

namespace HandsOnDeck.Classes
{
    public class CollisionHandler
    {
        internal List<CollideableGameObject> gameObjects;
        private static CollisionHandler collisionHandler;
        internal static CollisionHandler Instance => collisionHandler ??= new CollisionHandler();

        private CollisionHandler()
        {
            this.gameObjects = new List<CollideableGameObject>();
        }

        internal void AddHitbox(CollideableGameObject gameObject)
        {
            this.gameObjects.Add(gameObject);
        }

        internal List<CollideableGameObject> CheckForCollisions(CollideableGameObject collider)
        {
            List<CollideableGameObject> collisions = new();
            foreach (var gameObject in gameObjects)
            {
                if (gameObject != collider && collider.Hitbox.bounds.Intersects(gameObject.Hitbox.bounds))
                {
                    collisions.Add(gameObject);
                }
            }
            return collisions;
        }
    }

}
