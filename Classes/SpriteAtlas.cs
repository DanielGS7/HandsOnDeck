using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace HandsOnDeck2.Classes
{
    public class SpriteAtlas
    {
        public Dictionary<string, Rectangle> Sprites { get; private set; }
        public Texture2D Texture { get; private set; }

        public SpriteAtlas(GraphicsDevice graphicsDevice, string texturePath, string dataPath)
        {
            Texture = LoadTexture(graphicsDevice, texturePath);
            Sprites = LoadData(dataPath);
        }

        private Texture2D LoadTexture(GraphicsDevice graphicsDevice, string texturePath)
        {
            using (var stream = new FileStream(texturePath, FileMode.Open))
            {
                return Texture2D.FromStream(graphicsDevice, stream);
            }
        }

        private Dictionary<string, Rectangle> LoadData(string dataPath)
        {
            var sprites = new Dictionary<string, Rectangle>();
            var lines = File.ReadAllLines(dataPath);

            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                var name = parts[0];
                var x = int.Parse(parts[1]);
                var y = int.Parse(parts[2]);
                var width = int.Parse(parts[3]);
                var height = int.Parse(parts[4]);

                sprites[name] = new Rectangle(x, y, width, height);
            }

            return sprites;
        }

        public Rectangle GetSprite(string name)
        {
            if (Sprites.ContainsKey(name))
            {
                return Sprites[name];
            }
            return Rectangle.Empty;
        }
    }
}
