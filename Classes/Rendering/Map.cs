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
using System.Linq;

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
        public Boat player;
        private Camera camera;
        private ContentManager content;
        private GraphicsDevice graphicsDevice;
        public static int MapWidth { get; private set; }
        public static int MapHeight { get; private set; }
        public Camera Camera { get => camera; set => camera = value; }

        private List<Enemy> enemies;
        private List<IProjectile> projectiles;
        private List<Siren> sirens;

        private float timeSinceLastSpawn = 0f;
        private DifficultySettings difficultySettings;
        private const float SpawnInterval = 2f;
        private float enemySpeedMultiplier;
        private CannonballFactory cannonballFactory;
        private BombFactory bombFactory;

        private Map()
        {
            islands = new List<Island>();
            Camera = new Camera();
            difficultySettings = DifficultySettings.Instance;
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
            enemies = new List<Enemy>();
            projectiles = new List<IProjectile>();
            sirens = new List<Siren>();
            islands = Island.GenerateIslands(content, graphicsDevice, Island.totalIslands);
            cannonballFactory = new CannonballFactory(content);
            bombFactory = new BombFactory(content);
            enemySpeedMultiplier = GetEnemySpeedMultiplier();
        }

        public void LoadContent()
        {
            Background.Instance.Initialize(content, graphicsDevice);
        }


        public void Update(GameTime gameTime)
        {
            InputManager.Instance.Update();
            InputManager.Instance.HandleInput(player);

            player.Update(gameTime);

            Camera.Update(player.Position, graphicsDevice.Viewport, MapWidth, MapHeight);

            Background.Instance.Update(gameTime);
            foreach (var island in islands)
            {
                island.Update(gameTime);
            }
            CollisionManager.Instance.Update(gameTime);

            foreach (var siren in sirens)
            {
                siren.Update(gameTime);
            }
            CollisionManager.Instance.Update(gameTime);

            if (InputManager.Instance.IsKeyHeld(Keys.OemPlus))
                Background.Instance.SetScale(Background.Instance.GetScale() + 0.01f);
            if (InputManager.Instance.IsKeyHeld(Keys.OemMinus))
                Background.Instance.SetScale(Background.Instance.GetScale() - 0.01f);
            /*if (InputManager.Instance.IsKeyPressed(Keys.R))
                Background.Instance.SetRotation((Background.Instance.GetRotation() + 90f) % 360f);*/

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeSinceLastSpawn += deltaTime;

            if (timeSinceLastSpawn >= difficultySettings.GetEnemySpawnInterval() && enemies.Count < difficultySettings.GetMaxEnemiesInGame())
            {
                SpawnEnemy();
                timeSinceLastSpawn = 0f;
            }

            UpdateEnemies(gameTime);
            UpdateProjectiles(gameTime);
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                var enemy = enemies[i];
                enemy.Update(gameTime);
            }
        }

        private float GetEnemySpeedMultiplier()
        {
            switch (GlobalInfo.Instance.CurrentDifficulty)
            {
                case Difficulty.Easy: return 0.8f;
                case Difficulty.Normal: return 1f;
                case Difficulty.Hard: return 1.2f;
                default: return 1f;
            }
        }

        private void UpdateProjectiles(GameTime gameTime)
        {
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                var projectile = projectiles[i];
                projectile.Update(gameTime);
                if (projectile.IsExpired)
                {
                    projectiles.RemoveAt(i);
                }
            }
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

            foreach (var island in islands) DrawWrappedObject(spriteBatch, island, viewport);
            foreach (var enemy in enemies) DrawWrappedObject(spriteBatch, enemy, viewport);
            foreach (var projectile in projectiles) DrawWrappedObject(spriteBatch, projectile, viewport);
            DrawWrappedObject(spriteBatch, player, viewport);
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

        private void DrawWrappedObject(SpriteBatch spriteBatch, IGameObject obj, Rectangle viewport)
        {
            float objectRadius = Math.Max(obj.Size.X, obj.Size.Y) * obj.Scale / 2;
            Rectangle extendedViewport = new Rectangle(
                viewport.X - (int)objectRadius,
                viewport.Y - (int)objectRadius,
                viewport.Width + (int)(objectRadius * 2),
                viewport.Height + (int)(objectRadius * 2)
            );

            Vector2 objPos = obj.Position.ToVector2();
            Vector2[] positions = new Vector2[]
            {
            objPos,
            new Vector2(objPos.X - MapWidth, objPos.Y),
            new Vector2(objPos.X + MapWidth, objPos.Y),
            new Vector2(objPos.X, objPos.Y - MapHeight),
            new Vector2(objPos.X, objPos.Y + MapHeight),
            new Vector2(objPos.X - MapWidth, objPos.Y - MapHeight),
            new Vector2(objPos.X + MapWidth, objPos.Y - MapHeight),
            new Vector2(objPos.X - MapWidth, objPos.Y + MapHeight),
            new Vector2(objPos.X + MapWidth, objPos.Y + MapHeight)
            };

            foreach (var pos in positions)
            {
                if (extendedViewport.Contains(pos))
                {
                    obj.Position = new SeaCoordinate(pos.X, pos.Y);
                    obj.Draw(spriteBatch);
                }
            }
        }
        private bool IsPositionVisible(Vector2 position, Rectangle viewport)
        {
            return viewport.Contains(position);
        }

        private void SpawnEnemy()
        {
            Random random = new Random();
            Vector2 spawnPosition = GetRandomSpawnPosition();

            Enemy newEnemy;
            switch (random.Next(2))
            {
                case 0:
                    newEnemy = new RivalBoat(content, new SeaCoordinate(spawnPosition.X, spawnPosition.Y), cannonballFactory);
                    break;
                case 1:
                default:
                    newEnemy = new Bomber(content, new SeaCoordinate(spawnPosition.X, spawnPosition.Y), bombFactory);
                    break;
            }
            newEnemy.Speed *= enemySpeedMultiplier;
            enemies.Add(newEnemy);
            CollisionManager.Instance.AddCollideable(newEnemy);
        }
        public void AddSiren(Siren siren)
        {
            sirens.Add(siren);
        }

        private Vector2 GetRandomSpawnPosition()
        {
            Random random = new Random();
            Vector2 viewportCenter = Camera.Position;
            float viewportWidth = graphicsDevice.Viewport.Width;
            float viewportHeight = graphicsDevice.Viewport.Height;

            float spawnDistance = Math.Max(viewportWidth, viewportHeight) * 1.2f;
            float angle = (float)random.NextDouble() * MathHelper.TwoPi;

            Vector2 spawnOffset = new Vector2(
                (float)Math.Cos(angle) * spawnDistance,
                (float)Math.Sin(angle) * spawnDistance
            );

            Vector2 spawnPosition = viewportCenter + spawnOffset;
            spawnPosition.X = (spawnPosition.X + MapWidth) % MapWidth;
            spawnPosition.Y = (spawnPosition.Y + MapHeight) % MapHeight;

            return spawnPosition;
        }
        public void AddProjectile(IProjectile projectile)
        {
            projectiles.Add(projectile);
        }
        public List<IProjectile> GetProjectiles()
        {
            return projectiles;
        }
        internal void RemoveEnemy(Enemy enemy)
        {
            enemies.Remove(enemy);
            CollisionManager.Instance.RemoveCollideable(enemy);
        }
        public void RemoveProjectile(IProjectile projectile)
        {
            projectiles.Remove(projectile);
        }
        internal IEnumerable<Island> GetIslands()
        {
            return islands;
        }

        internal IEnumerable<object> GetEnemies()
        {
            return enemies;
        }
        public IEnumerable<ICollideable> GetCollideables()
        {
            var collideables = new List<ICollideable>();
            collideables.AddRange(islands);
            collideables.AddRange(enemies);
            collideables.Add(player);
            collideables.AddRange(projectiles.OfType<ICollideable>());
            return collideables;
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