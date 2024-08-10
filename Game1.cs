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
using HandsOnDeck2.Classes.GameObject;
using HandsOnDeck2.Classes.Rendering;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace HandsOnDeck2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState _previousKeyboardState;
        private Difficulty currentDifficulty;
        private string currentSaveFile;
        private GameSaveData currentGameState;

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

    public void SaveCurrentGame()
    {
        currentGameState = new GameSaveData
        {
            PlayerBoat = Map.Instance.GetPlayerBoat(),
            Islands = Map.Instance.GetIslands(),
            Score = GlobalInfo.Score
        };

        if (string.IsNullOrEmpty(currentSaveFile))
        {
            currentSaveFile = $"AutoSave_{DateTime.Now:yyyyMMdd_HHmmss}";
        }

        SaveManager.Instance.SaveGame(currentSaveFile, currentGameState);
        Debug.WriteLine($"Game saved with name: {currentSaveFile}"); // Debug output
    }

        public void SaveGame(string saveName)
        {
            SaveManager.Instance.SaveGame(saveName, currentGameState);
            currentSaveFile = saveName;
        }

        public void LoadGameSaveData(GameSaveData gameState)
        {
            if (gameState == null)
            {
                throw new ArgumentNullException(nameof(gameState), "Loaded game state is null.");
            }

            currentGameState = gameState;
            Map.Instance.LoadGameSaveData(gameState);
            ScreenManager.Instance.ChangeScreen(ScreenType.Gameplay);
        }

        public void ResumeGame()
        {
            if (currentGameState != null)
            {
                LoadGameSaveData(currentGameState);
            }
            else if (!string.IsNullOrEmpty(currentSaveFile))
            {
                GameSaveData loadedState = SaveManager.Instance.LoadData(currentSaveFile);
                if (loadedState != null)
                {
                    LoadGameSaveData(loadedState);
                }
            }
        }
        
    public void ResetGameState()
    {
        currentGameState = null;
        currentSaveFile = null;
        Map.Instance.ResetGame();
        GlobalInfo.Score = 0;
    }
    }
}