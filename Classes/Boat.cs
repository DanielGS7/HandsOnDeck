using HandsOnDeck2.Enums;
using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace HandsOnDeck2.Classes
{
    public class Boat : IEntity
    {
        public Vector2 Size { get; set; }
        public float Speed { get; set; }
        public VisualElement VisualElement { get; set; }
        public Vector2 Position { get; set; }
        private float rotation;
        private bool anchorDown;
        private bool sailsOpen;

        public Boat(ContentManager content, Vector2 startPosition)
        {
            var boatTexture = content.Load<Texture2D>("boat");
            var boatAnimation = new Animation("movingBoat", new Vector2(670, 243), 5, 5, 4f, true);
            boatAnimation.LoadContent(content);

            this.Position = startPosition;
            this.rotation = 0f;
            this.Speed = 0f;
            this.anchorDown = false;
            this.sailsOpen = false;
            this.Size = new Vector2(boatTexture.Width, boatTexture.Height);

            VisualElement = new VisualElement(boatAnimation, Position, new Vector2(boatAnimation.SpriteSize.X / 2, boatAnimation.SpriteSize.Y / 2), 0.2f, rotation, Color.White, SpriteEffects.None, 0f);
        }

        public void HandleInput(GameAction action)
        {
            switch (action)
            {
                case GameAction.SailsOpen:
                    sailsOpen = true;
                    break;
                case GameAction.SailsClosed:
                    sailsOpen = false;
                    break;
                case GameAction.SteerLeft:
                    rotation -= 0.1f;
                    break;
                case GameAction.SteerRight:
                    rotation += 0.1f;
                    break;
                case GameAction.ShootRight:
                    ShootCannonball(Vector2.UnitX);
                    break;
                case GameAction.ShootLeft:
                    ShootCannonball(-Vector2.UnitX);
                    break;
                case GameAction.ToggleAnchor:
                    ToggleAnchor();
                    break;
            }
            VisualElement.SetRotation(rotation);
        }

        private void ShootCannonball(Vector2 direction)
        {
            //TODO maak cannonball entity en regel schietlogica
        }

        private void ToggleAnchor()
        {
            // implementeer anker logica
        }

        public void Update(GameTime gameTime)
        {
            var map = Map.Instance;
            int mapWidth = map.MapWidth;
            int mapHeight = map.MapHeight;

            if (anchorDown)
            {
                Speed = 0f;
            }
            else if (sailsOpen)
            {
                Speed = MathHelper.Clamp(Speed + 0.01f, 0f, 5f);
            }
            else
            {
                Speed = MathHelper.Clamp(Speed - 0.01f, 0f, 5f);
            }

            Position += new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * Speed;

            
            if (Position.X < 0)
            {
                Position = new Vector2(Position.X + mapWidth, Position.Y);
            }
            else if (Position.X >= mapWidth)
            {
                Position = new Vector2(Position.X - mapWidth, Position.Y);
            }

            if (Position.Y < 0)
            {
                Position = new Vector2(Position.X, Position.Y + mapHeight);
            }
            else if (Position.Y >= mapHeight)
            {
                Position = new Vector2(Position.X, Position.Y - mapHeight);
            }

            VisualElement.SetPosition(Position);
            VisualElement.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            VisualElement.Draw(spriteBatch);
        }
    }
}
