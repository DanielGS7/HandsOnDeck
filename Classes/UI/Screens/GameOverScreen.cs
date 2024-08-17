using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck2.Enums;
using HandsOnDeck2.Classes.Rendering;

namespace HandsOnDeck2.Classes.UI.Screens
{
    public class GameOverScreen : Screen
    {
        private TextElement scoreText;

        public GameOverScreen(GraphicsDevice graphicsDevice, ContentManager content) : base(graphicsDevice, content)
        {
        }

        public override void Initialize()
        {
            AddTextElement("Game Over", new Vector2(0.5f, 0.2f), 0f, Color.White, 2f);
            scoreText = new TextElement(content, "Score: 0", new Vector2(0.5f, 0.4f), 0f, Color.White, 1.5f);
            uiElements.Add(scoreText);

            //Didn't get to implemenent Game Reset() logic
            //AddButton("Play Again", new Vector2(0.5f, 0.6f), new Vector2(0.3f, 0.1f), 0f, "wood_arrow", PlayAgain);
            AddButton("Main Menu", new Vector2(0.5f, 0.75f), new Vector2(0.3f, 0.1f), 0f, "wood_button", ReturnToMainMenu);
        }

        public void SetScore(int score)
        {
            scoreText.SetText($"Score: {score}");
        }

        private void PlayAgain()
        {
            ScreenManager.Instance.ChangeScreen(ScreenType.Gameplay);
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