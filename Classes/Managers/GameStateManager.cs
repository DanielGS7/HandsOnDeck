﻿using HandsOnDeck.Classes.UI;
using HandsOnDeck.Classes.UI.Screens;
using HandsOnDeck.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HandsOnDeck.Classes.Managers
{
    public class GameStateManager
    {
        private static GameStateManager instance;
        private GameState currentState;
        private Dictionary<GameState, UIScreen> screens;

        private GameStateManager()
        {
            screens = new Dictionary<GameState, UIScreen>();
            AddScreen(GameState.Game, new GameScreen());
            AddScreen(GameState.Start, StartScreen.Instance);
            ChangeState(GameState.Start);
        }

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

        public void AddScreen(GameState state, UIScreen screen)
        {
            screens[state] = screen;
        }

        public void ChangeState(GameState newState)
        {
            if (screens.ContainsKey(newState))
            {
                currentState = newState;
            }
        }

        public void LoadContent()
        {
            foreach (var screen in screens.Values)
            {
                screen.LoadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (screens.ContainsKey(currentState) && screens[currentState] != null)
            {
                screens[currentState].Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (screens.ContainsKey(currentState) && screens[currentState] != null)
            {
                screens[currentState].Draw(gameTime);
            }
        }
    }

}