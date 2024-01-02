using HandsOnDeck.Classes.Collisions;
using HandsOnDeck.Classes.Object;
using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Classes
{
    internal class CollisionHandler
    {
        internal List<GameObject> gameObjects;
        private static CollisionHandler collisionHandler;
        internal static CollisionHandler Instance => collisionHandler ??= new CollisionHandler();
        private CollisionHandler()
        {
            this.gameObjects = new List<GameObject>();
        }

        internal void AddHitbox(GameObject gameObject)
        {
            this.gameObjects.Add(gameObject);
        }

        internal List<GameObject> CheckForCollisions(GameObject collider)
        {
            List<GameObject> collisions = new();
            foreach (var gameObject in gameObjects)
            {
                if (gameObject!= collider && collider.Hitbox.bounds.Intersects(gameObject.Hitbox.bounds))
                {
                    collisions.Add(gameObject);
                }
            }
            return collisions;
        }
    }
    
}
