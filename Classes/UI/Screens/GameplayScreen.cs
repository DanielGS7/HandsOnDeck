using HandsOnDeck2.Classes.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck2.Classes.UI.Screens
{
    public class GameplayScreen : Screen
    {
        private Map gameMap;

        public GameplayScreen(GraphicsDevice graphicsDevice, ContentManager content) : base(graphicsDevice, content)
        {
        }

        public override void Initialize()
        {
            if (gameMap == null)
            {
                gameMap = Map.Instance;
                gameMap.Initialize(content, graphicsDevice);
                base.Initialize();
            }
        }

        public override void LoadContent()
        {
            gameMap.LoadContent();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                gameMap.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            gameMap.Draw(spriteBatch);
        }

        public override void HandleInput()
        {
        }
    }
}