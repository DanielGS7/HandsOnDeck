using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Classes.UI;
using HandsOnDeck.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HandsOnDeck.Classes.Object.Entity
{
    internal class CannonBallFactory: List<CannonBall>
    {
        public void AddCannonball(WorldCoordinate initialPosition, Vector2 initialVelocity) 
        {
            CannonBall cannonBall= new CannonBall(initialPosition, initialVelocity);
            cannonBall.LoadContent();
            CollisionManager.GetInstance.AddCollideableObject(cannonBall);
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
                    CollisionManager.GetInstance.RemoveCollideableObject(cb);
                    this.Remove(cb);
                }
            }
        }

        public void Draw(GameTime gameTime) 
        {
            foreach (CannonBall cb in this) 
            {
                WorldCoordinate drawPosition = GameScreen.GetInstance.viewportManager.GetDrawPosition(cb.position);
                cb.Draw(gameTime, drawPosition);
            }
        }
        internal void Reset()
        {
            this.Clear();
        }
    }
}
