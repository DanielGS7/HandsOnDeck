using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck2.Interfaces;
using System.Collections.Generic;
using System;
using HandsOnDeck2.Classes.CodeAccess;
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
            // Temporarily set the new position
            SeaCoordinate originalPosition = a.Position;
            a.Position = new SeaCoordinate(nextPosition.X, nextPosition.Y);

            // Check for collision at the new position
            bool collisionDetected = CheckCollision(a, b);

            // Restore the original position
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
            foreach (var collideable in collideables)
            {
                collideable.IsColliding = false;
            }

            for (int i = 0; i < collideables.Count; i++)
            {
                for (int j = i + 1; j < collideables.Count; j++)
                {
                    if (CheckCollision(collideables[i], collideables[j]))
                    {
                        collideables[i].IsColliding = true;
                        collideables[j].IsColliding = true;
                        collideables[i].OnCollision(collideables[j]);
                        collideables[j].OnCollision(collideables[i]);
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
                if (IsPointInPolygon(cornersA[i], cornersB) || IsPointInPolygon(cornersB[i], cornersA))
                {
                    return true;
                }
            }

            return false;
        }

        private Vector2[] GetRotatedRectangleCorners(ICollideable collideable)
        {
            Vector2 size = collideable.Size * collideable.Scale;
            Vector2 origin = collideable.Origin * collideable.Scale;
            float rotation = collideable.Rotation;

            Vector2[] corners = new Vector2[4];
            Vector2 topLeft = new Vector2(-origin.X, -origin.Y);
            Vector2 topRight = new Vector2(size.X - origin.X, -origin.Y);
            Vector2 bottomRight = new Vector2(size.X - origin.X, size.Y - origin.Y);
            Vector2 bottomLeft = new Vector2(-origin.X, size.Y - origin.Y);

            Vector2 position = collideable.Position.ToVector2();
            corners[0] = Vector2.Transform(topLeft, Matrix.CreateRotationZ(rotation)) + position;
            corners[1] = Vector2.Transform(topRight, Matrix.CreateRotationZ(rotation)) + position;
            corners[2] = Vector2.Transform(bottomRight, Matrix.CreateRotationZ(rotation)) + position;
            corners[3] = Vector2.Transform(bottomLeft, Matrix.CreateRotationZ(rotation)) + position;

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