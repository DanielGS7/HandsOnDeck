using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Classes.MonogameAccessibility;

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
        public static Texture2D ButtonSprite { get; private set; }
        public static Texture2D ButtonHoverSprite { get; private set; }
        public static Point transformedMousePosition;

        public const int ProgramWidth = 2048;
        public const int ProgramHeight = 1080;
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
            DefaultFont = Content.Load<SpriteFont>("default");
            ButtonSprite = Content.Load<Texture2D>("button/button");
            ButtonHoverSprite = Content.Load<Texture2D>("button/buttonH");
            Renderer.GetInstance.LoadContent(Content, _spriteBatch);
        }

        protected override void Update(GameTime gameTime)
        {
            Renderer.GetInstance.Update(gameTime);
            MouseState mouseState = Mouse.GetState();
            transformedMousePosition = Game1.TransformMousePosition(
                mouseState,
                _graphics.GraphicsDevice.Viewport.Width,
                _graphics.GraphicsDevice.Viewport.Height,
                Game1.RenderTarget.Width,
                Game1.RenderTarget.Height);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.SetRenderTarget(RenderTarget);
            GraphicsDevice.Clear(new Microsoft.Xna.Framework.Color(98, 91, 88));
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

        public static Point TransformMousePosition(MouseState mouseState, int windowWidth, int windowHeight, int renderTargetWidth, int renderTargetHeight)
        {
            float scaleX = renderTargetWidth / (float)windowWidth;
            float scaleY = renderTargetHeight / (float)windowHeight;

            return new Point(
                (int)(mouseState.X * scaleX),
                (int)(mouseState.Y * scaleY)
            );
        }
    }
}