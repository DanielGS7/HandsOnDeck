using Microsoft.Xna.Framework.Content;

namespace HandsOnDeck.Classes.MonogameAccessibility
{
    public class ContentLoader
    {
        private static ContentManager _contentManager;

        public static void Initialize(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public static T Load<T>(string assetName)
        {
            return _contentManager.Load<T>(assetName);
        }
    }

}
