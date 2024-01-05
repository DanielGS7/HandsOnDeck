using HandsOnDeck.Classes.Animations;
using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HandsOnDeck.Classes.Object.Static
{
    internal class Background : IGameObject
    {
        private static Background background;
        private static object syncRoot = new object();
        private static Animation water = new Animation("Swater", new Vector2(128, 128), 1, 6, 39, 15f, true);


        public void LoadContent()
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

        public void Update(GameTime gameTime)
        {
            water.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            GraphicsDevice _graphics = GraphicsDeviceSingleton.Instance;
            SpriteBatch _spriteBatch = SpriteBatchManager.Instance;
            Vector2 position = Vector2.Zero;
            Vector2 screenSize = new Vector2(_graphics.Viewport.Width, _graphics.Viewport.Height);
            water.Draw(position, screenSize);
        }
    }
}
