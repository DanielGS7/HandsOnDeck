using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck2.Interfaces;
using System.Collections.Generic;
using System;
using HandsOnDeck2.Classes.CodeAccess;
using HandsOnDeck2.Classes.Rendering;
using System.Linq;
using HandsOnDeck2.Classes.GameObject;
using HandsOnDeck2.Classes.Global;

namespace HandsOnDeck2.Classes.Collision
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

        public bool CheckCollisionAtNextPosition(ICollideable a, ICollideable b, Vector2 nextPosition)
        {
            SeaCoordinate originalPosition = a.Position;
            a.Position = new SeaCoordinate(nextPosition.X, nextPosition.Y);

            bool collisionDetected = CheckCollision(a, b);

            a.Position = originalPosition;

            return collisionDetected;
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
            collideables.Remove(collideable);
        }

        public void Reset()
        {
            collideables.Clear();
        }

        public void Update(GameTime gameTime)
        {
            var projectiles = Map.Instance.GetProjectiles();
            List<ICollideable> currentCollideables = collideables.ToList();
            foreach (var collideable in currentCollideables)
            {
                collideable.IsColliding = false;
            }

            for (int i = 0; i < currentCollideables.Count; i++)
            {
                for (int j = i + 1; j < currentCollideables.Count; j++)
                {
                    if (CheckCollision(currentCollideables[i], currentCollideables[j]))
                    {
                        currentCollideables[i].IsColliding = true;
                        currentCollideables[j].IsColliding = true;
                        currentCollideables[i].OnCollision(currentCollideables[j]);
                        currentCollideables[j].OnCollision(currentCollideables[i]);
                    }
                }
            }
            foreach (var projectile in projectiles)
            {
                if (projectile is ICollideable collideable)
                {
                    foreach (var other in currentCollideables)
                    {
                        if (other != collideable && CheckCollision(collideable, other))
                        {
                            collideable.OnCollision(other);
                            other.OnCollision(collideable);
                        }
                    }
                }
            }
        }

        private bool CheckCollision(ICollideable a, ICollideable b)
        {
            Vector2[] cornersA = GetRotatedRectangleCorners(a);
            Vector2[] cornersB = GetRotatedRectangleCorners(b);

            for (int i = 0; i < 4; i++)
            {
                if (IsPointInPolygon(cornersA[i], cornersB))
                    return true;
            }

            for (int i = 0; i < 4; i++)
            {
                if (IsPointInPolygon(cornersB[i], cornersA))
                    return true;
            }

            for (int i = 0; i < 4; i++)
            {
                int nextA = (i + 1) % 4;
                for (int j = 0; j < 4; j++)
                {
                    int nextB = (j + 1) % 4;
                    if (LineIntersects(cornersA[i], cornersA[nextA], cornersB[j], cornersB[nextB]))
                        return true;
                }
            }

            return false;
        }

        private Vector2[] GetRotatedRectangleCorners(ICollideable collideable)
        {
            Vector2 size;
            Vector2 position = collideable.Position.ToVector2();
            float rotation = collideable.Rotation;

            if (collideable is Island island)
            {
                size = island.ColliderSize;
            }
            else
            {
                size = collideable.Size * collideable.Scale;
            }

            Vector2 halfSize = size / 2;

            Vector2[] corners = new Vector2[4];
            corners[0] = Vector2.Transform(new Vector2(-halfSize.X, -halfSize.Y), Matrix.CreateRotationZ(rotation)) + position;
            corners[1] = Vector2.Transform(new Vector2(halfSize.X, -halfSize.Y), Matrix.CreateRotationZ(rotation)) + position;
            corners[2] = Vector2.Transform(new Vector2(halfSize.X, halfSize.Y), Matrix.CreateRotationZ(rotation)) + position;
            corners[3] = Vector2.Transform(new Vector2(-halfSize.X, halfSize.Y), Matrix.CreateRotationZ(rotation)) + position;

            return corners;
        }

        private bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
        {
            bool inside = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                if (polygon[i].Y > point.Y != polygon[j].Y > point.Y &&
                    point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X)
                {
                    inside = !inside;
                }
            }
            return inside;
        }

        private bool LineIntersects(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            Vector2 b = a2 - a1;
            Vector2 d = b2 - b1;
            float bDotDPerp = b.X * d.Y - b.Y * d.X;

            if (bDotDPerp == 0)
                return false;

            Vector2 c = b1 - a1;
            float t = (c.X * d.Y - c.Y * d.X) / bDotDPerp;
            if (t < 0 || t > 1)
                return false;

            float u = (c.X * b.Y - c.Y * b.X) / bDotDPerp;
            if (u < 0 || u > 1)
                return false;

            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!DrawColliders) return;

            foreach (var collideable in collideables)
            {
                DrawCollider(spriteBatch, collideable);
            }
        }

        private void DrawCollider(SpriteBatch spriteBatch, ICollideable collideable)
        {
            Vector2[] corners = GetRotatedRectangleCorners(collideable);
            Color color = collideable.IsColliding ? Color.Red : Color.Green;

            for (int i = 0; i < 4; i++)
            {
                int nextIndex = (i + 1) % 4;
                DrawLine(spriteBatch, corners[i], corners[nextIndex], color);
            }
        }

        private void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);
            spriteBatch.Draw(
                Texture2DHelper.GetWhiteTexture(GraphDev.GetInstance),
                new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), 1),
                null,
                color,
                angle,
                Vector2.Zero,
                SpriteEffects.None,
                0
            );
        }
    }
}