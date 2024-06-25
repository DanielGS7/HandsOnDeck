using HandsOnDeck.Classes.MonogameAccessibility;
using HandsOnDeck.Classes.Object.Static;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck.Classes.Managers
{
    public sealed class Renderer
    {
        private static Renderer renderer;
        private static readonly object syncRoot = new object();
        private SpriteBatch _spriteBatch;
        private GraphicsDeviceManager _graphics;
        public static RenderTarget2D RenderTarget;
        public static SpriteFont DefaultFont { get; private set; }
        public static Texture2D ButtonSprite { get; private set; }
        public static Texture2D ButtonHoverSprite { get; private set; }
        public static ContentManager Content {get; private set;}
        public static GameWindow Window {get; private set;}
        //public static ShaderManager shaderManager;
        public static Vector2 CurrentScale
        {
            get
            {
                float scaleX = RenderTarget.Width / 2048f; 
                float scaleY = RenderTarget.Height / 1080f;
                return new Vector2(scaleX, scaleY);
            }
        }

        public static Vector2 CurrentTranslation
        {
            get
            {
                Rectangle dst = GetInstance.CalculateDestinationRectangle(Window.ClientBounds.Width / (float)Window.ClientBounds.Height, 
                                                            Game1.ProgramWidth / (float)Game1.ProgramHeight);
                return new Vector2(dst.X, dst.Y);
            }
        }

        public static Vector2 currentPaddingSize = new Vector2();

        // om te kijken waar mijn muis is
        public static MousePositionDisplay mousePositionDisplay;

        private Renderer() { }

        public static Renderer GetInstance
        {
            get
            {
                if (renderer == null)
                {
                    lock (syncRoot)
                    {
                        if (renderer == null)
                            renderer = new Renderer();
                    }
                }
                return renderer;
            }
        }
        internal void Initialize(GraphicsDeviceManager graphics, ContentManager content, GameWindow window)
        {
            _graphics = graphics;
            Window = window;
            PresentationParameters pp = _graphics.GraphicsDevice.PresentationParameters;
            Content = content;
            ContentLoader.Initialize(Content);
            GraphicsDeviceSingleton.Initialize(_graphics.GraphicsDevice);
            //shaderManager = ShaderManager.GetInstance;
            RenderTarget = new RenderTarget2D(_graphics.GraphicsDevice, Game1.ProgramWidth, Game1.ProgramHeight, false, SurfaceFormat.Color, DepthFormat.None, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);
            MapOverlay.GetInstance.Initialize();
        }
        public void LoadContent(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            DefaultFont = Content.Load<SpriteFont>("default");
            ButtonSprite = Content.Load<Texture2D>("button/button");
            ButtonHoverSprite = Content.Load<Texture2D>("button/buttonH");
            Background.GetInstance.LoadContent();
            MapOverlay.GetInstance.LoadContent();
            Window.AllowUserResizing = true;
            mousePositionDisplay = new MousePositionDisplay();
            //shaderManager.LoadContent(Content);
        }
        public void Update(GameTime gameTime)
        { 
            //shaderManager.UpdateSunPosition(gameTime);
            Background.GetInstance.Update(gameTime);
            MapOverlay.GetInstance.Update(gameTime);
            mousePositionDisplay.Update(gameTime);
        }
        public void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.SetRenderTarget(RenderTarget);
            GraphicsDeviceSingleton.GetInstance.Clear(new Color(98, 91, 88));
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            GameStateManager.GetInstance.Draw(gameTime);
            MapOverlay.GetInstance.Draw(gameTime);
            _spriteBatch.End();

            float outputAspect = Window.ClientBounds.Width / (float)Window.ClientBounds.Height;
            float preferredAspect = Game1.ProgramWidth / (float)Game1.ProgramHeight;
            Rectangle dst = CalculateDestinationRectangle(outputAspect, preferredAspect);

            _graphics.GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
            _spriteBatch.Draw(RenderTarget, dst, Color.White);
            _spriteBatch.End();
        }

        public Rectangle CalculateDestinationRectangle(float outputAspect, float preferredAspect)
        {
            if (outputAspect <= preferredAspect)
            {
                int presentHeight = (int)((Window.ClientBounds.Width / preferredAspect) + 0.5f);
                int barHeight = (Window.ClientBounds.Height - presentHeight) / 2;
                return new Rectangle(0, barHeight, Window.ClientBounds.Width, presentHeight);
            }
            else
            {
                int presentWidth = (int)((Window.ClientBounds.Height * preferredAspect) + 0.5f);
                int barWidth = (Window.ClientBounds.Width - presentWidth) / 2;
                return new Rectangle(barWidth, 0, presentWidth, Window.ClientBounds.Height);
            }
        }
    }
}
