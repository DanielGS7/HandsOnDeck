using Microsoft.Xna.Framework;
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
        private EnemyBoat enemy1;
        private KamikazeBoat enemy2;

        public GameScreen()
        {
            gameObjects = new List<GameObject>();
            player = Player.GetInstance();
            bg = Background.GetInstance;
            enemy1 = new EnemyBoat(new Vector2(1000,500));
            enemy2 = new KamikazeBoat(new Vector2(1200,600));
            gameObjects.Add(enemy1);
            gameObjects.Add(enemy2);
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

    }
}
