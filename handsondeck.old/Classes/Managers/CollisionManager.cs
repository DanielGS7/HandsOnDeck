using HandsOnDeck.Classes.Object;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck.Classes.Collisions;
using Microsoft.Xna.Framework;

namespace HandsOnDeck.Classes.Managers
{
    public class CollisionManager
    {
        private static CollisionManager instance;
        private static readonly object lockObject = new object();

        internal List<CollideableGameObject> gameObjects;
        readonly HashSet<Tuple<Hitbox, Hitbox>> recordedCollisions = new HashSet<Tuple<Hitbox, Hitbox>>();
        private CollisionVisualizer visualizer;

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

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            visualizer = new CollisionVisualizer(graphicsDevice);
        }

        internal void AddCollideableObject(CollideableGameObject gameObject)
        {
            gameObjects.Add(gameObject);
            visualizer.AddHitbox(gameObject.Hitbox);
        }

        internal void RemoveCollideableObject(CollideableGameObject gameObject)
        {
            gameObjects.Remove(gameObject);
            visualizer.RemoveHitbox(gameObject.Hitbox);
        }

        internal void CheckForCollisions()
        {
            var currentCollisions = new List<Tuple<Hitbox, Hitbox>>();

            for (int i = 0; i < gameObjects.Count; i++)
            {
                for (int j = i + 1; j < gameObjects.Count; j++)
                {
                    var gameObjectA = gameObjects[i];
                    var gameObjectB = gameObjects[j];
                    var hitboxA = gameObjectA.Hitbox;
                    var hitboxB = gameObjectB.Hitbox;
                    var collisionPair = new Tuple<Hitbox, Hitbox>(hitboxA, hitboxB);

                    if (hitboxA.bounds.Intersects(hitboxB.bounds))
                    {
                        if (!recordedCollisions.Contains(collisionPair))
                        {
                            gameObjectA.onCollision(gameObjectB);
                            gameObjectB.onCollision(gameObjectA);
                            recordedCollisions.Add(collisionPair);
                            currentCollisions.Add(collisionPair);
                        }
                    }
                    else
                    {
                        recordedCollisions.Remove(collisionPair);
                    }
                }
            }

        }
        public void DrawVisualizations(WorldCoordinate viewportPosition)
        {
            visualizer.Draw(viewportPosition);
        }
    }
}
