using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck2.Enums;
using HandsOnDeck2.Classes.CodeAccess;
using HandsOnDeck2.Classes.Rendering;
using System.Collections.Generic;
using System;

namespace HandsOnDeck2.Classes.UI.Screens
{
    public class LoadGameScreen : Screen
    {
        private VideoBackground videoBackground;
        private List<SaveFileInfo> saveFiles;
        private List<UIElement> saveFileButtons;

        public LoadGameScreen(GraphicsDevice graphicsDevice, ContentManager content) : base(graphicsDevice, content)
        {
            videoBackground = new VideoBackground(graphicsDevice, content, "background\\background_frame_", 29, 25);
            saveFileButtons = new List<UIElement>();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void OnActivate()
        {
            RefreshSaveFiles();
        }

        private void RefreshSaveFiles()
        {
            // Clear existing save file buttons
            foreach (var button in saveFileButtons)
            {
                uiElements.Remove(button);
            }
            saveFileButtons.Clear();

            // Add title
            if (!uiElements.Exists(e => e is TextElement && ((TextElement)e).Text == "Load Game"))
            {
                AddTextElement("Load Game", new Vector2(0.5f, 0.1f), 0f, Color.White, 2f);
            }

            // Get save files
            saveFiles = SaveManager.Instance.GetSaveFiles();

            // Add buttons for each save file
            for (int i = 0; i < saveFiles.Count; i++)
            {
                var saveFile = saveFiles[i];
                var button = new Button(graphicsDevice, content, 
                    $"{saveFile.Name} - {saveFile.Date:d}", 
                    new Vector2(0.5f, 0.3f + i * 0.12f), 
                    new Vector2(0.6f, 0.1f), 
                    0f, 
                    "wood_button", 
                    () => LoadSaveFile(saveFile.Name));
                uiElements.Add(button);
                saveFileButtons.Add(button);
            }

            // Add back button if it doesn't exist
            if (!uiElements.Exists(e => e is Button && ((Button)e).Text == "Back"))
            {
                AddButton("Back", new Vector2(0.5f, 0.9f), new Vector2(0.2f, 0.08f), 0f, "wood_arrow", GoBack);
            }

            Console.WriteLine($"Refreshed save files. Found {saveFiles.Count} saves."); // Debug output
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

        private void LoadSaveFile(string saveName)
        {
            Console.WriteLine($"Attempting to load save file: {saveName}"); // Debug output
            GameSaveData loadedState = SaveManager.Instance.LoadData(saveName);
            if (loadedState != null)
            {
                Game1.Instance.LoadGameSaveData(loadedState);
                ScreenManager.Instance.ChangeScreen(ScreenType.Gameplay);
            }
            else
            {
                Console.WriteLine($"Failed to load save file: {saveName}"); // Debug output
            }
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