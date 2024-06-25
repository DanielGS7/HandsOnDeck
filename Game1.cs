using HandsOnDeck2.Classes;
using HandsOnDeck2.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HandsOnDeck2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Boat _boat;
        private Map _map;
        private Camera _camera;
        private bool _isFullscreen = false;

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
            _camera = new Camera();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var boatTexture = Content.Load<Texture2D>("boat");
            var boatAnimation = new Animation("movingBoat", new Vector2(670, 243), 5, 5, 4f, true);
            boatAnimation.LoadContent(Content);
            _boat = new Boat(boatAnimation, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2));

            var islandTexture = Content.Load<Texture2D>("island");
            _map = new Map(islandTexture, GraphicsDevice.Viewport.Width * 10, GraphicsDevice.Viewport.Height * 10);
            Background.Instance.Initialize(Content, GraphicsDevice);
            Background.Instance.SetScale(1f);
            Background.Instance.SetRotation(90f);
            Background.Instance.SetDirection(new Vector2(1, -1));
            Background.Instance.SetMoveSpeed(30f);
            Background.Instance.SetAnimationSpeed(7f);

            DebugTools.Initialize(GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();


            if (keyboardState.IsKeyDown(Keys.F))
            {
                _isFullscreen = !_isFullscreen;
                _graphics.IsFullScreen = _isFullscreen;
                _graphics.ApplyChanges();
            }

            if (keyboardState.IsKeyDown(Keys.W)) _boat.HandleInput(GameAction.SailsOpen);
            if (keyboardState.IsKeyDown(Keys.S)) _boat.HandleInput(GameAction.SailsClosed);
            if (keyboardState.IsKeyDown(Keys.A)) _boat.HandleInput(GameAction.SteerLeft);
            if (keyboardState.IsKeyDown(Keys.D)) _boat.HandleInput(GameAction.SteerRight);
            if (keyboardState.IsKeyDown(Keys.Q)) _boat.HandleInput(GameAction.ShootLeft);
            if (keyboardState.IsKeyDown(Keys.E)) _boat.HandleInput(GameAction.ShootRight);
            if (keyboardState.IsKeyDown(Keys.Space)) _boat.HandleInput(GameAction.ToggleAnchor);

            _boat.Update(gameTime);
            _camera.Update(_boat.Position, GraphicsDevice.Viewport);

            Background.Instance.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(transformMatrix: _camera.Transform);
            Background.Instance.Draw(_spriteBatch, _camera, GraphicsDevice.Viewport);
            _map.Draw(_spriteBatch);
            _boat.Draw(_spriteBatch);
            DebugTools.DrawRectangle(_spriteBatch, _boat, Color.Red);
            _spriteBatch.End();

            // Draw debug info
            _spriteBatch.Begin();
            DebugTools.DrawObjectInfo(_spriteBatch, _boat.Position, "boatposition", Color.FloralWhite);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
