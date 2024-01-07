using HandsOnDeck.Classes.Animations;
using HandsOnDeck.Classes.Collisions;
using HandsOnDeck.Interfaces;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace HandsOnDeck.Classes.Object.Entity
{
    internal class CannonBall : CollideableGameObject, IEntity
    {
        private Animation cannonBall;
        public Vector2 velocity { get; private set; }
        public static float BaseSpeed = 5.0f;
        public float TimeTillRemoval = 3.0f;
        public float TimeExisting = 0.0f;
        public CannonBall(Vector2 initialPosition, Vector2 initialVelocity)
        {
            position = initialPosition;
            size = new Vector2(600, 562);
            Hitbox = new Hitbox(new Rectangle(position.ToPoint(), size.ToPoint()),Enums.HitboxType.Trigger);
            _gameObjectTextureName = "cannonball1";
            cannonBall = new Animation(_gameObjectTextureName,size,0,1,1,0,false);
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
            Hitbox.bounds = new Rectangle(position.ToPoint(), size.ToPoint());
        }

        public override void Draw(GameTime gameTime)
        {
            Draw(gameTime, position);
        }
        public override void Draw(GameTime gameTime, Vector2 position)
        {
            cannonBall.Draw(position,0.04f,0,new Vector2(300,281));
        }

        public override void onCollision(CollideableGameObject other)
        {
            Debug.WriteLine("Cannonball collided");
        }
    }
}
