using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using HandsOnDeck2.Enums;

namespace HandsOnDeck2.Classes
{
    public class GameStateManager
    {
        private Dictionary<GameState, List<IUIElement>> uiElements;
        private GameState currentState;

        public GameStateManager()
        {
            uiElements = new Dictionary<GameState, List<IUIElement>>();
            currentState = GameState.MainMenu; // Initial state
        }

        public void Initialize(GameState initialState)
        {
            currentState = initialState;
        }

        public void AddUIElement(GameState state, IUIElement element)
        {
            if (!uiElements.ContainsKey(state))
            {
                uiElements[state] = new List<IUIElement>();
            }
            uiElements[state].Add(element);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var element in uiElements[currentState])
            {
                element.Update(gameTime);
                if (element is IUIInteractable interactableElement)
                {
                    // Handle mouse input for interactable UI elements
                    MouseState mouseState = Mouse.GetState();
                    interactableElement.HandleInput(mouseState);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var element in uiElements[currentState])
            {
                element.Draw(spriteBatch);
            }
        }

        public void ChangeState(GameState newState)
        {
            currentState = newState;
        }
    }
}
