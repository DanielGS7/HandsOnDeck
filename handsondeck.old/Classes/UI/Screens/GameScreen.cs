using Microsoft.Xna.Framework;
using HandsOnDeck.Classes.Object;
using System.Collections.Generic;
using HandsOnDeck.Classes.Object.Entity;
using HandsOnDeck.Classes.UI.Screens;
using System;
using HandsOnDeck.Classes.Object.Static;
using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Classes.UI.UIElements;
using HandsOnDeck.Classes.MonogameAccessibility;

namespace HandsOnDeck.Classes.UI
{
    public class GameScreen : UIScreen
    {
        public WorldCoordinate viewportPosition;
        public static Vector2 ViewportSize;
        public static Vector2 WorldSize;

        private List<GameObject> gameObjects;
        private Background bg;
        private Player player;
        private EnemyBoat enemy1;
        private KamikazeBoat enemy2;
        private ExplosiveBarrel barrel;
        private HeartContainer hearts;
        public ViewportManager viewportManager;

        public bool isPaused;
        private static GameScreen instance;

        public static GameScreen GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameScreen();
                    instance.Initialize();
                }
                return instance;
            }
        }

        private GameScreen()
        {
            gameObjects = new List<GameObject>();
            player = Player.GetInstance;
            bg = Background.GetInstance;
            enemy1 = new EnemyBoat(new WorldCoordinate(1000,500));
            enemy2 = new KamikazeBoat(new WorldCoordinate(1200,600));
            barrel = new ExplosiveBarrel(new WorldCoordinate(700, 700));
            hearts = new HeartContainer(new WorldCoordinate(150,250));
            gameObjects.Add(enemy1);
            gameObjects.Add(enemy2);
            gameObjects.Add(barrel);
            gameObjects.Add(player);
            uiElements.Add(hearts);
        }

        public override void Initialize()
        {
            base.Initialize();
            ViewportSize = new Vector2(2048, 1080);
            WorldSize = new Vector2(10240, 5400);
            CollisionManager.GetInstance.Initialize(GraphicsDeviceSingleton.GetInstance);
            viewportManager = new ViewportManager(ViewportSize.X, ViewportSize.Y);
            foreach (GameObject obj in gameObjects)
            {
                if (obj is CollideableGameObject collidableObj)
                {
                    CollisionManager.GetInstance.AddCollideableObject(collidableObj);
                }
            }
            viewportPosition = new WorldCoordinate();
        }

        internal override void LoadContent()
        {
            IslandManager.GetInstance.LoadContent();
            foreach (var gameObject in gameObjects)
            {
                gameObject.LoadContent();
            }
            hearts.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            IslandManager.GetInstance.Update(gameTime);
            if (isPaused) return;

            viewportManager.UpdateViewportPosition(Player.GetInstance.position.ToVector2(), new Vector2(500, 400));
            
            base.Update(gameTime);
            foreach (var gameObject in gameObjects)
            {
                gameObject.Update(gameTime);
            }
            player.Update(gameTime);
            hearts.Update(gameTime);
            CollisionManager.GetInstance.CheckForCollisions();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            
            // Draw the background with wrapping
            bg.Draw(gameTime, viewportManager.GetDrawPosition(new WorldCoordinate(viewportPosition.X % 128, viewportPosition.Y % 128)));
            
            // Draw all game objects with wrapping
            foreach (var gameObject in gameObjects)
            {
                WorldCoordinate drawPosition = viewportManager.GetDrawPosition(gameObject.position);
                gameObject.Draw(gameTime, drawPosition);
            }

            // Draw UI elements
            hearts.Draw(gameTime);
            MapOverlay.GetInstance.Draw(gameTime);
            CollisionManager.GetInstance.DrawVisualizations(viewportPosition);
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            this.gameObjects.Remove(gameObject);
        }

        public void ResetGame()
        {
            // Clear all game objects and UI elements
            gameObjects.Clear();
            uiElements.Clear();

            // Reinitialize game objects
            player = Player.GetInstance;
            bg = Background.GetInstance;
            enemy1 = new EnemyBoat(new WorldCoordinate(1000,500));
            enemy2 = new KamikazeBoat(new WorldCoordinate(1200,600));
            barrel = new ExplosiveBarrel(new WorldCoordinate(700, 700));
            hearts = new HeartContainer(new WorldCoordinate(150,250));

            player.LoadContent();
            bg.LoadContent();
            enemy1.LoadContent();
            enemy2.LoadContent();
            barrel.LoadContent();

            gameObjects.Add(enemy1);
            gameObjects.Add(enemy2);
            gameObjects.Add(barrel);
            gameObjects.Add(player);

            hearts.LoadContent();

            uiElements.Add(hearts);
        }
    }
}
