using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck2.Enums;
using HandsOnDeck2.Classes.CodeAccess;
using HandsOnDeck2.Classes.Rendering;

namespace HandsOnDeck2.Classes.UI.Screens
{
    public class MainMenuScreen : Screen
    {
        private VideoBackground videoBackground;
        public MainMenuScreen(GraphicsDevice graphicsDevice, ContentManager content) : base(graphicsDevice, content)
        {
            videoBackground = new VideoBackground(content, "background\\background_frame_", 29, 25);
        }

        public override void Initialize()
        {
            AddTextElement("Hands on Deck", new Vector2(0.5f, 0.2f), 0f, Color.White, 2f);
            AddButton("Start Game", new Vector2(0.5f, 0.4f), new Vector2(0.3f, 0.1f), -0.2f, "wood_arrow", StartGame);
            AddButton("Settings", new Vector2(0.5f, 0.55f), new Vector2(0.3f, 0.1f), 0.1f, "wood_button", OpenSettings);
            AddButton("Quit", new Vector2(0.5f, 0.7f), new Vector2(0.3f, 0.1f), -0.14f, "wood_button", QuitGame);
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

        private void StartGame()
        {
            ScreenManager.Instance.ChangeScreen(ScreenType.Difficulty);
        }

        private void OpenSettings()
        {
            ScreenManager.Instance.ChangeScreen(ScreenType.Settings);
        }

        private void QuitGame()
        {
            Game1.Instance.Exit();
        }

        public override void HandleInput()
        {
        }
    }
}