using HandsOnDeck2.Classes.Rendering;
using HandsOnDeck2.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace HandsOnDeck2.Classes.UI.Screens
{
    public class ScreenManager
    {
        private static ScreenManager instance;
        private Dictionary<ScreenType, Screen> screens;
        private ScreenType currentScreenType;

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

            ChangeScreen(ScreenType.MainMenu);
        }

        public void ChangeScreen(ScreenType screenType)
        {
            if (screens.ContainsKey(screenType))
            {
                screens[currentScreenType].IsActive = false;
                currentScreenType = screenType;
                screens[currentScreenType].IsActive = true;
            }
        }

        public void Update(GameTime gameTime)
        {
            screens[currentScreenType].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            screens[currentScreenType].Draw(spriteBatch);
        }
    }
}