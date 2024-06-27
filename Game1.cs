using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HandsOnDeck2.Classes;
using HandsOnDeck2.Classes.States;
using HandsOnDeck2.Enums;

namespace HandsOnDeck2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Map _map;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.IsFullScreen = false;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            GameStateManager.Instance.AddState(GameStates.MainMenu, new MainMenuState(Content, GraphicsDevice));
            GameStateManager.Instance.ChangeState(GameStates.MainMenu);
            //_map = Map.Instance;
            //_map.Initialize(Content, GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //_map.LoadContent();
            DebugTools.Initialize(GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            GameStateManager.Instance.Update(gameTime);


           // _map.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GameStateManager.Instance.Draw(_spriteBatch);
            //_map.Draw(_spriteBatch);
            base.Draw(gameTime);
        }
    }
}
