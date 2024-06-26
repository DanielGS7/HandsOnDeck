using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

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
            Vector2 targetCameraPosition = Position;

            float bufferX = viewport.Height * 0.28f;
            float bufferY = viewport.Height * 0.28f;

            float leftBound = Position.X + bufferX;
            float rightBound = Position.X + viewport.Width - bufferX;
            float topBound = Position.Y + bufferY;
            float bottomBound = Position.Y + viewport.Height - bufferY;

            if (targetPosition.X < leftBound)
            {
                targetCameraPosition.X = targetPosition.X - bufferX;
            }
            else if (targetPosition.X > rightBound)
            {
                targetCameraPosition.X = targetPosition.X - viewport.Width + bufferX;
            }

            if (targetPosition.Y < topBound)
            {
                targetCameraPosition.Y = targetPosition.Y - bufferY;
            }
            else if (targetPosition.Y > bottomBound)
            {
                targetCameraPosition.Y = targetPosition.Y - viewport.Height + bufferY;
            }

            Position = targetCameraPosition;

            Transform = Matrix.CreateTranslation(new Vector3(-Position, 0));
        }

        public void AdjustPositionForTeleport(Vector2 previousPosition, Vector2 newPosition)
        {
            Debug.WriteLine("Previous Position: " + previousPosition + " New Position: " + newPosition);
            Vector2 relativeOffset = previousPosition - Position;
            Debug.WriteLine("Old Relative Offset: " + relativeOffset);
            Position = newPosition - relativeOffset;
            Background.Instance.AdjustPositionForTeleport(previousPosition, newPosition);
            Debug.WriteLine("New Camera Position: " + Position + " New Relative Offset: " + (newPosition - Position));
        }
    }
}
