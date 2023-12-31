﻿using HandsOnDeck.Classes.Animations;
using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Classes.UI;
using HandsOnDeck.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Classes.Object.Entity
{
    public class EnemyBoat:GameObject
    {
        private Animation boatSprite;
        private float rotation;
        private float speed;
        private readonly float maxSpeed = 2.0f;
        private readonly float accelerationRate = 0.02f;
        private readonly float decelerationRate = 0.03f;
        private readonly float turnSpeedCoefficient = 0.2f;
        private readonly Random random;
        private const float EncircleDistanceMin = 100f;
        private const float EncircleDistanceMax = 200f;

        public EnemyBoat(Vector2 startPosition)
        {
            position = startPosition;
            boatSprite = new Animation("PirateShip", new Vector2(1195, 706), 0, 1, 1, 0, false);
            rotation = 0.0f;
            speed = 0.0f;
            random = new Random();
            _gameObjectTextureName = "enemy";
        }

        public override void LoadContent()
        {
            boatSprite.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            UpdateMovement(gameTime, Player.GetInstance());
            boatSprite.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Draw(gameTime, position);
        }
        public override void Draw(GameTime gameTime, Vector2 position)
        {
            boatSprite.Draw(position, 0.2f, rotation, new Vector2(597, 353));
        }

        private void UpdateMovement(GameTime gameTime, Player player)
        {
            Vector2 directDirectionToPlayer = player.position - position;
            Vector2 wrappedDirectionToPlayer = directDirectionToPlayer;

            if (Math.Abs(directDirectionToPlayer.X) > GameScreen.WorldSize.X / 2)
            {
                wrappedDirectionToPlayer.X -= Math.Sign(directDirectionToPlayer.X) * GameScreen.WorldSize.X;
            }
            if (Math.Abs(directDirectionToPlayer.Y) > GameScreen.WorldSize.Y / 2)
            {
                wrappedDirectionToPlayer.Y -= Math.Sign(directDirectionToPlayer.Y) * GameScreen.WorldSize.Y;
            }
            Vector2 directionToPlayer = (wrappedDirectionToPlayer.LengthSquared() < directDirectionToPlayer.LengthSquared()) ?
                                        wrappedDirectionToPlayer : directDirectionToPlayer;

            float distanceToPlayer = directionToPlayer.Length();
            float noise = ((float)random.NextDouble() - 0.5f) * 0.02f;
            float targetRotation = (float)Math.Atan2(directionToPlayer.Y, directionToPlayer.X);


            if (distanceToPlayer > EncircleDistanceMax || distanceToPlayer < EncircleDistanceMin)
            {
                
                rotation = TurnTowards(rotation, targetRotation + noise, turnSpeedCoefficient);

                
                float targetSpeed = (distanceToPlayer < EncircleDistanceMin) ? -maxSpeed / 3 : maxSpeed;
                speed = AdjustSpeed(speed, targetSpeed);
            }
            else
            {
                
                rotation = TurnTowards(rotation, targetRotation + MathHelper.PiOver2 + noise, turnSpeedCoefficient); 
                speed = AdjustSpeed(speed, maxSpeed / 2); 
            }

            
            Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            position += direction * speed;
            position.X = (position.X + GameScreen.WorldSize.X) % GameScreen.WorldSize.X;
            position.Y = (position.Y + GameScreen.WorldSize.Y) % GameScreen.WorldSize.Y;
        }

        private float TurnTowards(float currentRotation, float targetRotation, float turnSpeed)
        {
            
            float deltaRotation = MathHelper.WrapAngle(targetRotation - currentRotation);
            return currentRotation + MathHelper.Clamp(deltaRotation, -turnSpeed, turnSpeed);
        }

        private float AdjustSpeed(float currentSpeed, float targetSpeed)
        {
            return currentSpeed < targetSpeed
                ? Math.Min(currentSpeed + accelerationRate, targetSpeed)
                : Math.Max(currentSpeed - decelerationRate, targetSpeed);
        }
    }
}

