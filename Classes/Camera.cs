using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck2.Classes
{
    public class Camera
    {
        public Matrix Transform { get; private set; }
        public Vector2 Position { get => position; set => position = value; }

        private Vector2 position;
        private float lerpFactor;

        public Camera()
        {
            Position = Vector2.Zero;
            lerpFactor = 0.06f;
        }

        public void Update(Vector2 targetPosition, Viewport viewport)
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

            Position = Vector2.Lerp(Position, targetCameraPosition, lerpFactor);

            Transform = Matrix.CreateTranslation(new Vector3(-Position, 0));
        }
    }
}
