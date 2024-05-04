using Microsoft.Xna.Framework;
using HandsOnDeck.Classes.Animations;
using HandsOnDeck.Enums;
using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Interfaces;
using System.Collections.Generic;
using System;
using HandsOnDeck.Classes.UI;
using System.Diagnostics;
using HandsOnDeck.Classes.Collisions;

namespace HandsOnDeck.Classes.Object.Entity
{
    public class Player : CollideableGameObject, IEntity
    {
        private static Player instance;
        private static readonly object lockObject = new object();
        public int lifePoints = 5;
        public float rotation;
        public float speed;
        private float maxSpeed = 3.0f;
        private bool sailsUp = false;
        private bool toggleSailReleased = true;
        private float accelerationRate = 0.02f;
        private float decelerationRate = 0.03f;
        private float turnSpeedCoefficient = 0.5f;
        CannonBallFactory cannonBalls= new CannonBallFactory();
        bool canShoot = true;
        private float shotCooldown = 1.0f;
        private float currentCooldown = 0.0f;
        private Animation moving;
        private Animation stationary;

        private float targetRotation;
        private float rotationSpeed = MathHelper.PiOver4;
        private bool isAvoiding = false;
        private float rotationTimer = 0.0f;
        private float rotationDuration = 2.5f;

        private Player()
        {
            size = new Vector2(672, 243);
            position = new Vector2(Game1.ProgramWidth/2,Game1.ProgramHeight/2);
            _gameObjectTextureName = "movingBoat";
            Hitbox = new Hitbox(new Rectangle(position.ToPoint(), size.ToPoint()/new Point(5,5)), HitboxType.Trigger);
            moving = new Animation(_gameObjectTextureName,size, 0, 6, 5, 1, true);
            stationary = new Animation(_gameObjectTextureName, size, 5, 6, 6, 0, false);
            rotation = 0.0f;
            speed = 0.0f;
        }

        public static Player GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                            instance = new Player();
                    }
                }
                return instance;
            }
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
                Hitbox.bounds = new Rectangle(position.ToPoint(), size.ToPoint());
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

        private void UpdateMovement(GameTime gameTime)
        {
            if (isAvoiding)
            {
                if (isAvoiding)
                {
                    float rotationAmount = rotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    rotation += rotationAmount;

                    rotationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (rotationTimer >= rotationDuration)
                    {
                        isAvoiding = false; 
                        sailsUp = false;    
                    }
                }
            }
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

        public override void Reset()
        {
            position = new Vector2(500, 500);
            rotation = 0.0f;
            speed = 0.0f;
            sailsUp = false;
            toggleSailReleased = true;
            cannonBalls.Reset();
        }

        public override void onCollision(CollideableGameObject other)
        {
            if (other.Hitbox.type == HitboxType.Physical)
            {
                AvoidIsland(other);
            }


            switch (other.GetType().ToString())
            {
                case "HandsOnDeck.Classes.Object.Entity.CannonBall":
                    {
                        TakeDamage();
                        break;
                    }
                case "HandsOnDeck.Classes.Object.Entity.ExplosiveBarrel":
                    {
                        TakeDamage();
                        TakeDamage();
                        break;
                    }
                case "HandsOnDeck.Classes.Object.Entity.KamikazeBoat":
                    {
                        TakeDamage();
                        TakeDamage();
                        break;
                    }
            }
        }


        public void AvoidIsland(GameObject other)
        {
            if (!isAvoiding)
            {
                Vector2 directionToOther = other.position - position;

                float angleToOther = (float)Math.Atan2(directionToOther.Y, directionToOther.X);

                targetRotation = angleToOther - MathHelper.PiOver2;

                if (targetRotation < 0)
                    targetRotation += MathHelper.TwoPi;

                isAvoiding = true;

                rotationTimer = 0.0f;
            }
        }

        public void TakeDamage()
        {
            if (lifePoints > 0)
            {
                lifePoints--;
            }
            Debug.WriteLine(lifePoints);

            if(lifePoints<=0)
            {
                
                GameStateManager.GetInstance.ChangeState(GameState.GameOver);
            }
        }

        public void ResetLives()
        {
            lifePoints = 5;
        }
    }
}