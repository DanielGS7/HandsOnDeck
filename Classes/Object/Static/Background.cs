using HandsOnDeck.Classes.Animations;
using HandsOnDeck.Classes.Managers;
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
            Vector2 viewportOffset = GameScreen.Instance.viewportPosition;
            Vector2 startPosition = new Vector2(-viewportOffset.X % 128, -viewportOffset.Y % 128);
            Draw(gameTime, startPosition);
        }

        public override void Draw(GameTime gameTime, Vector2 position)
        {
            GraphicsDevice _graphics = GraphicsDeviceSingleton.Instance;
            SpriteBatch _spriteBatch = SpriteBatchManager.Instance;
            Vector2 screenSize = new Vector2(_graphics.Viewport.Width, _graphics.Viewport.Height);
            water.Draw(position, screenSize);
        }
    }
}
