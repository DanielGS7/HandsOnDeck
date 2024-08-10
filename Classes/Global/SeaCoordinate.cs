using Microsoft.Xna.Framework;
using System;

namespace HandsOnDeck2.Classes.Global
{
    public class SeaCoordinate
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        private static int mapWidth;
        private static int mapHeight;

        public static void SetMapDimensions(int width, int height)
        {
            mapWidth = width;
            mapHeight = height;
        }

        public SeaCoordinate(float x, float y)
        {
            X = x;
            Y = y;
            WrapCoordinates();
        }

        public static SeaCoordinate FromVector2(Vector2 vector)
        {
            return new SeaCoordinate(vector.X, vector.Y);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }

        private void WrapCoordinates()
        {
            X = (X + mapWidth) % mapWidth;
            Y = (Y + mapHeight) % mapHeight;
        }

        public void Move(float dx, float dy)
        {
            X += dx;
            Y += dy;
            WrapCoordinates();
        }

        public float DistanceTo(SeaCoordinate other)
        {
            float dx = Math.Min(Math.Abs(X - other.X), mapWidth - Math.Abs(X - other.X));
            float dy = Math.Min(Math.Abs(Y - other.Y), mapHeight - Math.Abs(Y - other.Y));
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public static SeaCoordinate operator +(SeaCoordinate a, SeaCoordinate b)
        {
            return new SeaCoordinate(a.X + b.X, a.Y + b.Y);
        }

        public static SeaCoordinate operator -(SeaCoordinate a, SeaCoordinate b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;

            if (Math.Abs(dx) > mapWidth / 2)
                dx = dx > 0 ? dx - mapWidth : dx + mapWidth;
            if (Math.Abs(dy) > mapHeight / 2)
                dy = dy > 0 ? dy - mapHeight : dy + mapHeight;

            return new SeaCoordinate(dx, dy);
        }

        public static implicit operator Vector2(SeaCoordinate coord)
        {
            return new Vector2(coord.X, coord.Y);
        }

        public static implicit operator SeaCoordinate(Vector2 vector)
        {
            return new SeaCoordinate(vector.X, vector.Y);
        }
    }
}