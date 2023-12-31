﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Classes.Object.Entity
{
    internal class CannonBalls: List<CannonBall>
    {
        public void AddCannonball(Vector2 initialPosition, Vector2 initialVelocity) 
        {
            CannonBall cannonBall= new CannonBall(initialPosition, initialVelocity);
            cannonBall.LoadContent();
            this.Add(cannonBall);
        }

      
        public void Update(GameTime gameTime) 
        {
            foreach (CannonBall cb in this.ToList()) 
            {
                cb.Update(gameTime);
                cb.TimeExisting += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(cb.TimeExisting>= cb.TimeTillRemoval)
                {
                    this.Remove(cb);
                }
            }
        }

        public void Draw(GameTime gameTime) 
        {
            foreach(CannonBall cb in this) 
            {
                cb.Draw(gameTime);
            }
        }

        internal void Reset()
        {
            this.Clear();
        }
    }
}
