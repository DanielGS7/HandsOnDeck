using System.Collections.Generic;
using HandsOnDeck.Classes.Object;
using Microsoft.Xna.Framework;

public class Chunk
{
    public List<GameObject> GameObjects { get; private set; }
    public Point ChunkID { get; private set; }

    public Chunk(Point chunkID)
    {
        ChunkID = chunkID;
        GameObjects = new List<GameObject>();
    }

    // Additional methods to manage game objects can be added here.
}
