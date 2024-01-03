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
    internal class Player : IEntity
    {
        private Rectangle spriteSelection;
        private Vector2 size = new Vector2(128, 128);
        private Animation bootSprite = new Animation("image1", new Vector2(672, 242), 0, 1, 1, 0, false);

        
        private Vector2 startPosition = new Vector2(500,500);
        private Vector2 position;
        private Vector2 velocity;
        private float rotation;
        private float acceleration = 0.5f;
        private float deceleration = 0.3f;
        private float maxSpeed = 5f;

        public Player()
        {
            spriteSelection = new Rectangle(0, 0, 180, 247);
            Hitbox = new Hitbox(new Rectangle(0, 0, 128, 128));
            type = HitboxType.Physical;
            position = startPosition;
            velocity = Vector2.Zero;
            rotation = 0f;
        }

        public Hitbox Hitbox { get; set; }
        public HitboxType type { get; set; }

        public void LoadContent()
        {
            bootSprite.LoadContent();
        }

        public void Draw()
        {
            GraphicsDevice _graphics = GraphicsDeviceSingleton.Instance;
            float scale = 0.2f;
            bootSprite.Draw(position, scale, rotation);
        }

        public void Update(GameTime gameTime)
        {
            HandleMovement();
            bootSprite.Update(gameTime);
        }
        private void HandleMovement()
        {
            
            List<GameAction> pressedActions = InputManager.GetInstance.GetPressedActions();
            Debug.WriteLine(pressedActions.Count);
            bool isMoving = false;

            foreach (var action in pressedActions)
            {
                Debug.WriteLine("ping");
                switch (action)
                {
                    case GameAction.SAILSUP:
                        
                        velocity += Vector2.Normalize(new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation))) * acceleration;
                        isMoving = true;
                        break;

                    case GameAction.SAILSDOWN:
                        
                        velocity -= velocity * deceleration;
                        break;

                    case GameAction.TURNLEFT:
                        
                        rotation -= 0.05f; 
                        break;

                    case GameAction.TURNRIGHT:
                        
                        rotation += 0.05f; 
                        break;
                }
            }

            
            if (isMoving)
            {
                
                position += velocity;

                
                if (velocity.Length() > maxSpeed)
                {
                    velocity = Vector2.Normalize(velocity) * maxSpeed;
                }
            }
        }
    }
}
