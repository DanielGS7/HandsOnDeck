using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck2.Enums;
using HandsOnDeck2.Classes.Rendering;
using HandsOnDeck2.Classes.CodeAccess;
using System;

namespace HandsOnDeck2.Classes.UI.Screens
{
    public class PauseScreen : Screen
    {
        internal BlurEffect blurEffect;
        private RenderTarget2D gameScreenCapture;

        public PauseScreen(GraphicsDevice graphicsDevice, ContentManager content) : base(graphicsDevice, content)
        {
            blurEffect = new BlurEffect(graphicsDevice, content);
            RecreateRenderTarget();
        }

        public override void Initialize()
        {
            AddTextElement("Paused", new Vector2(0.5f, 0.2f), 0f, Color.White, 2f);
            AddButton("Resume", new Vector2(0.5f, 0.4f), new Vector2(0.3f, 0.1f), 0.2f, "wood_arrow", ResumeGame);
            AddButton("Options", new Vector2(0.5f, 0.55f), new Vector2(0.3f, 0.1f), -0.2f, "wood_button", OpenOptions);
            AddButton("Main Menu", new Vector2(0.5f, 0.7f), new Vector2(0.3f, 0.1f), 0.2f, "wood_button", ReturnToMainMenu);
        }

        public void RecreateRenderTarget()
        {
            if (gameScreenCapture != null)
            {
                gameScreenCapture.Dispose();
            }
            gameScreenCapture = new RenderTarget2D(
                graphicsDevice,
                graphicsDevice.PresentationParameters.BackBufferWidth,
                graphicsDevice.PresentationParameters.BackBufferHeight);
        }

        public void CaptureGameScreen(SpriteBatch spriteBatch)
        {
            graphicsDevice.SetRenderTarget(gameScreenCapture);
            spriteBatch.Begin();
            ScreenManager.Instance.Draw(spriteBatch);
            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);
        }

        private void ResumeGame()
        {
            ScreenManager.Instance.ChangeScreen(ScreenType.Gameplay);
        }

        private void OpenOptions()
        {
            ScreenManager.Instance.ChangeScreen(ScreenType.Settings);
        }

        private void ReturnToMainMenu()
        {
            ScreenManager.Instance.ChangeScreen(ScreenType.MainMenu);
        }

        public override void HandleInput()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            graphicsDevice.Clear(Color.Transparent);

            blurEffect.SetParameters(gameScreenCapture, 1f, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            blurEffect.ApplyHorizontalBlur(graphicsDevice, spriteBatch);

            Rectangle destinationRectangle = CalculateAspectRatioFit(
                gameScreenCapture.Width, 
                gameScreenCapture.Height, 
                graphicsDevice.Viewport.Width, 
                graphicsDevice.Viewport.Height);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            blurEffect.ApplyVerticalBlur();
            spriteBatch.Draw(blurEffect.TempTarget, destinationRectangle, Color.White);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.Draw(Texture2DHelper.GetWhiteTexture(graphicsDevice), graphicsDevice.Viewport.Bounds, new Color(0, 0, 0, 128));
            spriteBatch.End();

            spriteBatch.Begin();
            base.Draw(spriteBatch);
            spriteBatch.End();
        }

        private Rectangle CalculateAspectRatioFit(int srcWidth, int srcHeight, int maxWidth, int maxHeight)
        {
            var ratioX = (float)maxWidth / srcWidth;
            var ratioY = (float)maxHeight / srcHeight;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(srcWidth * ratio);
            var newHeight = (int)(srcHeight * ratio);

            var newX = (maxWidth - newWidth) / 2;
            var newY = (maxHeight - newHeight) / 2;

            return new Rectangle(newX, newY, newWidth, newHeight);
        }
    }
}