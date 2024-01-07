using Microsoft.Xna.Framework.Graphics;
namespace HandsOnDeck.Classes.MonogameAccessibility
{
    public class GraphicsDeviceSingleton
    {
        private static GraphicsDevice _graphicsDevice;

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        public static GraphicsDevice GetInstance { get { return _graphicsDevice; } }
    }
}
