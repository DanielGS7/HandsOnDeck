using HandsOnDeck2.Enums;
using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using HandsOnDeck2.Classes.GameObject.Entity;
using HandsOnDeck2.Classes.GameObject;
using HandsOnDeck2.Classes.Collision;
using HandsOnDeck2.Classes.Global;

namespace HandsOnDeck2.Classes.Rendering
{
    public class Map
    {
        private static Map _instance;
        private static readonly object _lock = new object();
        internal static Map Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Map();
                    }
                    return _instance;
                }
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
            CollisionManager.Instance.Reset();
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

            if (InputManager.Instance.IsKeyHeld(Keys.OemPlus))
                Background.Instance.SetScale(Background.Instance.GetScale() + 0.01f);
            if (InputManager.Instance.IsKeyHeld(Keys.OemMinus))
                Background.Instance.SetScale(Background.Instance.GetScale() - 0.01f);
            if (InputManager.Instance.IsKeyPressed(Keys.R))
                Background.Instance.SetRotation((Background.Instance.GetRotation() + 90f) % 360f);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(transformMatrix: Camera.Transform);
            Background.Instance.Draw(spriteBatch, Camera, graphicsDevice.Viewport);

            DrawVisibleObjects(spriteBatch);
            CollisionManager.Instance.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin();
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

                public Boat GetPlayerBoat()
        {
            return player;
        }

        public void LoadGameSaveData(GameSaveData gameState)
        {
            if (gameState == null)
            {
                throw new ArgumentNullException(nameof(gameState), "Game state cannot be null.");
            }

            if (gameState.PlayerBoat == null)
            {
                throw new InvalidOperationException("PlayerBoat in the loaded game state is null.");
            }

            CollisionManager.Instance.Reset();
            islands.Clear();

            MapWidth = gameState.MapWidth;
            MapHeight = gameState.MapHeight;
            SeaCoordinate.SetMapDimensions(MapWidth, MapHeight);

            player = new Boat(content, gameState.PlayerBoat.Position);
            player.LoadFromSaveData(gameState.PlayerBoat);
            CollisionManager.Instance.AddCollideable(player);

            foreach (var islandData in gameState.Islands)
            {
                var island = new Island(content, graphicsDevice, islandData.SpriteIndex, islandData.Name, islandData.Position, islandData.Scale, islandData.Rotation);
                islands.Add(island);
                CollisionManager.Instance.AddCollideable(island);
            }

            Camera.Position = player.Position.ToVector2();
            GlobalInfo.Score = gameState.Score;
        }

        public void SetPlayerBoat(Boat boat)
        {
            player = boat;
            CollisionManager.Instance.AddCollideable(player);
        }

        public List<Island> GetIslands()
        {
            return islands;
        }

        public void SetIslands(List<Island> newIslands)
        {
            islands = newIslands;
            foreach (var island in islands)
            {
                CollisionManager.Instance.AddCollideable(island);
            }
        }

        internal void ResetGame()
        {
            lock (_lock)
            {
                _instance = null;
            }
        }
    }
}