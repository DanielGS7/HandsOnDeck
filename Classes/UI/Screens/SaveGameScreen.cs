using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck2.Enums;
using HandsOnDeck2.Classes.CodeAccess;
using HandsOnDeck2.Classes.Rendering;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace HandsOnDeck2.Classes.UI.Screens
{
    public class SaveGameScreen : Screen
    {
        private VideoBackground videoBackground;
        private TextElement saveNameText;
        private string saveName = "";

        public SaveGameScreen(GraphicsDevice graphicsDevice, ContentManager content) : base(graphicsDevice, content)
        {
            videoBackground = new VideoBackground(graphicsDevice, content, "background\\background_frame_", 29, 25);
        }

        public override void Initialize()
        {
            AddTextElement("Save Game", new Vector2(0.5f, 0.1f), 0f, Color.White, 2f);
            saveNameText = new TextElement(content, "Save Name: ", new Vector2(0.5f, 0.4f), 0f, Color.White, 1.5f);
            uiElements.Add(saveNameText);
            AddButton("Save", new Vector2(0.5f, 0.6f), new Vector2(0.3f, 0.1f), 0f, "wood_button", SaveGame);
            AddButton("Cancel", new Vector2(0.5f, 0.75f), new Vector2(0.3f, 0.1f), 0f, "wood_arrow", Cancel);
        }

        public override void Update(GameTime gameTime)
        {
            videoBackground.Update(gameTime);
            base.Update(gameTime);

            KeyboardState keyboardState = Keyboard.GetState();
            Keys[] pressedKeys = keyboardState.GetPressedKeys();

            foreach (Keys key in pressedKeys)
            {
                if (key >= Keys.A && key <= Keys.Z && saveName.Length < 20)
                {
                    saveName += key.ToString();
                }
                else if (key == Keys.Back && saveName.Length > 0)
                {
                    saveName = saveName.Substring(0, saveName.Length - 1);
                }
            }

            saveNameText.SetText($"Save Name: {saveName}");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            videoBackground.Draw(spriteBatch, GraphDev.GetInstance.Viewport.Bounds);
            base.Draw(spriteBatch);
        }

        private void SaveGame()
        {
            if (!string.IsNullOrWhiteSpace(saveName))
            {
                Game1.Instance.SaveGame(saveName);
                ScreenManager.Instance.ChangeScreen(ScreenType.MainMenu);
            }
        }

        private void Cancel()
        {
            ScreenManager.Instance.ChangeScreen(ScreenType.MainMenu);
        }

        public override void HandleInput()
        {
        }
    }
}