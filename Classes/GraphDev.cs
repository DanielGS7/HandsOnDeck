using Microsoft.Xna.Framework.Graphics;
namespace HandsOnDeck2.Classes
{
    public class GraphDev
    {
        private static GraphicsDevice _graphicsDevice;

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        public static GraphicsDevice GetInstance { get { return _graphicsDevice; } }
    }
}
