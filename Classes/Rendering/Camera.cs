using HandsOnDeck2.Classes.CodeAccess;
using HandsOnDeck2.Classes.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HandsOnDeck2.Classes
{
    public class Camera
    {
        public Matrix Transform { get; private set; }
        public Vector2 Position { get; private set; }

        private Vector2 previousPlayerPosition;

        public Camera()
        {
            Position = Vector2.Zero;
            previousPlayerPosition = Vector2.Zero;
        }

        public void Update(Vector2 playerPosition, Viewport viewport, int mapWidth, int mapHeight)
        {
            Vector2 targetCameraPosition = Position;

            float bufferX = viewport.Width * 0.28f;
            float bufferY = viewport.Height * 0.28f;

            float leftBound = Position.X + bufferX;
            float rightBound = Position.X + viewport.Width - bufferX;
            float topBound = Position.Y + bufferY;
            float bottomBound = Position.Y + viewport.Height - bufferY;

            if (HasCrossedEdge(previousPlayerPosition, playerPosition, mapWidth, mapHeight))
            {
                Vector2 offset = Position - previousPlayerPosition;
                Position = playerPosition + offset;
            }

            else
            {
                if (playerPosition.X < leftBound)
                {
                    targetCameraPosition.X = playerPosition.X - bufferX;
                }
                else if (playerPosition.X > rightBound)
                {
                    targetCameraPosition.X = playerPosition.X - viewport.Width + bufferX;
                }

                if (playerPosition.Y < topBound)
                {
                    targetCameraPosition.Y = playerPosition.Y - bufferY;
                }
                else if (playerPosition.Y > bottomBound)
                {
                    targetCameraPosition.Y = playerPosition.Y - viewport.Height + bufferY;
                }
                Position = targetCameraPosition;
            }

            Transform = Matrix.CreateTranslation(new Vector3(-Position, 0));

            previousPlayerPosition = playerPosition;
        }

        private bool HasCrossedEdge(Vector2 oldPos, Vector2 newPos, int mapWidth, int mapHeight)
        {
            return Math.Abs(newPos.X - oldPos.X) > mapWidth / 2 ||
                   Math.Abs(newPos.Y - oldPos.Y) > mapHeight / 2;
        }

        public Rectangle GetViewport()
        {
            return new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                GraphDev.GetInstance.Viewport.Width,
                GraphDev.GetInstance.Viewport.Height
            );
        }
    }
}