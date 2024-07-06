using HandsOnDeck2.Classes.Collisions;
using HandsOnDeck2.Enums;
using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

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
            MapWidth = 5120;
            MapHeight = 2880;

            SeaCoordinate.SetMapDimensions(MapWidth, MapHeight);

            player = new Boat(content, new SeaCoordinate(MapWidth / 2, MapHeight / 2));
            islands = Island.GenerateIslands(content, graphicsDevice, Island.totalIslands);
        }

        public void LoadContent()
        {
            Background.Instance.Initialize(content, graphicsDevice);
        }


       public void Update(GameTime gameTime)
        {
            InputManager.Instance.Update();
            InputManager.Instance.HandleInput(player);
            
            SeaCoordinate previousPosition = player.Position;
            player.Update(gameTime);

            if (player.Position.X != previousPosition.X || player.Position.Y != previousPosition.Y)
            {
                Camera.AdjustPositionForTeleport(previousPosition, player.Position);
            }

            Camera.Update(player.Position, graphicsDevice.Viewport, MapWidth, MapHeight);

            Background.Instance.Update(gameTime, Camera.GetOffset());
            foreach (var island in islands)
            {
                island.Update(gameTime);
            }
            CollisionManager.Instance.Update(gameTime);

            // Handle additional input for background scaling and rotation
            if (InputManager.Instance.IsKeyHeld(Keys.OemPlus))
                Background.Instance.SetScale(Background.Instance.GetScale() + 0.01f);
            if (InputManager.Instance.IsKeyHeld(Keys.OemMinus))
                Background.Instance.SetScale(Background.Instance.GetScale() - 0.01f);
            if (InputManager.Instance.IsKeyPressed(Keys.R))
                Background.Instance.SetRotation((Background.Instance.GetRotation() + 90f) % 360f);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: Camera.Transform);
            Background.Instance.Draw(spriteBatch, Camera, graphicsDevice.Viewport);

            DrawVisibleObjects(spriteBatch);
            CollisionManager.Instance.Draw(spriteBatch);

            spriteBatch.End();
        }

        private void DrawVisibleObjects(SpriteBatch spriteBatch)
        {
            Rectangle viewport = new Rectangle(
                (int)Camera.Position.X,
                (int)Camera.Position.Y,
                graphicsDevice.Viewport.Width,
                graphicsDevice.Viewport.Height
            );

            foreach (var island in islands)
            {
                if (IsObjectVisible(island, viewport))
                {
                    island.Draw(spriteBatch);
                }
            }

            player.Draw(spriteBatch);
        }

        private bool IsObjectVisible(IGameObject obj, Rectangle viewport)
        {
            float objectRadius = Math.Max(obj.Size.X, obj.Size.Y) * obj.Scale / 2;
            Rectangle extendedViewport = new Rectangle(
                viewport.X - (int)objectRadius,
                viewport.Y - (int)objectRadius,
                viewport.Width + (int)(objectRadius * 2),
                viewport.Height + (int)(objectRadius * 2)
            );

            Vector2 objPos = obj.Position.ToVector2();
            return IsPositionVisible(objPos, extendedViewport) ||
                   IsPositionVisible(new Vector2(objPos.X - MapWidth, objPos.Y), extendedViewport) ||
                   IsPositionVisible(new Vector2(objPos.X + MapWidth, objPos.Y), extendedViewport) ||
                   IsPositionVisible(new Vector2(objPos.X, objPos.Y - MapHeight), extendedViewport) ||
                   IsPositionVisible(new Vector2(objPos.X, objPos.Y + MapHeight), extendedViewport) ||
                   IsPositionVisible(new Vector2(objPos.X - MapWidth, objPos.Y - MapHeight), extendedViewport) ||
                   IsPositionVisible(new Vector2(objPos.X + MapWidth, objPos.Y - MapHeight), extendedViewport) ||
                   IsPositionVisible(new Vector2(objPos.X - MapWidth, objPos.Y + MapHeight), extendedViewport) ||
                   IsPositionVisible(new Vector2(objPos.X + MapWidth, objPos.Y + MapHeight), extendedViewport);
        }

        private bool IsPositionVisible(Vector2 position, Rectangle viewport)
        {
            return viewport.Contains(position);
        }
    }
}