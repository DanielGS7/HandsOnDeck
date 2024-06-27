using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck2.Interfaces;
using System.Collections.Generic;
using System;

namespace HandsOnDeck2.Classes.Collisions
{
    public class CollisionManager
    {
        private static CollisionManager instance;
        private List<ICollideable> collideables;
        public bool DrawColliders { get; set; } = true;

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

                    if (CheckCollision(collideableA, collideableB))
                    {
                        collideableA.Collider.Collides();
                        collideableB.Collider.Collides();
                        if (collideableA.Collider.IsTrigger || collideableB.Collider.IsTrigger)
                        {
                            collideableA.OnTriggerEnter(collideableB);
                            collideableB.OnTriggerEnter(collideableA);
                        }
                        else
                        {
                            collideableA.OnCollision(collideableB);
                            collideableB.OnCollision(collideableA);
                        }
                    }
                }
            }
        }

        private bool CheckCollision(ICollideable a, ICollideable b)
        {
            return CheckRotatedRectanglesCollision(
                a.Collider.Bounds, a.Rotation,
                b.Collider.Bounds, b.Rotation
            );
        }

        private bool CheckRotatedRectanglesCollision(Rectangle rectA, float rotationA, Rectangle rectB, float rotationB)
        {
            Vector2[] cornersA = GetRotatedRectangleCorners(rectA, rotationA);
            Vector2[] cornersB = GetRotatedRectangleCorners(rectB, rotationB);

            return CheckSeparatingAxisTheorem(cornersA, cornersB) && CheckSeparatingAxisTheorem(cornersB, cornersA);
        }

        private Vector2[] GetRotatedRectangleCorners(Rectangle rect, float rotation)
        {
            Vector2[] corners = new Vector2[4];
            Vector2 center = new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            Vector2[] originalCorners = {
                new Vector2(rect.X, rect.Y),
                new Vector2(rect.X + rect.Width, rect.Y),
                new Vector2(rect.X + rect.Width, rect.Y + rect.Height),
                new Vector2(rect.X, rect.Y + rect.Height)
            };

            for (int i = 0; i < 4; i++)
            {
                Vector2 corner = originalCorners[i] - center;
                float cos = (float)Math.Cos(rotation);
                float sin = (float)Math.Sin(rotation);
                corners[i] = new Vector2(
                    corner.X * cos - corner.Y * sin,
                    corner.X * sin + corner.Y * cos
                ) + center;
            }

            return corners;
        }

        private bool CheckSeparatingAxisTheorem(Vector2[] cornersA, Vector2[] cornersB)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector2 edge = cornersA[(i + 1) % 4] - cornersA[i];
                Vector2 axis = new Vector2(-edge.Y, edge.X);
                float minA = float.MaxValue, maxA = float.MinValue;
                float minB = float.MaxValue, maxB = float.MinValue;

                foreach (var corner in cornersA)
                {
                    float projection = Vector2.Dot(corner, axis);
                    minA = Math.Min(minA, projection);
                    maxA = Math.Max(maxA, projection);
                }

                foreach (var corner in cornersB)
                {
                    float projection = Vector2.Dot(corner, axis);
                    minB = Math.Min(minB, projection);
                    maxB = Math.Max(maxB, projection);
                }

                if (maxA < minB || maxB < minA)
                {
                    return false;
                }
            }

            return true;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (!DrawColliders) return;

            foreach (var collideable in collideables)
            {
                collideable.Collider.Draw(spriteBatch, graphicsDevice, collideable.Rotation);
            }
        }
    }
}
