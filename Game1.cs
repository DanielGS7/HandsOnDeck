using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HandsOnDeck2.Classes;
using HandsOnDeck2.Enums;
using HandsOnDeck2.Classes.UI.Screens;
using HandsOnDeck2.Classes.CodeAccess;
using Microsoft.Xna.Framework.Media;
using HandsOnDeck2.Classes.Sound;
using HandsOnDeck2.Classes.GameObject.Entity;
using HandsOnDeck2.Classes.Rendering;

namespace HandsOnDeck2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState _previousKeyboardState;
        private Difficulty currentDifficulty;
        public Boat PlayerBoat { get; private set; }
        private static Game1 instance;
        public static Game1 Instance => instance;

        public Game1()
        {
            instance = this;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.IsFullScreen = false;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            GraphDev.Initialize(GraphicsDevice);
            ScreenManager.Instance.Initialize(GraphicsDevice, Content);
            ScreenManager.Instance.ChangeScreen(ScreenType.MainMenu);
            PlayerBoat = Map.Instance.player;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if ((gamePadState.Buttons.Back == ButtonState.Pressed && _previousKeyboardState.IsKeyUp(Keys.Escape)) ||
                (currentKeyboardState.IsKeyDown(Keys.Escape) && _previousKeyboardState.IsKeyUp(Keys.Escape)))
            {
                HandleEscapeInput();
            }

            ScreenManager.Instance.Update(gameTime);

            _previousKeyboardState = currentKeyboardState;

            base.Update(gameTime);
        }

        private void HandleEscapeInput()
        {
            switch (ScreenManager.Instance.CurrentScreenType)
            {
                case ScreenType.Gameplay:
                    ScreenManager.Instance.ChangeScreen(ScreenType.Pause);
                    break;
                case ScreenType.Pause:
                    ScreenManager.Instance.ChangeScreen(ScreenType.Gameplay);
                    break;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            ScreenManager.Instance.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SetDifficulty(Difficulty difficulty)
        {
            currentDifficulty = difficulty;
        }

        public void ApplySettings(float volume, bool fullscreen)
        {
            GlobalInfo.MusicVolume = volume/2;
            GlobalInfo.SfxVolume = volume;
            MediaPlayer.Volume = volume;
            if (_graphics.IsFullScreen != fullscreen) _graphics.IsFullScreen = fullscreen;
            _graphics.ApplyChanges();
        }
    }
}
