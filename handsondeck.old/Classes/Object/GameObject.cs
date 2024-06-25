using HandsOnDeck.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck.Classes.Object
{
    public abstract class GameObject : IGameObject
    {
        public string _gameObjectTextureName;
        public WorldCoordinate spawnPosition;
        public WorldCoordinate position;
        public Vector2 size;

        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, WorldCoordinate position);
        public virtual void Reset()
        {
            position = spawnPosition;
        }
    }
}
