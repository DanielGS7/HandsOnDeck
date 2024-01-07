using HandsOnDeck.Classes.Object;
using System.Collections.Generic;

namespace HandsOnDeck.Classes.Managers
{
    public class CollisionManager
    {
        private static CollisionManager instance;
        private static readonly object lockObject = new object();

        internal List<CollideableGameObject> gameObjects;
        public static CollisionManager GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                            instance = new CollisionManager();
                    }
                }
                return instance;
            }
        }

        private CollisionManager()
        {
            gameObjects = new List<CollideableGameObject>();
        }

        internal void AddCollideableObject(CollideableGameObject gameObject)
        {
            gameObjects.Add(gameObject);
        }

        internal void RemoveCollideableObject(CollideableGameObject gameObject)
        {
            gameObjects.Remove(gameObject);
        }

        internal void CheckForCollisions()
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                for (int j = i + 1; j < gameObjects.Count; j++)
                {
                    var gameObjectA = gameObjects[i];
                    var gameObjectB = gameObjects[j];

                    if (gameObjectA.Hitbox.bounds.Intersects(gameObjectB.Hitbox.bounds))
                    {
                        gameObjectA.onCollision(gameObjectB);
                        gameObjectB.onCollision(gameObjectA);
                    }
                }
            }
        }
    }

}
