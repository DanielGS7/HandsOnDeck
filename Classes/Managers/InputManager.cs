﻿using HandsOnDeck.Enums;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Classes.Managers
{
    public class InputManager
    {
        private static InputManager instance;
        private static readonly object lockObject = new object();

        private Dictionary<GameAction, Keys> keyMappings;
        private KeyboardState currentKeyboardState;

        private InputManager()
        {
            keyMappings = new Dictionary<GameAction, Keys>();
        }

        public static InputManager GetInstance()
        {
            lock (lockObject)
            {
                if (instance == null)
                {
                    instance = new InputManager();
                }
            }

            return instance;
        }

        public void Initialize()
        {
            AddMapping(GameAction.SAILSUP, Keys.Z);
            AddMapping(GameAction.SAILSDOWN, Keys.S);
            AddMapping(GameAction.TURNLEFT, Keys.Q);
            AddMapping(GameAction.TURNRIGHT, Keys.D);
            AddMapping(GameAction.TOGGLEANCHOR, Keys.LeftShift);
            AddMapping(GameAction.SELECT, Keys.Space);
            AddMapping(GameAction.PAUSE, Keys.Escape);
            AddMapping(GameAction.SHOOT, Keys.LeftControl);
            AddMapping(GameAction.RELOAD, Keys.A);
            AddMapping(GameAction.REPAIR, Keys.H);
        }

        public void AddMapping(GameAction action, Keys key)
        {
            if (keyMappings.ContainsKey(action))
            {
                keyMappings[action] = key;
            }
            else
            {
                keyMappings.Add(action, key);
            }
        }


        public void RemoveMapping(GameAction action)
        {
            if (keyMappings.ContainsKey(action))
            {
                keyMappings.Remove(action);
            }
            else
            {
                throw new ArgumentException($"Action '{action}' not found.");
            }
        }

        public void SetCurrentKeyboardState(KeyboardState state)
        {
            currentKeyboardState = state;
        }

        public List<GameAction> GetPressedActions()
        {
            List<GameAction> pressedActions = new List<GameAction>();

            foreach (var mapping in keyMappings)
            {
                if (currentKeyboardState.IsKeyDown(mapping.Value))
                {
                    pressedActions.Add(mapping.Key);
                }
            }

            return pressedActions;
        }
    }

}