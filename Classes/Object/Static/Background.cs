using System;
using HandsOnDeck.Classes.Animations;
using HandsOnDeck.Classes.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck.Classes.Object.Static
{
    internal class Background : GameObject
    {
        private static Background background;
        private static readonly object syncRoot = new object();
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
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime, WorldCoordinate position)
        {
            Vector2 tileSize = new Vector2(128, 128);
            int bufferTiles = 1;
            Vector2 startPosition = new Vector2(
                (position.X % tileSize.X) - tileSize.X * bufferTiles,
                (position.Y % tileSize.Y) - tileSize.Y * bufferTiles
            );

            int verticalTiles = (int)Math.Ceiling(GameScreen.ViewportSize.Y / tileSize.Y) + 2 * bufferTiles;
            int horizontalTiles = (int)Math.Ceiling(GameScreen.ViewportSize.X / tileSize.X) + 2 * bufferTiles;

            for (int x = 0; x < horizontalTiles; x++)
            {
                for (int y = 0; y < verticalTiles; y++)
                {
                    Vector2 drawPosition = new Vector2(
                        startPosition.X + x * tileSize.X,
                        startPosition.Y + y * tileSize.Y
                    );
                    water.DrawStatic(drawPosition, 1.0f, 0.0f, Vector2.Zero);
                }
            }
        }
    }
}
