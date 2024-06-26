using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck2.Classes
{
    public static class Texture2DHelper
    {
        private static Texture2D whiteTexture;

        public static Texture2D GetWhiteTexture(GraphicsDevice graphicsDevice)
        {
            if (whiteTexture == null)
            {
                whiteTexture = new Texture2D(graphicsDevice, 1, 1);
                whiteTexture.SetData(new[] { Color.White });
            }
            return whiteTexture;
        }
    }
}
