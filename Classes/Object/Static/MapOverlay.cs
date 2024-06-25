using HandsOnDeck.Classes.Animations;
using HandsOnDeck.Interfaces;
using Microsoft.Xna.Framework;

namespace HandsOnDeck.Classes.Object.Static
{
    internal class MapOverlay : IGameObject
    {
        private static MapOverlay mapOverlay;
        private static object syncRoot = new object();
        private static Animation _overlay;

        public static MapOverlay GetInstance
        {
            get
            {
                if (mapOverlay == null)
                {
                    lock (syncRoot)
                    {
                        if (mapOverlay == null)
                            mapOverlay = new MapOverlay();
                    }
                }
                return mapOverlay;
            }
        }
        public void Initialize()
        {
            _overlay = new Animation("mapOverlay", new Vector2(1024, 576), 0, 1, 1, 0, false);
        }

        public void LoadContent()
        {
            _overlay.LoadContent();
        }
        public void Draw(GameTime gameTime)
        {
            _overlay.DrawStatic(Vector2.Zero, 2, 0, Vector2.Zero);
        }

        public void Update(GameTime gameTime){} //No update logic needed for this
    }
}
