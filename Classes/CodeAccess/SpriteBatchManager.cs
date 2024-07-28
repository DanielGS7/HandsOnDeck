using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck2.Classes.CodeAccess
{
    public class SpriteBatchManager
    {
        private static SpriteBatch _spriteBatch;

        public static void Initialize(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        public static SpriteBatch Instance { get { return _spriteBatch; } }
    }
}
