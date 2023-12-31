﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck.Classes.Object;
using System.Collections.Generic;
using HandsOnDeck.Classes.Object.Entity;
using HandsOnDeck.Classes.UI.Screens;
using System;
using HandsOnDeck.Classes.Object.Static;

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

        public bool isPaused;

        private static GameScreen instance;

        public static GameScreen Instance
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

        public void TogglePause()
        {
            isPaused = !isPaused;
        }

        private GameScreen()
        {
            gameObjects = new List<GameObject>();
            player = Player.GetInstance();
            bg = Background.GetInstance;
            enemy1 = new EnemyBoat(new Vector2(1000,500));
            enemy2 = new KamikazeBoat(new Vector2(1200,600));
            gameObjects.Add(enemy1);
            gameObjects.Add(enemy2);
            gameObjects.Add(player);
            
        }

        private void UpdateViewportPosition()
        {
            Vector2 playerPosition = Player.GetInstance().position;
            Vector2 threshold = new Vector2(400, 400);
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
                return currentViewport + wrappedDistance * 0.1f;
        }
        public override void Initialize()
        {
            ViewportSize = new Vector2(2048, 1080);
            WorldSize = new Vector2(20480, 10800);
            base.Initialize();
        }

        internal override void LoadContent()
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.LoadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (isPaused) return;
            UpdateViewportPosition();
            base.Update(gameTime);
            foreach (var gameObject in gameObjects)
            {
                gameObject.Update(gameTime);
            }
            player.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            bg.Draw(gameTime, new Vector2(viewportPosition.X % 128, viewportPosition.Y % 128));

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

        private Vector2 AdjustForWorldWrapping(Vector2 drawPosition, Vector2 originalPosition)
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

        private bool IsWithinViewport(Vector2 position, Vector2 originalPosition)
        {
            if (viewportPosition.X < ViewportSize.X && originalPosition.X > WorldSize.X - ViewportSize.X)
                position.X += WorldSize.X;
            else if (viewportPosition.X > WorldSize.X - ViewportSize.X && originalPosition.X < ViewportSize.X)
                position.X -= WorldSize.X;

            if (viewportPosition.Y < ViewportSize.Y && originalPosition.Y > WorldSize.Y - ViewportSize.Y)
                position.Y += WorldSize.Y;
            else if (viewportPosition.Y > WorldSize.Y - ViewportSize.Y && originalPosition.Y < ViewportSize.Y)
                position.Y -= WorldSize.Y;

            return position.X >= 0 && position.X <= ViewportSize.X &&
                   position.Y >= 0 && position.Y <= ViewportSize.Y;
        }


        public void ResetGame()
        {
            gameObjects.Clear();
            player = Player.GetInstance();
            player.Reset();
            bg = Background.GetInstance;
            gameObjects.Add(player);
            gameObjects.Add(bg);
        }
    }
}
