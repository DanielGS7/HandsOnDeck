using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck2.Interfaces;
using System;
using HandsOnDeck2.Enums;
using HandsOnDeck2.Classes.Collision;
using HandsOnDeck2.Classes.Rendering;
using HandsOnDeck2.Classes.Global;

namespace HandsOnDeck2.Classes.GameObject.Entity
{
    public class Boat : IEntity, ICollideable, IControllable
    {
        public Vector2 Size { get; set; }
        public float Speed { get; set; }
        public VisualElement VisualElement { get; set; }
        public Vector2 Origin { get; set; }
        public float Scale { get; set; }
        public float Rotation { get => rotation; set => rotation = value; }
        public SeaCoordinate Position { get; set; }
        public bool IsColliding { get; set; }

        private float rotation;
        private bool anchorDown;
        private bool sailsOpen;

        public Boat(ContentManager content, SeaCoordinate startPosition)
        {
            var boatTexture = content.Load<Texture2D>("boat");
            var boatAnimation = new Animation("movingBoat", new Vector2(670, 243), 5, 5, 4f, true);
            boatAnimation.LoadContent(content);
            Position = startPosition;
            rotation = 0f;
            Speed = 0f;
            anchorDown = false;
            sailsOpen = false;
            Size = new Vector2(670, 243);
            Scale = 0.2f;
            Origin = new Vector2(Size.X / 2, Size.Y / 2);
            VisualElement = new VisualElement(boatAnimation, Color.White, SpriteEffects.None, 0f);
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
                case GameAction.ShootLeft:
                    // Implement shooting left
                    break;
                case GameAction.ShootRight:
                    // Implement shooting right
                    break;
                case GameAction.ToggleAnchor:
                    anchorDown = !anchorDown;
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
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

            Vector2 movement = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * Speed;
            Position = new SeaCoordinate(Position.X + movement.X, Position.Y + movement.Y);

            VisualElement.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            VisualElement.Draw(spriteBatch, Position, Origin, Scale, Rotation);
        }

        public void OnCollision(ICollideable other)
        {
        }
    }
}