using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck2.Interfaces
{
    public interface IGameState
    {
        void Enter();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        void Exit();
    }
}
