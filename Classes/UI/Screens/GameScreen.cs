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
        private List<GameObject> gameObjects;
        private Background bg;
        private Player player;

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
            gameObjects.Add(player);
            gameObjects.Add(bg);
        }

        public override void Initialize()
        {
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

            foreach (var gameObject in gameObjects)
            {
                gameObject.Draw(gameTime);
            }
            player.Draw(gameTime);

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
