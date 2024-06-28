using HandsOnDeck2.Classes.UI;
using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace HandsOnDeck2.Classes.States
{
    public class MainMenuState : IGameState
    {
        private ContentManager content;
        private GraphicsDevice graphicsDevice;
        private List<Button> buttons;
        private SpriteFont font;

        public MainMenuState(ContentManager content, GraphicsDevice graphicsDevice)
        {
            this.content = content;
            this.graphicsDevice = graphicsDevice;
            buttons = new List<Button>();
        }

        public void Enter()
        {
            font = content.Load<SpriteFont>("default"); // Ensure you have a sprite font file named FontName.spritefont in your content

            // Initialize buttons
            var playButton = new Button(content, new Rectangle(400, 100, 200, 50), new Vector2((graphicsDevice.Viewport.Width * 1.1f), (graphicsDevice.Viewport.Height * 0.7f)), "Play", font);
            var settingsButton = new Button(content, new Rectangle(400, 100, 200, 50), new Vector2((graphicsDevice.Viewport.Width * 1.1f), (graphicsDevice.Viewport.Height * 0.9f)), "Settings", font);
            var exitButton = new Button(content, new Rectangle(400, 100, 200, 50), new Vector2((graphicsDevice.Viewport.Width * 1.1f), (graphicsDevice.Viewport.Height * 1.1f)), "Exit", font);
            buttons.Add(playButton);
            buttons.Add(settingsButton);
            buttons.Add(exitButton);

            // Additional buttons can be added similarly
        }

        public void Exit()
        {
            // Unload content or clean up resources
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            foreach (var button in buttons)
            {
                button.HandleInput(mouseState);
                button.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var button in buttons)
            {
                button.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
