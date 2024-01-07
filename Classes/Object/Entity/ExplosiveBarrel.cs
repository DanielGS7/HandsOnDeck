using HandsOnDeck.Classes.Animations;
using Microsoft.Xna.Framework;

namespace HandsOnDeck.Classes.Object.Entity
{
    internal class ExplosiveBarrel : CollideableGameObject
    {
        private Animation explosiveBarrel = new Animation("Unstable_Barrel",new Vector2(360,360),0,1,1,0, false);
        private float activationRange = 300f;
        private bool detectedByPlayer = false;

        public ExplosiveBarrel(Vector2 startPosition) 
        {
            position = startPosition;
        }

        public override void LoadContent()
        {
            explosiveBarrel.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            explosiveBarrel.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            
            
        }

        public override void Draw(GameTime gameTime, Vector2 position)
        {
            float distanceToPlayer = Vector2.Distance(Player.GetInstance.position, position);
            if (distanceToPlayer < activationRange)
            {
                detectedByPlayer |= true;
            }
            if (detectedByPlayer)
            {
                explosiveBarrel.Draw(position, 0.2f, 0, new Vector2(180, 180));
            }
        }

        public override void onCollision(CollideableGameObject other)
        {
            throw new System.NotImplementedException();
        }
    }
}
