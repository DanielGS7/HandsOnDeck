using HandsOnDeck.Classes.Animations;
using Microsoft.Xna.Framework;

namespace HandsOnDeck.Classes.Object.Entity
{
    internal class ExplosiveBarrel : CollideableGameObject
    {
        private Animation explosiveBarrel = new Animation("Unstable_Barrel",new Vector2(360,360),0,1,1,0, false);
        private float activationRange = 100f;

        public ExplosiveBarrel(Vector2 startPosition) 
        {
            position = startPosition;
        }

        public override void LoadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {

        }
        public override void Draw(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime, Vector2 position)
        {

        }
    }
}
