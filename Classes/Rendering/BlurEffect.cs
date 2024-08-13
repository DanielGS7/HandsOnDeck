using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace HandsOnDeck2.Classes.Rendering
{
    public class BlurEffect
    {
        private Effect effect;
        private RenderTarget2D tempTarget;

        public RenderTarget2D TempTarget => tempTarget;
        public BlurEffect(GraphicsDevice graphicsDevice, ContentManager content)
        {
            effect = content.Load<Effect>("Effects/blur_effect");
            RecreateRenderTarget(graphicsDevice);
        }

        public void RecreateRenderTarget(GraphicsDevice graphicsDevice)
        {
            if (tempTarget != null && !tempTarget.IsDisposed &&
                (tempTarget.Width != graphicsDevice.PresentationParameters.BackBufferWidth ||
                 tempTarget.Height != graphicsDevice.PresentationParameters.BackBufferHeight))
            {
                tempTarget.Dispose();
                tempTarget = null;
            }

            if (tempTarget == null)
            {
                tempTarget = new RenderTarget2D(
                    graphicsDevice,
                    graphicsDevice.PresentationParameters.BackBufferWidth,
                    graphicsDevice.PresentationParameters.BackBufferHeight);
            }
        }

        public void SetParameters(Texture2D texture, float blurAmount, int screenWidth, int screenHeight)
        {
            effect.Parameters["GameTexture"].SetValue(texture);
            effect.Parameters["BlurAmount"].SetValue(blurAmount);
            effect.Parameters["ScreenSize"].SetValue(new Vector2(screenWidth, screenHeight));
        }

        public void ApplyHorizontalBlur(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            graphicsDevice.SetRenderTarget(tempTarget);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            effect.CurrentTechnique = effect.Techniques["GaussianBlurTechnique"];
            effect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(effect.Parameters["GameTexture"].GetValueTexture2D(), Vector2.Zero, Color.White);
            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);
        }

        public void ApplyVerticalBlur(SpriteBatch spriteBatch)
        {
            effect.Parameters["GameTexture"].SetValue(tempTarget);
            effect.CurrentTechnique = effect.Techniques["GaussianBlurTechnique"];
            effect.CurrentTechnique.Passes[1].Apply();
        }

    }
}