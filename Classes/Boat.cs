using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck2.Interfaces;
using System;
using HandsOnDeck2.Enums;
using HandsOnDeck2.Classes.Collisions;
using System.Diagnostics;

namespace HandsOnDeck2.Classes
{
    public class Boat : IEntity, ICollideable
    {
        public Vector2 Size { get; set; }
        public float Speed { get; set; }
        public VisualElement VisualElement { get; set; }

        public Collider Collider { get => collider; set => collider = value; }
        public Vector2 Origin { get; set; }
        public float Scale { get; set; }
        public float Rotation { get => rotation; set => rotation = value; }
        public Vector2 Position { get; set; }

        private float rotation;
        private bool anchorDown;
        private bool sailsOpen;
        private Collider collider;
        private Vector2 velocity;

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
            this.Scale = 0.2f;
            this.Origin = new Vector2(boatAnimation.SpriteSize.X / 2, boatAnimation.SpriteSize.Y / 2);
            VisualElement = new VisualElement(boatAnimation, Color.White, SpriteEffects.None, 0f);
            collider = new Collider(new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), false);
            CollisionManager.Instance.AddCollideable(this);
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
        }

        private void ShootCannonball(Vector2 direction)
        {
            //TODO: Implement cannonball entity and shooting logic
        }

        private void ToggleAnchor()
        {
            // Implement anchor logic
        }

        public void Update(GameTime gameTime)
        {
            var previousPosition = Position;

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

            velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * Speed;
            Position += velocity;

            bool hasTeleported = false;
            if (Position.X < 0)
            {
                Position = new Vector2(Position.X + Map.MapWidth, Position.Y);
                hasTeleported = true;
            }
            else if (Position.X >= Map.MapWidth)
            {
                Position = new Vector2(Position.X - Map.MapWidth, Position.Y);
                hasTeleported = true;
            }

            if (Position.Y < 0)
            {
                Position = new Vector2(Position.X, Position.Y + Map.MapHeight);
                hasTeleported = true;
            }
            else if (Position.Y >= Map.MapHeight)
            {
                Position = new Vector2(Position.X, Position.Y - Map.MapHeight);
                hasTeleported = true;
            }

            if (hasTeleported)
            {
                Map.Instance.Camera.AdjustPositionForTeleport(previousPosition, Position);
            }
            VisualElement.Update(gameTime);
            collider.UpdateBounds(new Rectangle((int)Position.X, (int)Position.Y, (int)(Size.X * Scale), (int)(Size.Y * Scale)));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            VisualElement.Draw(spriteBatch, Position, Origin, Scale, Rotation);
            //Debug.WriteIf(Position.X != 400,Position);
        }

        public void OnCollision(ICollideable other)
        {

        }

        public void OnTriggerEnter(ICollideable other)
        {

        }

        public void OnTriggerExit(ICollideable other)
        {

        }
    }
}
