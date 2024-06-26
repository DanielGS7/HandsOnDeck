using Microsoft.Xna.Framework;
using System.Collections.Generic;
using HandsOnDeck2.Interfaces;

namespace HandsOnDeck2.Classes.Collisions
{
    public class CollisionManager
    {
        private static CollisionManager instance;
        private List<ICollideable> collideables;

        private CollisionManager()
        {
            collideables = new List<ICollideable>();
        }

        public static CollisionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CollisionManager();
                }
                return instance;
            }
        }

        public void AddCollideable(ICollideable collideable)
        {
            if (!collideables.Contains(collideable))
            {
                collideables.Add(collideable);
            }
        }

        public void RemoveCollideable(ICollideable collideable)
        {
            if (collideables.Contains(collideable))
            {
                collideables.Remove(collideable);
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < collideables.Count; i++)
            {
                var collideableA = collideables[i];

                for (int j = i + 1; j < collideables.Count; j++)
                {
                    var collideableB = collideables[j];

                    if (collideableA.Collider.Bounds.Intersects(collideableB.Collider.Bounds))
                    {
                        if (collideableA.Collider.IsTrigger || collideableB.Collider.IsTrigger)
                        {
                            collideableA.OnTriggerEnter(collideableB);
                            collideableB.OnTriggerEnter(collideableA);

                            collideableA.Collider.Color = Color.Yellow;
                            collideableB.Collider.Color = Color.Yellow;
                        }
                        else
                        {
                            collideableA.OnCollision(collideableB);
                            collideableB.OnCollision(collideableA);

                            collideableA.Collider.Color = Color.Red;
                            collideableB.Collider.Color = Color.Red;
                        }
                    }
                }
            }
        }
    }
}
