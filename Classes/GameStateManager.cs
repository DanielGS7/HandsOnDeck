using HandsOnDeck2.Enums;
using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace HandsOnDeck2.Classes
{
    public class GameStateManager
    {
        private static GameStateManager instance;
        public static GameStateManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameStateManager();
                }
                return instance;
            }
        }

        private Dictionary<GameStates, IGameState> gameStates;
        private IGameState currentState;

        private GameStateManager()
        {
            gameStates = new Dictionary<GameStates, IGameState>();
        }

        public void AddState(GameStates state, IGameState gameState)
        {
            if (!gameStates.ContainsKey(state))
            {
                gameStates[state] = gameState;
            }
        }

        public void ChangeState(GameStates newState)
        {
            currentState?.Exit();
            currentState = gameStates[newState];
            currentState.Enter();
        }

        public void Update(GameTime gameTime)
        {
            currentState?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentState?.Draw(spriteBatch);
        }
    }
}
