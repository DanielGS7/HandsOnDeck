using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace HandsOnDeck.Classes.UI.Screens
{
    using Microsoft.Xna.Framework;
    using System;
    using System.Configuration;

    public class PauseScreen : UIScreen
    {
        private Button resumeButton;
        private Toggle musicToggle;
        private Toggle soundToggle;
        private Button exitButton;

        public PauseScreen()
        {
            resumeButton = new Button("Resume", new Vector2(Game1.ProgramWidth / 2, Game1.ProgramHeight / 2 - 200), ResumeGame);
            musicToggle = new Toggle("Music", new Vector2(Game1.ProgramWidth / 2 - 350, Game1.ProgramHeight / 2),true, ToggleMusic);
            soundToggle = new Toggle("Sound", new Vector2(Game1.ProgramWidth / 2 + 150, Game1.ProgramHeight / 2),true, ToggleSound);
            exitButton = new Button("Exit to Main Menu", new Vector2(Game1.ProgramWidth / 2, Game1.ProgramHeight / 2 + 200), ExitToMainMenu);

            AddUIElement(resumeButton);
            AddUIElement(musicToggle);
            AddUIElement(soundToggle);
            AddUIElement(exitButton);
        }

        internal override void LoadContent() { }
        private void ResumeGame()
        {
            GameScreen.Instance.isPaused = false;
            GameStateManager.Instance.ChangeState(GameState.Game);
        }

        private void ToggleMusic()
        {
        }

        private void ToggleSound()
        {
        }

        private void ExitToMainMenu()
        {
            GameStateManager.Instance.ChangeState(GameState.Start);
        }
    }

}
