using HandsOnDeck.Classes.UI.Screens;

using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Enums;
using Microsoft.Xna.Framework;

namespace HandsOnDeck.Classes.UI.Screens
{
    public class SettingsScreen : UIScreen
    {
        private Toggle soundToggle;
        private Toggle musicToggle;
        private Button backButton;

        public SettingsScreen()
        {
            soundToggle = new Toggle("Sound", new Vector2(Game1.ProgramWidth / 2 - 350, Game1.ProgramHeight / 2), true, ToggleSound);
            musicToggle = new Toggle("Music", new Vector2(Game1.ProgramWidth / 2 + 150, Game1.ProgramHeight / 2), true, ToggleMusic);
            backButton = new Button("Back", new Vector2(Game1.ProgramWidth / 2, Game1.ProgramHeight / 2 + 200), GoBack);

            AddUIElement(soundToggle);
            AddUIElement(musicToggle);
            AddUIElement(backButton);
        }

        private void ToggleSound()
        {
            
        }

        private void ToggleMusic()
        {
            
        }

        private void GoBack()
        {
            GameStateManager.GetInstance.ChangeState(GameState.Pause);
        }

        internal override void LoadContent()
        {
            throw new System.NotImplementedException();
        }
    }
}