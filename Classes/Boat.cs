using HandsOnDeck2.Enums;
using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        public Boat(Texture2D texture, Vector2 startPosition)
        {
            this.Position = startPosition;
            this.rotation = 0f;
            this.Speed = 0f;
            this.anchorDown = false;
            this.sailsOpen = false;
            this.Size = new Vector2(texture.Width, texture.Height);

            VisualElement = new VisualElement(texture, Position, new Vector2(texture.Width / 2, texture.Height / 2), 0.2f, rotation, Color.White, SpriteEffects.None, 0f);
        }

        public Boat(Animation animation, Vector2 startPosition)
        {
            this.Position = startPosition;
            this.rotation = 0f;
            this.Speed = 0f;
            this.anchorDown = false;
            this.sailsOpen = false;
            this.Size = animation.SpriteSize;

            VisualElement = new VisualElement(animation, Position, new Vector2(animation.SpriteSize.X / 2, animation.SpriteSize.Y / 2), 0.2f, rotation, Color.White, SpriteEffects.None, 0f);
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
            // Implement shooting cannonball logic here
        }

        private void ToggleAnchor()
        {
            // Implement anchor toggling logic here
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

            Position += new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * Speed;
            VisualElement.SetPosition(Position);
            VisualElement.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            VisualElement.Draw(spriteBatch);

            // Draw debug rectangle using IEntity method
            DebugTools.DrawRectangle(spriteBatch, this, Color.Red);
        }
    }
}
