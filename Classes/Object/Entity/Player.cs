﻿using Microsoft.Xna.Framework;
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

namespace HandsOnDeck.Classes.Object.Entity
{
    public class Player : CollidableGameObject, IEntity
    {
        private static Player instance;

        private Animation boatSprite;
        public Vector2 position;
        public float rotation;
        public float speed;
        private float maxSpeed = 3.0f;
        private bool sailsUp = false;
        private bool toggleSailReleased = true;
        private float accelerationRate = 0.02f;
        private float decelerationRate = 0.03f;
        private float turnSpeedCoefficient = 0.5f;
        CannonBalls cannonBalls= new CannonBalls();

        private Player()
        {
            position = new Vector2(500,500);
            boatSprite = new Animation("image1", new Vector2(672, 242), 0, 1, 1, 0, false);
            rotation = 0.0f;
            speed = 0.0f;
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
            boatSprite.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput();
            UpdateMovement(gameTime);
            cannonBalls.Update(gameTime);
            boatSprite.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            boatSprite.Draw(position, 0.2f, rotation, new Vector2(336, 121));
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

            if (actions.Contains(GameAction.SHOOTLEFT))
            {
                Vector2 leftDirection = new Vector2((float)Math.Cos(rotation - MathHelper.PiOver2), (float)Math.Sin(rotation - MathHelper.PiOver2));
                ShootCannonBall(leftDirection);
            }

            if (actions.Contains(GameAction.SHOOTRIGHT))
            {
                Vector2 rightDirection = new Vector2((float)Math.Cos(rotation + MathHelper.PiOver2), (float)Math.Sin(rotation + MathHelper.PiOver2));
                ShootCannonBall(rightDirection);
            }
        }

        private void ShootCannonBall(Vector2 direction)
        {
            cannonBalls.AddCannonball(position, direction * (speed + CannonBall.BaseSpeed));
            
            
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
