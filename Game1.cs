﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using HandsOnDeck.Classes.Managers;

namespace HandsOnDeck
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static RenderTarget2D RenderTarget;
        public static int TimeModifier = 1;
        public static float ScaleModifier = 6;
        public static float EntityStatesSpeed = 1;
        public static SpriteFont DefaultFont { get; private set; }

        public const int ProgramWidth = 2048;
        public const int ProgramHeight = 1080;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            TargetElapsedTime = TimeSpan.FromSeconds(1d / 60);
            }
        protected override void Initialize()
        {
            Renderer.GetInstance.Initialize(_graphics);
            IsFixedTimeStep = true;
            ContentLoader.Initialize(Content);
            GraphicsDeviceSingleton.Initialize(_graphics.GraphicsDevice);
            base.Initialize();
            SpriteBatchManager.Initialize(_spriteBatch);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
            Renderer.GetInstance.LoadContent(Content, _spriteBatch);
            DefaultFont = Content.Load<SpriteFont>("default");  
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Renderer.GetInstance.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.SetRenderTarget(RenderTarget);
            GraphicsDevice.Clear(Color.Green);
            Window.AllowUserResizing = true;

            Renderer.GetInstance.Draw(gameTime);

            float outputAspect = Window.ClientBounds.Width / (float)Window.ClientBounds.Height;
            float preferredAspect = ProgramWidth / (float)ProgramHeight;

            Rectangle dst;

            if (outputAspect <= preferredAspect)
            {
                int presentHeight = (int)((Window.ClientBounds.Width / preferredAspect) + 0.5f);
                int barHeight = (Window.ClientBounds.Height - presentHeight) / 2;
                dst = new Rectangle(0, barHeight, Window.ClientBounds.Width, presentHeight);
            }
            else
            {
                int presentWidth = (int)((Window.ClientBounds.Height * preferredAspect) + 0.5f);
                int barWidth = (Window.ClientBounds.Width - presentWidth) / 2;
                dst = new Rectangle(barWidth, 0, presentWidth, Window.ClientBounds.Height);
            }

            _graphics.GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
            _spriteBatch.Draw(RenderTarget, dst, Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}