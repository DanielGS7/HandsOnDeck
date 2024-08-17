using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck2.Interfaces;
using System;
using HandsOnDeck2.Enums;
using HandsOnDeck2.Classes.Collision;
using HandsOnDeck2.Classes.Rendering;
using HandsOnDeck2.Classes.Global;
using HandsOnDeck2.Classes.Sound;
using HandsOnDeck2.Classes.UI.Screens;

namespace HandsOnDeck2.Classes.GameObject.Entity
{
    public class PlayerBoat : IEntity, ICollideable, IControllable
    {
        internal const float maxSpeed = 3f;
        internal const float rotationSpeed = 3f;
        public Vector2 Size { get; set; }
        public float Speed { get; set; }
        public VisualElement VisualElement { get; set; }
        public Vector2 Origin { get; set; }
        public float Scale { get; set; }
        public float Rotation { get => rotation; set => rotation = value; }
        public SeaCoordinate Position { get; set; }
        public bool IsColliding { get; set; }
        public bool IsLeftCannonLoaded { get; private set; }
        public bool IsRightCannonLoaded { get; private set; }
        public bool IsInvincible => invincibilityTimer > 0;
        public event Action OnDamageTaken;

        private Vector2 externalForce;
        private float sirenEffect = 0f;
        private const float SirenEffectDecay = 0.98f;

        private float rotation;
        private bool anchorDown;
        private bool sailsOpen;
        private float invincibilityTimer = 0f;
        private float invincibilityDuration;
        private bool leftCannonLoaded = true;
        private bool rightCannonLoaded = true;
        private float reloadTimer;
        private const float ReloadTime = 2f;

        private IProjectileFactory playerCannonballFactory;

        public PlayerBoat(ContentManager content, SeaCoordinate startPosition)
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
            invincibilityDuration = DifficultySettings.Instance.GetInvincibilityDuration();
            IsLeftCannonLoaded = true;
            IsRightCannonLoaded = true;
            reloadTimer = 0f;

            playerCannonballFactory = new PlayerCannonballFactory(content);
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
                    rotation -= rotationSpeed / 100;
                    break;
                case GameAction.SteerRight:
                    rotation += rotationSpeed / 100;
                    break;
                case GameAction.ShootLeft:
                        ShootCannon(true);
                        IsLeftCannonLoaded = false;
                    break;
                case GameAction.ShootRight:
                        ShootCannon(false);
                        IsRightCannonLoaded = false;
                    break;
                case GameAction.ToggleAnchor:
                    anchorDown = !anchorDown;
                    break;
                case GameAction.Reload:
                    StartReload();
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (anchorDown)
            {
                Speed = 0f;
            }
            else if (sailsOpen)
            {
                Speed = MathHelper.Clamp(Speed + 0.01f, 0f, maxSpeed);
            }
            else
            {
                Speed = MathHelper.Clamp(Speed - 0.01f, 0f, maxSpeed);
            }

            rotation += sirenEffect * deltaTime;
            rotation = MathHelper.WrapAngle(rotation);
            sirenEffect *= (float)Math.Pow(SirenEffectDecay, deltaTime * 60);

            Vector2 movement = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * Speed;
            Position = new SeaCoordinate(Position.X + movement.X, Position.Y + movement.Y);

            VisualElement.Update(gameTime);

            if (reloadTimer > 0)
            {
                reloadTimer -= deltaTime;
                if (reloadTimer <= 0)
                {
                    reloadTimer = 0;
                    leftCannonLoaded = true;
                    rightCannonLoaded = true;
                }
            }
            if (invincibilityTimer > 0)
            {
                invincibilityTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (invincibilityTimer <= 0)
                {
                    invincibilityTimer = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            VisualElement.Draw(spriteBatch, Position, Origin, Scale, Rotation);
        }

        public void ApplyExternalForce(Vector2 force)
        {
            externalForce += force;
        }

        public void ApplySirenEffect(float rotationInfluence)
        {
            sirenEffect -= rotationInfluence;
        }

        public bool TakeDamage()
        {
            if (!IsInvincible)
            {
                invincibilityTimer = invincibilityDuration;
                OnDamageTaken?.Invoke();
                AudioManager.Instance.Play("damage");
                return true;
            }
            return false;
        }     
        public void OnCollision(ICollideable other)
        {
            if (other is Island)
            {
                Speed = MathHelper.Clamp(Speed - 0.05f, 1f, 5f);

                Vector2 directionAway = Position.ToVector2() - other.Position.ToVector2();
                directionAway.Normalize();

                float angleAway = (float)Math.Atan2(directionAway.Y, directionAway.X);

                float angleDifference = MathHelper.WrapAngle(angleAway - rotation);

                float maxTurnAngle = 0.01f; 

                float clampedAngleDifference = MathHelper.Clamp(angleDifference, -maxTurnAngle, maxTurnAngle);

                rotation += clampedAngleDifference;

                rotation = MathHelper.WrapAngle(rotation);
            }
            else if (other is Enemy)
            {
                Vector2 directionAway = Position.ToVector2() - other.Position.ToVector2();
                directionAway.Normalize();

                float angleAway = (float)Math.Atan2(directionAway.Y, directionAway.X);

                float angleDifference = MathHelper.WrapAngle(angleAway - rotation);

                float maxTurnAngle = 0.05f;

                float clampedAngleDifference = MathHelper.Clamp(angleDifference, -maxTurnAngle, maxTurnAngle);

                rotation += clampedAngleDifference;

                rotation = MathHelper.WrapAngle(rotation);
            }
        }

        private void ShootCannon(bool isLeft)
        {
            if ((isLeft && IsLeftCannonLoaded) || (!isLeft && IsRightCannonLoaded))
            {
                Vector2 forwardDirection = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                
                Vector2 perpendicularDirection = new Vector2(-forwardDirection.Y, forwardDirection.X);
                if (isLeft) perpendicularDirection = -perpendicularDirection;

                Vector2 spawnOffset = perpendicularDirection * (Size.Y / 2) * Scale;
                spawnOffset += forwardDirection * (Size.X / 4) * Scale;
                SeaCoordinate spawnPosition = new SeaCoordinate(Position.X + spawnOffset.X, Position.Y + spawnOffset.Y);

                IProjectile playerCannonball = playerCannonballFactory.CreateProjectile(spawnPosition, perpendicularDirection, this);
                Map.Instance.AddProjectile(playerCannonball);

                if (isLeft)
                    leftCannonLoaded = false;
                else
                    rightCannonLoaded = false;
                
                AudioManager.Instance.Play("cannon_fire", null, 0.5f);
            }
        }
        private void StartReload()
        {
            if ((!leftCannonLoaded || !rightCannonLoaded) && reloadTimer == 0f )
            {
                reloadTimer = ReloadTime;
                AudioManager.Instance.Play("reload");
            }
        }
        
        public void FinishReload()
        {
            IsLeftCannonLoaded = true;
            IsRightCannonLoaded = true;
        }
    }
}