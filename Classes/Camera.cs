using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck2.Classes
{
    public class Camera
    {
        public Matrix Transform { get; private set; }
        public Vector2 Position { get; private set; }


        public Camera()
        {
            Position = Vector2.Zero;
        }

        public void Update(Vector2 targetPosition, Viewport viewport, int mapWidth, int mapHeight)
        {
            float viewportWidth = viewport.Width;
            float viewportHeight = viewport.Height;
            float bufferX = viewportHeight * 0.28f;
            float bufferY = viewportHeight * 0.28f;

            float leftBound = Position.X + bufferX;
            float rightBound = Position.X + viewportWidth - bufferX;
            float topBound = Position.Y + bufferY;
            float bottomBound = Position.Y + viewportHeight - bufferY;

            Vector2 targetCameraPosition = Position;

            if (targetPosition.X < leftBound)
            {
                targetCameraPosition.X = targetPosition.X - bufferX;
            }
            else if (targetPosition.X > rightBound)
            {
                targetCameraPosition.X = targetPosition.X - viewportWidth + bufferX;
            }

            if (targetPosition.Y < topBound)
            {
                targetCameraPosition.Y = targetPosition.Y - bufferY;
            }
            else if (targetPosition.Y > bottomBound)
            {
                targetCameraPosition.Y = targetPosition.Y - viewportHeight + bufferY;
            }

            if (targetCameraPosition.X < 0)
            {
                targetCameraPosition.X += mapWidth;
            }
            else if (targetCameraPosition.X >= mapWidth)
            {
                targetCameraPosition.X -= mapWidth;
            }

            if (targetCameraPosition.Y < 0)
            {
                targetCameraPosition.Y += mapHeight;
            }
            else if (targetCameraPosition.Y >= mapHeight)
            {
                targetCameraPosition.Y -= mapHeight;
            }

            Position = targetCameraPosition;

            Transform = Matrix.CreateTranslation(new Vector3(-Position, 0));
        }
    }
}
