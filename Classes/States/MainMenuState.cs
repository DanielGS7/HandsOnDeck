using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using HandsOnDeck2.Classes.UI;

namespace HandsOnDeck2.Classes.States
{
    public class MainMenuState : IGameState
    {
        private ContentManager content;
        private GraphicsDevice graphicsDevice;
        private List<Button> buttons;
        private Button playButton;

        public MainMenuState(ContentManager content, GraphicsDevice graphicsDevice)
        {
            this.content = content;
            this.graphicsDevice = graphicsDevice;
            buttons = new List<Button>();
        }

        public void Enter()
        {
            // Initialize buttons
            playButton = new Button(content, new Rectangle(400, 100, 200, 50), new Vector2((graphicsDevice.Viewport.Width*0.63f), (graphicsDevice.Viewport.Height*0.55f)));
            buttons.Add(playButton);

            // Additional buttons can be added similarly
        }

        public void Exit()
        {
            // Unload content or clean up resources
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            playButton.Position = new Vector2((graphicsDevice.Viewport.Width*0.63f), (graphicsDevice.Viewport.Height*0.55f));
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
