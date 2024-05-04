﻿using Microsoft.Xna.Framework;
using HandsOnDeck.Classes.Object;
using System.Collections.Generic;
using HandsOnDeck.Classes.Object.Entity;
using HandsOnDeck.Classes.UI.Screens;
using System;
using HandsOnDeck.Classes.Object.Static;
using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Classes.UI.UIElements;

namespace HandsOnDeck.Classes.UI
{
    public class GameScreen : UIScreen
    {

        public Vector2 viewportPosition;
        public static Vector2 ViewportSize;
        public static Vector2 WorldSize;

        private List<GameObject> gameObjects;

        private Background bg;
        private Player player;
        private EnemyBoat enemy1;
        private KamikazeBoat enemy2;
        private ExplosiveBarrel barrel;
        private HeartContainer hearts;

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
            enemy1 = new EnemyBoat(new Vector2(1000,500));
            enemy2 = new KamikazeBoat(new Vector2(1200,600));
            barrel = new ExplosiveBarrel(new Vector2(700, 700));
            hearts = new HeartContainer(new Vector2(150,250));
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
            foreach (GameObject obj in gameObjects)
            {
                if (obj is CollideableGameObject collidableObj)
                {
                    CollisionManager.GetInstance.AddCollideableObject(collidableObj);
                }
            }

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
            UpdateViewportPosition();
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
            bg.Draw(gameTime, new Vector2(viewportPosition.X % 128, viewportPosition.Y % 128));
            IslandManager.GetInstance.Draw(gameTime);
            hearts.Draw(gameTime);

            foreach (var gameObject in gameObjects)
            {
                Vector2 drawPosition = gameObject.position - viewportPosition;
                Vector2 wrappedPosition = AdjustForWorldWrapping(drawPosition, gameObject.position);

                if (drawPosition == wrappedPosition)
                {
                    gameObject.Draw(gameTime, drawPosition);
                }
                else if (drawPosition != wrappedPosition)
                {
                    gameObject.Draw(gameTime, wrappedPosition);
                }
            }
        }

        private void UpdateViewportPosition()
        {
            Vector2 playerPosition = Player.GetInstance.position;
            Vector2 threshold = new Vector2(500, 400);
            Vector2 relativePlayerPosition = playerPosition - viewportPosition;

            if (relativePlayerPosition.X < threshold.X)
                viewportPosition.X = AdjustViewportEdge(playerPosition.X - threshold.X, viewportPosition.X, WorldSize.X);
            else if (relativePlayerPosition.X > ViewportSize.X - threshold.X)
                viewportPosition.X = AdjustViewportEdge(playerPosition.X - (ViewportSize.X - threshold.X), viewportPosition.X, WorldSize.X);


            if (relativePlayerPosition.Y < threshold.Y)
                viewportPosition.Y = AdjustViewportEdge(playerPosition.Y - threshold.Y, viewportPosition.Y, WorldSize.Y);
            else if (relativePlayerPosition.Y > ViewportSize.Y - threshold.Y)
                viewportPosition.Y = AdjustViewportEdge(playerPosition.Y - (ViewportSize.Y - threshold.Y), viewportPosition.Y, WorldSize.Y);


            viewportPosition.X = (viewportPosition.X + WorldSize.X) % WorldSize.X;
            viewportPosition.Y = (viewportPosition.Y + WorldSize.Y) % WorldSize.Y;
        }

        private float AdjustViewportEdge(float targetPosition, float currentViewport, float worldSize)
        {
            float directDistance = targetPosition - currentViewport;
            float wrappedDistance = (targetPosition + worldSize) - currentViewport;

            if (Math.Abs(directDistance) < Math.Abs(wrappedDistance))
                return currentViewport + directDistance * 0.1f;
            else
                return currentViewport + wrappedDistance * 0.05f;
        }

        public Vector2 AdjustForWorldWrapping(Vector2 drawPosition, Vector2 originalPosition)
        {
            Vector2 adjustedPosition = drawPosition;

            if (originalPosition.X < ViewportSize.X && viewportPosition.X > WorldSize.X - ViewportSize.X)
            {
                adjustedPosition.X += WorldSize.X;
            }
            else if (originalPosition.X > WorldSize.X - ViewportSize.X && viewportPosition.X < ViewportSize.X)
            {
                adjustedPosition.X -= WorldSize.X; 
            }

            if (originalPosition.Y < ViewportSize.Y && viewportPosition.Y > WorldSize.Y - ViewportSize.Y)
            {
                adjustedPosition.Y += WorldSize.Y;
            }
            else if (originalPosition.Y > WorldSize.Y - ViewportSize.Y && viewportPosition.Y < ViewportSize.Y)
            {
                adjustedPosition.Y -= WorldSize.Y; 
            }
            return adjustedPosition;
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
    enemy1 = new EnemyBoat(new Vector2(1000,500));
    enemy2 = new KamikazeBoat(new Vector2(1200,600));
    barrel = new ExplosiveBarrel(new Vector2(700, 700));
    hearts = new HeartContainer(new Vector2(150,250));

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
