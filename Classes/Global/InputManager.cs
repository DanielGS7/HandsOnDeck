using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using HandsOnDeck2.Interfaces;
using HandsOnDeck2.Enums;

namespace HandsOnDeck2.Classes.Global
{
    public class InputManager
    {
        private static InputManager instance;
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
        private Dictionary<Keys, GameAction> keyMappings;

        private InputManager()
        {
            InitializeKeyMappings();
        }

        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputManager();
                }
                return instance;
            }
        }

        private void InitializeKeyMappings()
        {
            keyMappings = new Dictionary<Keys, GameAction>
            {
                { Keys.Z, GameAction.SailsOpen },
                { Keys.S, GameAction.SailsClosed },
                { Keys.Q, GameAction.SteerLeft },
                { Keys.D, GameAction.SteerRight },
                { Keys.A, GameAction.ShootLeft },
                { Keys.E, GameAction.ShootRight },
                { Keys.Space, GameAction.ToggleAnchor },
                { Keys.R, GameAction.Reload }
            };
        }

        public void Update()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
        }

        public void HandleInput(IControllable controllable)
        {
            foreach (var keyMapping in keyMappings)
            {
                if (currentKeyboardState.IsKeyDown(keyMapping.Key))
                {
                    controllable.HandleInput(keyMapping.Value);
                }
            }
        }

        public bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyHeld(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }
    }
}