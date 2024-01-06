using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Classes.Managers.HandsOnDeck.Classes.Managers;
using HandsOnDeck.Classes.UI.UIElements;
using HandsOnDeck.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.Xml.Linq;

namespace HandsOnDeck.Classes.UI.Screens
{
    public class StartScreen : UIScreen
    {
        private SpriteFont titleFont;
        private string titleText = "Hands On Deck";
        private Vector2 titlePosition;
        private Button startButton;
        private Button settingsButton;
        private Button exitButton;

        private static StartScreen instance;

        public static StartScreen Instance
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
            startButton = new Button("Start", new Vector2(1000, 650), StartGame);
            settingsButton = new Button("Settings", new Vector2(900, 850), OpenSettings);
            exitButton = new Button("Exit", new Vector2(1200, 850), ExitGame);

            AddUIElement(startButton);
            AddUIElement(settingsButton);
            AddUIElement(exitButton);
        }

        internal void LoadContent()
        {
            titlePosition = new Vector2(400, 100);
            startButton.LoadContent();
            settingsButton.LoadContent();
            exitButton.LoadContent();

        }

        public override void Draw(GameTime gameTime)
        {
            titleFont = Game1.DefaultFont;
            base.Draw(gameTime);
            SpriteBatchManager.Instance.DrawString(titleFont, titleText, titlePosition, Color.White);
        }

        private void StartGame()
        {
            GameStateManager.Instance.ChangeState(GameState.Game);
        }

        private void OpenSettings()
        {
            GameStateManager.Instance.AddScreen(GameState.Settings, new SettingsScreen());
            GameStateManager.Instance.ChangeState(GameState.Settings);
        }
        private void ExitGame()
        {
            System.Environment.Exit(0);
        }
    }
}
