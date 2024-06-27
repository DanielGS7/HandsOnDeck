using HandsOnDeck2.Enums;
using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace HandsOnDeck2.Classes
{
    public class Map
    {
        private static Map instance;
        public static Map Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Map();
                }
                return instance;
            }
        }

        private List<Island> islands;
        private Boat player;
        private Camera camera;
        private ContentManager content;
        private GraphicsDevice graphicsDevice;
        public static int MapWidth { get; private set; }
        public static int MapHeight { get; private set; }
        public Camera Camera { get => camera; set => camera = value; }

        private Map() 
        {
            islands = new List<Island>();
            Camera = new Camera();
        }

        public void Initialize(ContentManager content, GraphicsDevice graphicsDevice)
        {
            this.content = content;
            this.graphicsDevice = graphicsDevice;
            MapWidth = graphicsDevice.Viewport.Width * 5;
            MapHeight = graphicsDevice.Viewport.Height * 5;

            player = new Boat(content, new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2));
            islands = Island.GenerateIslands(content, graphicsDevice, Island.totalIslands);
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
            Camera.Update(player.Position, graphicsDevice.Viewport, MapWidth, MapHeight);
            DebugTools.ResetVerticalOffset();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: Camera.Transform);
            Background.Instance.Draw(spriteBatch, Camera, graphicsDevice.Viewport);

            foreach (var island in islands)
            {
                DrawObject(spriteBatch, island);
                DebugTools.DrawRectangle(spriteBatch, island,Color.Green);
                island.Collider.Draw(spriteBatch,graphicsDevice,island.VisualElement.Rotation);
            }

            DrawObject(spriteBatch, player);

            spriteBatch.End();

            spriteBatch.Begin();
        DebugTools.DrawObjectInfo(spriteBatch, player.Position, "bootpos", Color.White);

        Island nearestIsland = GetNearestIsland(player.Position);
        if (nearestIsland != null)
        {
            DebugTools.DrawObjectInfo(spriteBatch, nearestIsland.Position, "nearest island pos", Color.White);
        }

        spriteBatch.End();
        }

        private void DrawObject(SpriteBatch spriteBatch, IGameObject gameObject)
        {
            Vector2 adjustedPosition = WrapPosition(gameObject.Position);
            gameObject.VisualElement.SetPosition(adjustedPosition);
            gameObject.Draw(spriteBatch);


            int viewportWidth = graphicsDevice.Viewport.Width;
            int viewportHeight = graphicsDevice.Viewport.Height;

            if (adjustedPosition.X < viewportWidth)
            {
                Vector2 oppositePosition = adjustedPosition + new Vector2(MapWidth, 0);
                gameObject.VisualElement.SetPosition(oppositePosition);
                gameObject.Draw(spriteBatch);
            }
            if (adjustedPosition.X > MapWidth - viewportWidth)
            {
                Vector2 oppositePosition = adjustedPosition - new Vector2(MapWidth, 0);
                gameObject.VisualElement.SetPosition(oppositePosition);
                gameObject.Draw(spriteBatch);
            }
            if (adjustedPosition.Y < viewportHeight)
            {
                Vector2 oppositePosition = adjustedPosition + new Vector2(0, MapHeight);
                gameObject.VisualElement.SetPosition(oppositePosition);
                gameObject.Draw(spriteBatch);
            }
            if (adjustedPosition.Y > MapHeight - viewportHeight)
            {
                Vector2 oppositePosition = adjustedPosition - new Vector2(0, MapHeight);
                gameObject.VisualElement.SetPosition(oppositePosition);
                gameObject.Draw(spriteBatch);
            }
                        if (adjustedPosition.X < viewportWidth && adjustedPosition.Y < viewportHeight)
            {
                Vector2 oppositePosition = adjustedPosition + new Vector2(MapWidth, MapHeight);
                gameObject.VisualElement.SetPosition(oppositePosition);
                gameObject.Draw(spriteBatch);
            }
            if (adjustedPosition.X > MapWidth - viewportWidth && adjustedPosition.Y < viewportHeight)
            {
                Vector2 oppositePosition = adjustedPosition + new Vector2(-MapWidth, MapHeight);
                gameObject.VisualElement.SetPosition(oppositePosition);
                gameObject.Draw(spriteBatch);
            }
            if (adjustedPosition.X < viewportWidth && adjustedPosition.Y > MapHeight - viewportHeight)
            {
                Vector2 oppositePosition = adjustedPosition + new Vector2(MapWidth, -MapHeight);
                gameObject.VisualElement.SetPosition(oppositePosition);
                gameObject.Draw(spriteBatch);
            }
            if (adjustedPosition.X > MapWidth - viewportWidth && adjustedPosition.Y > MapHeight - viewportHeight)
            {
                Vector2 oppositePosition = adjustedPosition + new Vector2(-MapWidth, -MapHeight);
                gameObject.VisualElement.SetPosition(oppositePosition);
                gameObject.Draw(spriteBatch);
            }
        }

        private Vector2 WrapPosition(Vector2 position)
        {
            float wrappedX = position.X;
            float wrappedY = position.Y;

            if (position.X < 0)
            {
                wrappedX = MapWidth + position.X;
            }
            else if (position.X >= MapWidth)
            {
                wrappedX = position.X - MapWidth;
            }

            if (position.Y < 0)
            {
                wrappedY = MapHeight + position.Y;
            }
            else if (position.Y >= MapHeight)
            {
                wrappedY = position.Y - MapHeight;
            }

            return new Vector2(wrappedX, wrappedY);
        }

        private Island GetNearestIsland(Vector2 playerPosition)
        {
            Island nearestIsland = null;
            float nearestDistance = float.MaxValue;

            foreach (var island in islands)
            {
                float distance = Vector2.Distance(playerPosition, island.Position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestIsland = island;
                }
            }

            return nearestIsland;
        }
    }
}
