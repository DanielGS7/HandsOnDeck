using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck2.Classes
{
    public class MainMenuState : IGameState
    {
        public void Enter() { }

        public void Update(GameTime gameTime)
        {
            // Handle input and update menu items
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            // Draw menu items
            spriteBatch.End();
        }

        public void Exit() { }
    }
}
