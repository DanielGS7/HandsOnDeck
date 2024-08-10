using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck2.Enums;
using HandsOnDeck2.Classes.CodeAccess;
using HandsOnDeck2.Classes.Rendering;
using System.Collections.Generic;
using System.Diagnostics;

namespace HandsOnDeck2.Classes.UI.Screens
{
    public class MainMenuScreen : Screen
    {
        private VideoBackground videoBackground;
        private List<UIElement> dynamicUIElements;

        public MainMenuScreen(GraphicsDevice graphicsDevice, ContentManager content) : base(graphicsDevice, content)
        {
            videoBackground = new VideoBackground(graphicsDevice, content, "background\\background_frame_", 29, 25);
            dynamicUIElements = new List<UIElement>();
        }

        public override void Initialize()
        {
            base.Initialize();
            RefreshButtons();
        }

    private void RefreshButtons()
    {
        // Clear existing dynamic UI elements
        foreach (var element in dynamicUIElements)
        {
            uiElements.Remove(element);
        }
        dynamicUIElements.Clear();

        // Add static elements
        if (uiElements.Count == 0)
        {
            AddTextElement("Hands on Deck", new Vector2(0.5f, 0.2f), 0f, Color.White, 2f);
        }

        // Add dynamic buttons
        AddDynamicButton("Start New Game", new Vector2(0.5f, 0.4f), new Vector2(0.3f, 0.1f), -0.2f, "wood_arrow", StartNewGame);
        
        bool hasSaveFiles = SaveManager.Instance.HasSaveFiles();
        Debug.WriteLine($"Has save files: {hasSaveFiles}"); // Debug output

        if (hasSaveFiles)
        {
            AddDynamicButton("Load Game", new Vector2(0.5f, 0.55f), new Vector2(0.3f, 0.1f), 0.1f, "wood_button", LoadGame);
        }

        AddDynamicButton("Settings", new Vector2(0.5f, 0.7f), new Vector2(0.3f, 0.1f), 0.1f, "wood_button", OpenSettings);
        AddDynamicButton("Quit", new Vector2(0.5f, 0.85f), new Vector2(0.3f, 0.1f), -0.14f, "wood_button", QuitGame);
    }

        private void AddDynamicButton(string text, Vector2 position, Vector2 size, float rotation, string textureName, System.Action onClick)
        {
            var button = new Button(graphicsDevice, content, text, position, size, rotation, textureName, onClick);
            uiElements.Add(button);
            dynamicUIElements.Add(button);
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

        private void StartNewGame()
        {
            Game1.Instance.ResetGameState();
            ScreenManager.Instance.ChangeScreen(ScreenType.Difficulty);
        }

        private void LoadGame()
        {
            ScreenManager.Instance.ChangeScreen(ScreenType.LoadGame);
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

        public override void OnActivate()
        {
            RefreshButtons();
        }
    }
}