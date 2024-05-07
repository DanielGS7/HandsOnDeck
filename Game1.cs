using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Classes.MonogameAccessibility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace HandsOnDeck
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static int TimeModifier = 1;
        public static float EntityStatesSpeed = 1;
        public const int ProgramWidth = 2048;
        public const int ProgramHeight = 1080;

        public const float ratio = ProgramWidth / ProgramHeight;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1d / 60);
        }
        protected override void Initialize()
        {
            Renderer.GetInstance.Initialize(_graphics, Content, Window);
            InputManager.GetInstance.Initialize(_graphics.GraphicsDevice);
            IsFixedTimeStep = true;
            base.Initialize();
            SpriteBatchManager.Initialize(_spriteBatch);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
            Renderer.GetInstance.LoadContent(_spriteBatch);
            GameStateManager.GetInstance.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.GetInstance.Update();
            GameStateManager.GetInstance.Update(gameTime);
            Renderer.GetInstance.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Renderer.GetInstance.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
