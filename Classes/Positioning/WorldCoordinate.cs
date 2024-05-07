using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

public class WorldCoordinate
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; private set; }
    public float Height { get; private set; }
    private const int ChunkSize = 100;

    public WorldCoordinate(float x, float y)
    {
        X = x;
        Y = y;
        Width = 20480;
        Height = 10800;
        Normalize();
    }

        public WorldCoordinate(Vector2 position)
    {
        X = position.X;
        Y = position.Y;
        Width = 20480;
        Height = 10800;
        Normalize();
    }

    public WorldCoordinate()
    {
        X = 0;
        Y = 0;
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

    public Point ToPoint()
    {
        return new Point((int)Math.Round(X), (int)Math.Round(Y));
    }


    public List<Chunk> GetRelevantChunks(Dictionary<Point, Chunk> chunkMap)
    {
        List<Chunk> relevantChunks = new List<Chunk>();
        Point currentChunkId = GetChunkID();

        relevantChunks.Add(chunkMap[currentChunkId]);

        List<Point> neighborChunkIds = new List<Point>
        {
            new Point((currentChunkId.X - 1 + (int)(Width / ChunkSize)) % (int)(Width / ChunkSize), currentChunkId.Y),
            new Point((currentChunkId.X + 1) % (int)(Width / ChunkSize), currentChunkId.Y),                          
            new Point(currentChunkId.X, (currentChunkId.Y - 1 + (int)(Height / ChunkSize)) % (int)(Height / ChunkSize)),
            new Point(currentChunkId.X, (currentChunkId.Y + 1) % (int)(Height / ChunkSize))                           
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

        public static WorldCoordinate operator +(WorldCoordinate a, WorldCoordinate b)
    {
        return new WorldCoordinate(a.X + b.X, a.Y + b.Y);
    }

    public static WorldCoordinate operator -(WorldCoordinate a, WorldCoordinate b)
    {
        return new WorldCoordinate(a.X - b.X, a.Y - b.Y);
    }

        public static WorldCoordinate operator +(WorldCoordinate wc, Vector2 v)
    {
        return new WorldCoordinate(wc.X + v.X, wc.Y + v.Y);
    }

    public static WorldCoordinate operator -(WorldCoordinate wc, Vector2 v)
    {
        return new WorldCoordinate(wc.X - v.X, wc.Y - v.Y);
    }
}
