using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Classes.Managers
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
