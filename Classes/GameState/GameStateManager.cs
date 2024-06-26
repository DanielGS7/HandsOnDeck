using HandsOnDeck2.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using HandsOnDeck2.Interfaces;

namespace HandsOnDeck2.Classes
{
    public class GameStateManager
    {
        private Dictionary<GameStates, IGameState> gameStates;
        private GameStates currentState;

        public GameStateManager()
        {
            gameStates = new Dictionary<GameStates, IGameState>();
        }

        public void AddState(GameStates state, IGameState gameState)
        {
            gameStates[state] = gameState;
        }

        public void ChangeState(GameStates newState)
        {
            currentState = newState;
            gameStates[currentState].Enter();
        }

        public void Update(GameTime gameTime)
        {
            if (gameStates.ContainsKey(currentState))
            {
                gameStates[currentState].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (gameStates.ContainsKey(currentState))
            {
                gameStates[currentState].Draw(spriteBatch);
            }
        }
    }
}
