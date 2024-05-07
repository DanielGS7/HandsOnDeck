using HandsOnDeck.Classes.Animations;
using HandsOnDeck.Classes.Collisions;
using HandsOnDeck.Enums;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace HandsOnDeck.Classes.Object.Entity
{
    internal class ExplosiveBarrel : CollideableGameObject
    {
        private Animation explosiveBarrel;
        private float activationRange = 300f;
        private bool detectedByPlayer = false;

        public ExplosiveBarrel(WorldCoordinate startPosition) 
        {
            position = startPosition;
            size = new Vector2(360, 360);
            _gameObjectTextureName = "Unstable_Barrel";
            Hitbox = new Hitbox(new Rectangle(position.ToPoint(), size.ToPoint()), HitboxType.Trigger);
            explosiveBarrel = new Animation(_gameObjectTextureName, size, 0, 1, 1, 0, false);
        }

        public override void LoadContent()
        {
            explosiveBarrel.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            explosiveBarrel.Update(gameTime);
        }
        public override void Draw(GameTime gameTime) {} //Draw method unnecessary

        public override void Draw(GameTime gameTime, WorldCoordinate position)
        {
            float distanceToPlayer = Vector2.Distance(Player.GetInstance.position.ToVector2(), position.ToVector2());
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
            Debug.WriteLine("Explosivebarrel hit!");
        }
    }
}
