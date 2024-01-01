using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Classes.Managers
{
    public class SpriteBatchManager
    {
        private static SpriteBatch _spriteBatch;

        public static void Initialize(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        public static SpriteBatch Instance
        {
            get
            {
                return _spriteBatch;
            }
        }
    }
}
