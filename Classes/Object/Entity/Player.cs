using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandsOnDeck.Classes.Animations;
using HandsOnDeck.Classes.Collisions;
using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Enums;
using HandsOnDeck.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck.Classes.Object.Entity
{
    public class Player : IEntity
    {
        private const float MaxSpeed = 10f; 
        private const float AccelerationRate = 0.1f;
        private const float DecelerationRate = 0.2f;

        private Animation boatSprite;
        private Vector2 position;
        private Vector2 velocity;
        private float rotation;
        private bool sailsUp;

        Hitbox ICollideable.Hitbox { get; set; }
        HitboxType ICollideable.type { get; set; }

        public Player()
        {
            boatSprite = new Animation("image1", new Vector2(672, 242), 0, 1, 1, 0, false);
            position = new Vector2(500,500);
            rotation = 0f;
            sailsUp = false;

            ((ICollideable)this).Hitbox = new Hitbox(new Rectangle((int)position.X,(int)position.Y, 672, 242));
            ((ICollideable)this).type = HitboxType.Physical;

            InputManager.GetInstance.Initialize();
        }

        public void LoadContent()
        {
            boatSprite.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            HandleInput();

            // Update boat position and rotation based on velocity
            position += velocity;
            rotation = MathHelper.WrapAngle(rotation + velocity.X * 0.01f);

            // Update hitbox position
            ((ICollideable)this).Hitbox.Update(position);

            // Update boat sprite animation
            boatSprite.Update(gameTime);
        }

        public void Draw()
        {
            boatSprite.Draw(position, 1f, rotation);
        }

        private void HandleInput()
        {
            List<GameAction> pressedActions = InputManager.GetInstance.GetPressedActions();

            foreach (var action in pressedActions)
            {
                switch (action)
                {
                    case GameAction.SAILSUP:
                        sailsUp = true;
                        Accelerate();
                        break;
                    case GameAction.SAILSDOWN:
                        sailsUp = false;
                        Decelerate();
                        break;
                    case GameAction.TURNLEFT:
                        TurnLeft();
                        break;
                    case GameAction.TURNRIGHT:
                        TurnRight();
                        break;
                }
            }
        }

        private void Accelerate()
        {
            float acceleration = Sigmoid(velocity.X, MaxSpeed) * AccelerationRate;
            velocity.X += acceleration;
            velocity.X = MathHelper.Clamp(velocity.X, 0f, MaxSpeed);
        }

        private void Decelerate()
        {
            float deceleration = Sigmoid(velocity.X, MaxSpeed) * DecelerationRate;
            velocity.X -= deceleration;
            velocity.X = MathHelper.Clamp(velocity.X, 0f, MaxSpeed);
        }

        private void TurnLeft()
        {
            if (sailsUp)
            {
                rotation -= MathHelper.ToRadians(1f);
            }
        }

        private void TurnRight()
        {
            if (sailsUp)
            {
                rotation += MathHelper.ToRadians(1f);
            }
        }

        private float Sigmoid(float x, float maxSpeed)
        {
            // Sigmoid function for smooth acceleration and deceleration
            return (float)(maxSpeed / (1 + Math.Exp(-x)));
        }

        /*public void HandleCollision(Entity otherEntity)
        {
            if (otherEntity == HitboxType.Physical)
            {
                // Calculate new velocity based on collision response
                Vector2 collisionNormal = Vector2.Normalize(position - otherObject.Position);
                float relativeVelocity = Vector2.Dot(velocity, collisionNormal);

                velocity -= 2 * relativeVelocity * collisionNormal / mass;

                // Adjust rotation away from the collision
                rotation += MathHelper.ToRadians(180f);
            }
        }*/
    }
}
