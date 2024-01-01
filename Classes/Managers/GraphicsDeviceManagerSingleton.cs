using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Classes.Managers
{
    public class GraphicsDeviceManagerSingleton
    {
        private static GraphicsDeviceManager _graphicsDevice;

        public static void Initialize(GraphicsDeviceManager graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        public static GraphicsDeviceManager Instance
        {
            get
            {
                return _graphicsDevice;
            }
        }
    }

}
