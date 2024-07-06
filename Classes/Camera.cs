using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck2.Classes;
using System;

namespace HandsOnDeck2.Classes
{
    public class Camera
    {
        public Matrix Transform { get; private set; }
        public Vector2 Position { get; private set; }
        private Vector2 offset;

        public Camera()
        {
            Position = Vector2.Zero;
            offset = Vector2.Zero;
        }

        public void Update(SeaCoordinate targetPosition, Viewport viewport, int mapWidth, int mapHeight)
        {
            float bufferX = viewport.Width * 0.28f;
            float bufferY = viewport.Height * 0.28f;

            Vector2 targetVector = targetPosition.ToVector2();
            Vector2 newPosition = Position;

            if (targetVector.X - Position.X < bufferX)
                newPosition.X = targetVector.X - bufferX;
            else if (targetVector.X - Position.X > viewport.Width - bufferX)
                newPosition.X = targetVector.X - viewport.Width + bufferX;

            if (targetVector.Y - Position.Y < bufferY)
                newPosition.Y = targetVector.Y - bufferY;
            else if (targetVector.Y - Position.Y > viewport.Height - bufferY)
                newPosition.Y = targetVector.Y - viewport.Height + bufferY;

            offset = newPosition - Position;
            Position = newPosition;

            Transform = Matrix.CreateTranslation(new Vector3(-Position, 0));
        }

        public void AdjustPositionForTeleport(SeaCoordinate previousBoatPosition, SeaCoordinate newBoatPosition)
        {
            Vector2 positionDifference = newBoatPosition.ToVector2() - previousBoatPosition.ToVector2();
            Position += positionDifference;
        }

        public Vector2 GetOffset()
        {
            return offset;
        }
    }
}