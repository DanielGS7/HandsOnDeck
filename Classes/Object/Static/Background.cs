using HandsOnDeck.Classes.Animations;
using HandsOnDeck.Classes.MonogameAccessibility;
using HandsOnDeck.Classes.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace HandsOnDeck.Classes.Object.Static
{
    internal class Background : GameObject
    {
        private static Background background;
        private static object syncRoot = new object();
        private static Animation water = new Animation("Swater", new Vector2(128, 128), 1, 6, 39, 15f, true);


        public override void LoadContent()
        {
            water.LoadContent();
        }
        public static Background GetInstance
        {
            get
            {
                if (background == null)
                {
                    lock (syncRoot)
                    {
                        if (background == null)
                            background = new Background();
                    }
                }
                return background;
            }
        }

        public override void Update(GameTime gameTime)
        {
            water.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            WorldCoordinate viewportOffset = GameScreen.GetInstance.viewportPosition;
            WorldCoordinate startPosition = new WorldCoordinate(-viewportOffset.X % 128, -viewportOffset.Y % 128);
            Draw(gameTime, startPosition);
        }

        public override void Draw(GameTime gameTime, WorldCoordinate position)
        {
            GraphicsDevice _graphics = GraphicsDeviceSingleton.GetInstance;
            SpriteBatch _spriteBatch = SpriteBatchManager.Instance;
            Vector2 screenSize = new Vector2(_graphics.Viewport.Width, _graphics.Viewport.Height);
            water.Draw(position, screenSize);
        }
    }
}
