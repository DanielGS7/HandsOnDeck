using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RenderTargetUsage = Microsoft.Xna.Framework.Graphics.RenderTargetUsage;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;
using HandsOnDeck.Classes.Object.Static;
using Microsoft.Xna.Framework.Input;

namespace HandsOnDeck.Classes.Managers
{

    public sealed class Renderer
    {
        private static Renderer renderer;
        private static object syncRoot = new object();

        internal SpriteBatch _spriteBatch;
        GraphicsDeviceManager graphics;
        //private MousePositionDisplay mousePositionDisplay;

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

        internal void Initialize(GraphicsDeviceManager _graphics)
        {
            PresentationParameters pp = _graphics.GraphicsDevice.PresentationParameters;
            Game1.RenderTarget = new RenderTarget2D(_graphics.GraphicsDevice, Game1.ProgramWidth, Game1.ProgramHeight, false,
            SurfaceFormat.Color, DepthFormat.None, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);
            graphics = _graphics;
            MapOverlay.GetInstance.Initialize();
            InputManager.GetInstance.Initialize();
        }

        public void LoadContent(ContentManager content, SpriteBatch _spriteBatch)
        {
            this._spriteBatch = _spriteBatch;
            Background.GetInstance.LoadContent();
            GameStateManager.GetInstance.LoadContent();
            MapOverlay.GetInstance.LoadContent();
            //mousePositionDisplay = new MousePositionDisplay();
        }
        public void Update(GameTime gameTime)
        {
            Background.GetInstance.Update(gameTime);
            InputManager.GetInstance.SetCurrentKeyboardState(Keyboard.GetState());
            GameStateManager.GetInstance.Update(gameTime);
            //mousePositionDisplay.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            GetInstance._spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
            //Alles dat getekend moet worden komt onder deze lijn
            Background.GetInstance.Draw(gameTime);
            GameStateManager.GetInstance.Draw(gameTime);
            MapOverlay.GetInstance.Draw(gameTime);
            //Alles dat getekend moet worden komt boven deze lijn
            GetInstance._spriteBatch.End();
        }
    }
}