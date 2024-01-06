using HandsOnDeck.Classes.Animations;
using HandsOnDeck.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Classes.Object.Entity
{
    internal class CannonBall : CollideableGameObject, IEntity
    {
        private Animation cannonBall;
        public Vector2 velocity { get; private set; }
        public Vector2  position { get; set; }
        public static float BaseSpeed = 5.0f;
        public CannonBall(Vector2 initialPosition, Vector2 initialVelocity)
        {
            cannonBall = new Animation("cannonball",new Vector2(287,175),0,1,1,0,false);
            position = initialPosition;
            velocity = initialVelocity;
        }

        public override void LoadContent()
        {
            cannonBall.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;
            cannonBall.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            cannonBall.Draw(position,0.2f,0,new Vector2(96,58));
        }
    }
}
