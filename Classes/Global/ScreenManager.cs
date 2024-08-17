using HandsOnDeck2.Classes.Rendering;
using HandsOnDeck2.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using HandsOnDeck2.Classes.Sound;
using HandsOnDeck2.Interfaces;

namespace HandsOnDeck2.Classes.UI.Screens
{
    public class ScreenManager
    {
        private static ScreenManager instance;
        internal Dictionary<ScreenType, Screen> screens;
        internal ScreenType currentScreenType;

        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScreenManager();
                }
                return instance;
            }
        }

        public ScreenType CurrentScreenType => currentScreenType;

        private ScreenManager()
        {
            screens = new Dictionary<ScreenType, Screen>();
        }

        public void Initialize(GraphicsDevice graphicsDevice, ContentManager content)
        {
            screens[ScreenType.MainMenu] = new MainMenuScreen(graphicsDevice, content);
            screens[ScreenType.Gameplay] = new GameplayScreen(graphicsDevice, content);
            screens[ScreenType.Pause] = new PauseScreen(graphicsDevice, content);
            screens[ScreenType.Settings] = new SettingsScreen(graphicsDevice, content);
            screens[ScreenType.GameOver] = new GameOverScreen(graphicsDevice, content);
            screens[ScreenType.Difficulty] = new DifficultyScreen(graphicsDevice, content);

            foreach (var screen in screens.Values)
            {
                screen.Initialize();
                screen.LoadContent();
            }
            AudioManager.Instance.LoadContent(content);
            ChangeScreen(ScreenType.MainMenu);
        }

        public void ChangeScreen(ScreenType screenType)
        {
            if (screens.ContainsKey(screenType))
            {
                screens[currentScreenType].IsActive = false;
                currentScreenType = screenType;

                GameState newGameState = DetermineGameState(screenType);
                AudioManager.Instance.PlayMusicForState(newGameState, 3f);

                screens[currentScreenType].IsActive = true;
            }
        }

        private GameState DetermineGameState(ScreenType screenType)
        {
            switch (screenType)
            {
                case ScreenType.MainMenu:
                case ScreenType.Settings:
                case ScreenType.Difficulty:
                    return GameState.DefaultMenu;
                case ScreenType.Gameplay:
                    return GameState.DefaultPlay;
                case ScreenType.Pause:
                    return GameState.PausedMenu;
                case ScreenType.GameOver:
                    return GameState.GameOverMenu;
                default:
                    return GameState.DefaultMenu;
            }
        }

        public void Update(GameTime gameTime)
        {
            AudioManager.Instance.Update(gameTime);
            screens[currentScreenType].Update(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            screens[currentScreenType].Draw(spriteBatch);
        }
    }
}