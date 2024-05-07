using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class WorldCoordinate
{
    public float X { get; private set; }
    public float Y { get; private set; }
    public float Width { get; private set; }
    public float Height { get; private set; }
    private const int ChunkSize = 100;  // Define chunk size, adjust based on your game's scale

    public WorldCoordinate(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Normalize();
    }

    private void Normalize()
    {
        X = (X + Width) % Width;
        Y = (Y + Height) % Height;
    }

    public Vector2 ToVector2()
    {
        return new Vector2(X, Y);
    }

    public List<Chunk> GetRelevantChunks(Dictionary<Point, Chunk> chunkMap)
    {
        List<Chunk> relevantChunks = new List<Chunk>();
        Point currentChunkId = GetChunkID();

        // Include current chunk
        relevantChunks.Add(chunkMap[currentChunkId]);

        // Determine neighboring chunks, considering wrapping
        List<Point> neighborChunkIds = new List<Point>
        {
            new Point((currentChunkId.X - 1 + (int)(Width / ChunkSize)) % (int)(Width / ChunkSize), currentChunkId.Y), // Left
            new Point((currentChunkId.X + 1) % (int)(Width / ChunkSize), currentChunkId.Y),                          // Right
            new Point(currentChunkId.X, (currentChunkId.Y - 1 + (int)(Height / ChunkSize)) % (int)(Height / ChunkSize)), // Up
            new Point(currentChunkId.X, (currentChunkId.Y + 1) % (int)(Height / ChunkSize))                           // Down
        };

        foreach (var id in neighborChunkIds)
        {
            if (chunkMap.ContainsKey(id))
            {
                relevantChunks.Add(chunkMap[id]);
            }
        }

        return relevantChunks;
    }

    private Point GetChunkID()
    {
        int chunkX = (int)(X / ChunkSize);
        int chunkY = (int)(Y / ChunkSize);
        return new Point(chunkX, chunkY);
    }
}
