using HandsOnDeck.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck.Classes.Object
{
    public abstract class GameObject : IGameObject
    {
        public string _gameObjectTextureName;
        public Texture2D _gameObjectTexture;
        public Vector2 position;

        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, Vector2 position);
    }
}
