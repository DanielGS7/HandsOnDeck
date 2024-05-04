using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Classes.MonogameAccessibility;
using HandsOnDeck.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck.Classes.UI.Screens
{
    public class StartScreen : UIScreen
    {
        private SpriteFont titleFont;
        private string titleText = "Hands On Deck";
        private Button startButton;
        private Button settingsButton;
        private Button exitButton;

        private static StartScreen instance;

        public static StartScreen GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StartScreen();
                    instance.Initialize();
                }
                return instance;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            startButton = new Button("Start", new Vector2(1000, 650), StartGame);
            settingsButton = new Button("Settings", new Vector2(900, 850), OpenSettings);
            exitButton = new Button("Exit", new Vector2(1200, 850), ExitGame);

            AddUIElement(startButton);
            AddUIElement(settingsButton);
            AddUIElement(exitButton);
        }

        internal override void LoadContent()
        {
            startButton.LoadContent();
            settingsButton.LoadContent();
            exitButton.LoadContent();

        }

        public override void Draw(GameTime gameTime)
        {
            titleFont = Renderer.DefaultFont;
            float scale = 5.0f;
            Vector2 textSize = titleFont.MeasureString(titleText) * scale;
            Vector2 textPosition = new Vector2(1000, 400) - textSize / 2;
            Color goldColor = new Color(255, 215, 0);

            SpriteBatchManager.Instance.DrawString(titleFont, titleText, textPosition, goldColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            base.Draw(gameTime);
        }


        private void StartGame()
        {
            GameStateManager.GetInstance.ChangeState(GameState.Game);
        }

        private void OpenSettings()
        {
            GameStateManager.GetInstance.AddScreen(GameState.Settings, new SettingsScreen());
            GameStateManager.GetInstance.ChangeState(GameState.Settings);
        }
        private void ExitGame()
        {
            System.Environment.Exit(0);
        }
    }
}
