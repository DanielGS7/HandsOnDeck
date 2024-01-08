using HandsOnDeck.Classes.UI;
using HandsOnDeck.Classes.UI.Screens;
using HandsOnDeck.Enums;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace HandsOnDeck.Classes.Managers
{
    public class GameStateManager
    {
        private static GameStateManager instance;

        private GameState currentState;
        private Dictionary<GameState, UIScreen> screens;
        private bool pausePreviouslyPressed = false;

        public static GameStateManager GetInstance
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

        private GameStateManager()
        {
            screens = new Dictionary<GameState, UIScreen>();
            AddScreen(GameState.Start, StartScreen.GetInstance);
            AddScreen(GameState.Game, GameScreen.GetInstance);
            AddScreen(GameState.Pause, new PauseScreen());
            AddScreen(GameState.GameOver,GameOverScreen.GetInstance);
            ChangeState(GameState.Start);
        }

        public void LoadContent()
        {
            foreach (var screen in screens.Values)
            {
                Debug.WriteLine("meow");
                screen.LoadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            bool pausePressed = InputManager.GetInstance.GetPressedActions().Contains(GameAction.PAUSE);
            pausePreviouslyPressed = pausePressed;

            if (pausePressed && !pausePreviouslyPressed)
            {
                TogglePause();
            }

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

        public void TogglePause()
        {
            if (currentState == GameState.Game)
            {
                ChangeState(GameState.Pause);
                GameScreen.GetInstance.isPaused = true;
            }
            else if (currentState == GameState.Pause)
            {
                ChangeState(GameState.Game);
                GameScreen.GetInstance.isPaused = false;
            }
        }
    }

}
