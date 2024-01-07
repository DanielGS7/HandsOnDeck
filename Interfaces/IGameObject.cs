using Microsoft.Xna.Framework;

namespace HandsOnDeck.Interfaces
{
    internal interface IGameObject
    {
        void LoadContent();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}