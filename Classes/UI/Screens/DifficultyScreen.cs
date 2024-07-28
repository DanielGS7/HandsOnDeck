using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck2.Enums;
using HandsOnDeck2.Classes.CodeAccess;
using HandsOnDeck2.Classes.Rendering;

namespace HandsOnDeck2.Classes.UI.Screens
{
    public class DifficultyScreen : Screen
    {
        private VideoBackground videoBackground;
        public DifficultyScreen(GraphicsDevice graphicsDevice, ContentManager content) : base(graphicsDevice, content)
        {
            videoBackground = new VideoBackground(graphicsDevice, content, "background\\background_frame_", 29, 25);
        }

        public override void Initialize()
        {
            AddTextElement("Select Difficulty", new Vector2(0.5f, 0.2f), 0f, Color.White, 2f);
            AddButton("Easy", new Vector2(0.5f, 0.4f), new Vector2(0.3f, 0.1f), 0f, "wood_button", () => StartGame(Difficulty.Easy));
            AddButton("Normal", new Vector2(0.5f, 0.55f), new Vector2(0.3f, 0.1f), 0f, "wood_button", () => StartGame(Difficulty.Normal));
            AddButton("Hard", new Vector2(0.5f, 0.7f), new Vector2(0.3f, 0.1f), 0f, "wood_button", () => StartGame(Difficulty.Hard));
            AddButton("Back", new Vector2(0.5f, 0.85f), new Vector2(0.2f, 0.08f), 0f, "wood_button", GoBack);
        }

        public override void Update(GameTime gameTime)
        {
            videoBackground.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            videoBackground.Draw(spriteBatch, GraphDev.GetInstance.Viewport.Bounds);
            base.Draw(spriteBatch);
        }
        private void StartGame(Difficulty difficulty)
        {

            Game1.Instance.SetDifficulty(difficulty);
            ScreenManager.Instance.ChangeScreen(ScreenType.Gameplay);
        }

        private void GoBack()
        {
            ScreenManager.Instance.ChangeScreen(ScreenType.MainMenu);
        }

        public override void HandleInput()
        {
        }
    }
}