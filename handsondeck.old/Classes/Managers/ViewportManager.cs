using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class ViewportManager
{
    public WorldCoordinate Position { get; private set; }
    public Vector2 Size { get; private set; }
    
    public ViewportManager(float width, float height)
    {
        Position = new WorldCoordinate();
        Size = new Vector2(width, height);
    }

    public void UpdateViewportPosition(Vector2 playerPosition, Vector2 threshold)
    {
        Vector2 relativePlayerPosition = playerPosition - Position.ToVector2();

        if (relativePlayerPosition.X < threshold.X)
            Position.X = WrapAround(playerPosition.X - threshold.X, WorldCoordinate.Width);
        else if (relativePlayerPosition.X > Size.X - threshold.X)
            Position.X = WrapAround(playerPosition.X - (Size.X - threshold.X), WorldCoordinate.Width);

        if (relativePlayerPosition.Y < threshold.Y)
            Position.Y = WrapAround(playerPosition.Y - threshold.Y, WorldCoordinate.Height);
        else if (relativePlayerPosition.Y > Size.Y - threshold.Y)
            Position.Y = WrapAround(playerPosition.Y - (Size.Y - threshold.Y), WorldCoordinate.Height);
    }

    private float WrapAround(float position, float max)
    {
        return (position + max) % max;
    }


private float AdjustViewportEdge(float targetPosition, float currentViewport, float worldSize)
{
    float distance = (targetPosition - currentViewport + worldSize / 2) % worldSize - worldSize / 2;
    return currentViewport + distance * 0.1f;   
}


    public bool IsVisible(WorldCoordinate objectPosition)
    {
        Vector2 objectPos = objectPosition.ToVector2();
        Vector2 viewportPos = Position.ToVector2();

        if (Math.Abs(objectPos.X - viewportPos.X) < Size.X && Math.Abs(objectPos.Y - viewportPos.Y) < Size.Y)
            return true;

        return false;
    }

    public WorldCoordinate GetDrawPosition(WorldCoordinate objectPosition)
    {
        WorldCoordinate position = objectPosition - Position;
        position.X = (position.X + WorldCoordinate.Width) % WorldCoordinate.Width;
        position.Y = (position.Y + WorldCoordinate.Height) % WorldCoordinate.Height;

        if (position.X > WorldCoordinate.Width - Size.X) position.X -= WorldCoordinate.Width;
        if (position.Y > WorldCoordinate.Height - Size.Y) position.Y -= WorldCoordinate.Height;

        return position;
    }

    public List<Vector2> GetWrappedPositions(Vector2 objectPosition)
    {
        List<Vector2> positions = new List<Vector2> { AdjustForWorldWrapping(objectPosition) };

        if (objectPosition.X < Size.X)
            positions.Add(new Vector2(objectPosition.X + WorldCoordinate.Width, objectPosition.Y));
        if (objectPosition.X > WorldCoordinate.Width - Size.X)
            positions.Add(new Vector2(objectPosition.X - WorldCoordinate.Width, objectPosition.Y));
        if (objectPosition.Y < Size.Y)
            positions.Add(new Vector2(objectPosition.X, objectPosition.Y + WorldCoordinate.Height));
        if (objectPosition.Y > WorldCoordinate.Height - Size.Y)
            positions.Add(new Vector2(objectPosition.X, objectPosition.Y - WorldCoordinate.Height));

        return positions;
    }

    public Vector2 AdjustForWorldWrapping(Vector2 objectPosition)
    {
        Vector2 viewportOrigin = Position.ToVector2();
        Vector2 position = objectPosition - viewportOrigin;

        position.X = WrapAround(position.X, WorldCoordinate.Width);
        position.Y = WrapAround(position.Y, WorldCoordinate.Height);

        return position;
    }
}
