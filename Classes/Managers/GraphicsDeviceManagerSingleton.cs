using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Classes.Managers
{
    public class GraphicsDeviceSingleton
    {
        private static GraphicsDevice _graphicsDevice;

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        public static GraphicsDevice Instance
        {
            get
            {
                return _graphicsDevice;
            }
        }
    }

}
