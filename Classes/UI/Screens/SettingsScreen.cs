using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck2.Enums;
using HandsOnDeck2.Classes.CodeAccess;
using HandsOnDeck2.Classes.Rendering;

namespace HandsOnDeck2.Classes.UI.Screens
{
    public class SettingsScreen : Screen
    {
        private VolumeSlider volumeSlider;
        private Checkbox fullscreenCheckbox;
        private VideoBackground videoBackground;

        public SettingsScreen(GraphicsDevice graphicsDevice, ContentManager content) : base(graphicsDevice, content)
        {
            videoBackground = new VideoBackground(graphicsDevice, content, "background\\background_frame_", 29, 25);
        }

        public override void Initialize()
        {
            AddTextElement("Settings", new Vector2(0.5f, 0.1f), 0f, Color.Black, 2f);

            AddTextElement("Volume", new Vector2(0.47f, 0.28f), 0f, Color.Black);
            volumeSlider = new VolumeSlider(graphicsDevice, content, new Vector2(0.5f, 0.35f), new Vector2(0.6f, 0.05f));
            uiElements.Add(volumeSlider);

            AddTextElement("Fullscreen", new Vector2(0.47f, 0.48f), 0f, Color.Black);
            fullscreenCheckbox = new Checkbox(graphicsDevice, content, new Vector2(0.5f, 0.55f), new Vector2(0.05f, 0.05f), "");
            uiElements.Add(fullscreenCheckbox);

            AddButton("Apply", new Vector2(0.4f, 0.7f), new Vector2(0.2f, 0.1f), 0f, "wood_button", ApplySettings);
            AddButton("Back", new Vector2(0.6f, 0.7f), new Vector2(0.2f, 0.1f), 0f, "wood_arrow", GoBack);
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

        private void ApplySettings()
        {
            float volume = volumeSlider.GetValue();
            bool fullscreen = fullscreenCheckbox.IsChecked();

            Game1.Instance.ApplySettings(volume, fullscreen);
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