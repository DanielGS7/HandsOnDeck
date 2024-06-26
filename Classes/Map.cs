using HandsOnDeck2.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace HandsOnDeck2.Classes
{
    public class Map
    {
        private List<Island> islands;
        private Boat player;
        private Camera camera;
        private ContentManager content;
        private GraphicsDevice graphicsDevice;

        public Map(ContentManager content, GraphicsDevice graphicsDevice)
        {
            this.content = content;
            this.graphicsDevice = graphicsDevice;
            islands = new List<Island>();
            camera = new Camera();
        }

        public void Initialize()
        {
            player = new Boat(content, new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2));
            islands = Island.GenerateIslands(content, graphicsDevice, 16);
        }

        public void LoadContent()
        {
            Background.Instance.Initialize(content, graphicsDevice);
        }

        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            // Handle player input
            if (keyboardState.IsKeyDown(Keys.W)) player.HandleInput(GameAction.SailsOpen);
            if (keyboardState.IsKeyDown(Keys.S)) player.HandleInput(GameAction.SailsClosed);
            if (keyboardState.IsKeyDown(Keys.A)) player.HandleInput(GameAction.SteerLeft);
            if (keyboardState.IsKeyDown(Keys.D)) player.HandleInput(GameAction.SteerRight);
            if (keyboardState.IsKeyDown(Keys.Q)) player.HandleInput(GameAction.ShootLeft);
            if (keyboardState.IsKeyDown(Keys.E)) player.HandleInput(GameAction.ShootRight);
            if (keyboardState.IsKeyDown(Keys.Space)) player.HandleInput(GameAction.ToggleAnchor);

            // Example of changing scale and rotation
            if (keyboardState.IsKeyDown(Keys.OemPlus)) Background.Instance.SetScale(Background.Instance.GetScale() + 0.01f); // Increase scale
            if (keyboardState.IsKeyDown(Keys.OemMinus)) Background.Instance.SetScale(Background.Instance.GetScale() - 0.01f); // Decrease scale
            if (keyboardState.IsKeyDown(Keys.R)) Background.Instance.SetRotation((Background.Instance.GetRotation() + 90f) % 360f); // Rotate 90 degrees

            // Update game objects
            player.Update(gameTime);
            Background.Instance.Update(gameTime);
            foreach (var island in islands)
            {
                island.Update(gameTime);
            }
            camera.Update(player.Position, graphicsDevice.Viewport);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: camera.Transform);
            Background.Instance.Draw(spriteBatch, camera, graphicsDevice.Viewport);
            foreach (var island in islands)
            {
                island.Draw(spriteBatch);
            }
            player.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
