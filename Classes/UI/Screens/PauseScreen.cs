using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck2.Enums;
using HandsOnDeck2.Classes.Rendering;

namespace HandsOnDeck2.Classes.UI.Screens
{
    public class PauseScreen : Screen
    {
        public PauseScreen(GraphicsDevice graphicsDevice, ContentManager content) : base(graphicsDevice, content)
        {
        }

        public override void Initialize()
        {
            AddTextElement("Paused", new Vector2(0.5f, 0.2f), 0f, Color.White, 2f);
            AddButton("Resume", new Vector2(0.5f, 0.4f), new Vector2(0.3f, 0.1f), 0f, "wood_arrow", ResumeGame);
            AddButton("Options", new Vector2(0.5f, 0.55f), new Vector2(0.3f, 0.1f), 0f, "wood_button", OpenOptions);
            AddButton("Main Menu", new Vector2(0.5f, 0.7f), new Vector2(0.3f, 0.1f), 0f, "wood_button", ReturnToMainMenu);
        }

        private void ResumeGame()
        {
            ScreenManager.Instance.ChangeScreen(ScreenType.Gameplay);
        }

        private void OpenOptions()
        {
            ScreenManager.Instance.ChangeScreen(ScreenType.Settings);
        }

        private void ReturnToMainMenu()
        {
            ScreenManager.Instance.ChangeScreen(ScreenType.MainMenu);
        }

        public override void HandleInput()
        {
        }
    }
}