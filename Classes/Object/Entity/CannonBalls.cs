using Microsoft.Xna.Framework;
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
            this.Add(new CannonBall(initialPosition, initialVelocity));
        }

        public void loadContent() 
        {
            foreach (CannonBall cb in this) 
            {
                cb.LoadContent();
            }
        }
        public void Update(GameTime gameTime) 
        {
            foreach (CannonBall cb in this) 
            {
                cb.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime) 
        {
            foreach(CannonBall cb in this) 
            {
                cb.Draw(gameTime);
            }
        }
    }
}
