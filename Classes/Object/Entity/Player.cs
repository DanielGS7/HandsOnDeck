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
    public class Player
    {
        private Animation boatSprite;
        public Vector2 position;
        private float rotation;
        private float speed;
        private float maxSpeed = 3.0f;
        private bool sailsUp = false;
        private bool toggleSailReleased = true;
        private float accelerationRate = 0.02f;
        private float decelerationRate = 0.03f;
        private float turnSpeedCoefficient = 0.1f;

        public Player(Vector2 startPosition)
        {
            position = startPosition;
            boatSprite = new Animation("image1", new Vector2(672, 242), 0, 1, 1, 0, false);
            rotation = 0.0f;
            speed = 0.0f;
        }

        public void LoadContent()
        {
            boatSprite.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            HandleInput();
            UpdateMovement(gameTime);
            boatSprite.Update(gameTime);
        }

        public void Draw()
        {
            boatSprite.Draw(position, 0.2f, rotation, new Vector2(336, 121));
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

        private float SigmoidInterpolation(float current, float target, double time)
        {
            float delta = target - current;
            float rate = 0.1f;
            return current + delta * (float)(1 / (1 + Math.Exp(-rate * time)));
        }
    }
}
