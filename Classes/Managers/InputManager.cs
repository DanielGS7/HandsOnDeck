using HandsOnDeck.Classes.MonogameAccessibility;
using HandsOnDeck.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace HandsOnDeck.Classes.Managers
{
    public class InputManager
    {
        private static InputManager instance;
        private static readonly object lockObject = new object();

        private Dictionary<GameAction, Keys> keyMappings;
        private KeyboardState currentKeyboardState;
        private MouseState currentMouseState;
        private GraphicsDevice graphicsDevice;

        public static InputManager GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                            instance = new InputManager();
                    }
                }
                return instance;
            }
        }

        private InputManager()
        {
            keyMappings = new Dictionary<GameAction, Keys>();
        }

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            AddMapping(GameAction.TOGGLESAILS, Keys.Z);
            AddMapping(GameAction.TURNLEFT, Keys.Q);
            AddMapping(GameAction.TURNRIGHT, Keys.D);
            AddMapping(GameAction.TOGGLEANCHOR, Keys.LeftShift);
            AddMapping(GameAction.SELECT, Keys.Space);
            AddMapping(GameAction.PAUSE, Keys.Escape);
            AddMapping(GameAction.SHOOTLEFT, Keys.Left);
            AddMapping(GameAction.SHOOTRIGHT, Keys.Right);
            AddMapping(GameAction.RELOAD, Keys.A);
            AddMapping(GameAction.REPAIR, Keys.H);
        }

        public void Update(){
            currentMouseState = Mouse.GetState();
            currentKeyboardState = Keyboard.GetState();
        }

        public void AddMapping(GameAction action, Keys key)
        {
            if (keyMappings.ContainsKey(action))
            {
                keyMappings[action] = key;
            }
            else
            {
                keyMappings.Add(action, key);
            }
        }

        public void RemoveMapping(GameAction action)
        {
            if (keyMappings.ContainsKey(action))
            {
                keyMappings.Remove(action);
            }
            else
            {
                throw new ArgumentException($"Action '{action}' not found");
            }
        }

        public List<GameAction> GetPressedActions()
        {
            List<GameAction> pressedActions = new List<GameAction>();

            foreach (var mapping in keyMappings)
            {
                if (currentKeyboardState.IsKeyDown(mapping.Value))
                {
                    pressedActions.Add(mapping.Key);
                }
            }

            return pressedActions;
        }
        public Point GetTransformedMousePosition()
        {
            Rectangle dst = Renderer.GetInstance.CalculateDestinationRectangle(Renderer.Window.ClientBounds.Width / (float)Renderer.Window.ClientBounds.Height, 
                                                                            Game1.ProgramWidth / (float)Game1.ProgramHeight);

            float scaleX = dst.Width / (float)Game1.ProgramWidth;
            float scaleY = dst.Height / (float)Game1.ProgramHeight;

            int transformedX = (int)((currentMouseState.X - dst.X) / scaleX);
            int transformedY = (int)((currentMouseState.Y - dst.Y) / scaleY);

            return new Point(transformedX, transformedY);
        }
        /*public void Draw()
        {
            String pos = GetTransformedMousePosition().ToVector2().ToString()+" "+graphicsDevice.Viewport.ToString();
            SpriteBatchManager.Instance.DrawString(Renderer.DefaultFont, pos , GetTransformedMousePosition().ToVector2(), Color.White,0,Vector2.Zero,3,SpriteEffects.None,0);
        }*/
    }
}
