using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HandsOnDeck.Classes.Animations;
using HandsOnDeck.Enums;
using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Interfaces;
using HandsOnDeck.Classes.Collisions;
using System.Collections.Generic;
using System;
using static System.Windows.Forms.Design.AxImporter;
using HandsOnDeck.Classes.UI;

namespace HandsOnDeck.Classes.Object.Entity
{
    public class Player : CollideableGameObject, IEntity
    {
        private static Player instance;

        private Animation boatSprite;
        public float rotation;
        public float speed;
        private float maxSpeed = 3.0f;
        private bool sailsUp = false;
        private bool toggleSailReleased = true;
        private float accelerationRate = 0.02f;
        private float decelerationRate = 0.03f;
        private float turnSpeedCoefficient = 0.5f;
        CannonBalls cannonBalls= new CannonBalls();
        bool canShoot = true;
        private float shotCooldown = 1.0f;
        private float currentCooldown = 0.0f;
        private Animation moving;
        private Animation stationary;

        private Player()
        {
            moving = new Animation("movingBoat", new Vector2(672, 243), 0, 6, 5, 1, true);
            stationary = new Animation("movingBoat", new Vector2(672, 243), 5, 6, 6, 0, false);
            position = new Vector2(Game1.ProgramWidth/2,Game1.ProgramHeight/2);
            rotation = 0.0f;
            speed = 0.0f;
            _gameObjectTextureName = "player";
        }

        public static Player GetInstance()
        {
            if (instance == null)
            {
                instance = new Player();
            }
            return instance;
        }

        public override void LoadContent()
        {
            
            moving.LoadContent();
            stationary.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput();
            UpdateMovement(gameTime);
            cannonBalls.Update(gameTime);
            
            if(speed > 0) 
            {
               moving.Update(gameTime);
            }
            else 
            {
                stationary.Update(gameTime);
            }
            if (!canShoot)
            {
                currentCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (currentCooldown <= 0)
                {
                    canShoot = true;
                    currentCooldown = 0;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Draw(gameTime, position);
        }

        public override void Draw(GameTime gameTime, Vector2 position)
        {
            if (speed > 0)
            {
                moving.Draw(position, 0.2f, rotation, new Vector2(336, 121));
            }
            else
            {
                stationary.Draw(position, 0.2f, rotation, new Vector2(336, 121));
            }
            cannonBalls.Draw(gameTime);
        }

        private void HandleInput()
        {
            List<GameAction> actions = InputManager.GetInstance.GetPressedActions();
            if (actions.Contains(GameAction.TOGGLESAILS) && toggleSailReleased)
            {
                sailsUp = !sailsUp;
                toggleSailReleased = false;
            }
            else if (!actions.Contains(GameAction.TOGGLESAILS))
            {
                toggleSailReleased = true;
            }

            if (speed > 0)
            {
                if (actions.Contains(GameAction.TURNLEFT))
                    rotation -= 0.01f;
                if (actions.Contains(GameAction.TURNRIGHT))
                    rotation += 0.01f;
            }

            if (actions.Contains(GameAction.SHOOTLEFT) && canShoot)
            {
                Vector2 leftDirection = new Vector2((float)Math.Cos(rotation - MathHelper.PiOver2), (float)Math.Sin(rotation - MathHelper.PiOver2));
                ShootCannonBall(leftDirection);
            }

            if (actions.Contains(GameAction.SHOOTRIGHT) && canShoot)
            {
                Vector2 rightDirection = new Vector2((float)Math.Cos(rotation + MathHelper.PiOver2), (float)Math.Sin(rotation + MathHelper.PiOver2));
                ShootCannonBall(rightDirection);
            }
        }

        private void ShootCannonBall(Vector2 direction)
        {
            cannonBalls.AddCannonball(position, direction * (speed + CannonBall.BaseSpeed));
            canShoot = false;
            currentCooldown = shotCooldown;
            
        }

        private void UpdateMovement(GameTime gameTime)
        {
            float targetSpeed = sailsUp ? maxSpeed : 0.0f;
            if (speed < targetSpeed)
            {
                speed = Math.Min(speed + accelerationRate, targetSpeed);
            }
            else if (speed > targetSpeed)
            {
                speed = Math.Max(speed - decelerationRate, targetSpeed);
            }

            float turnSpeed = speed * turnSpeedCoefficient;

            Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            position += direction * speed;
            position.X = (position.X + GameScreen.WorldSize.X) % GameScreen.WorldSize.X;
            position.Y = (position.Y + GameScreen.WorldSize.Y) % GameScreen.WorldSize.Y;
        }

        public void Reset()
        {
            position = new Vector2(500, 500);
            rotation = 0.0f;
            speed = 0.0f;
            sailsUp = false;
            toggleSailReleased = true;
            cannonBalls.Reset();
        }
    }
}
