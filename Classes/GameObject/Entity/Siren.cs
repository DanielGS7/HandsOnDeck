using HandsOnDeck2.Classes.Global;
using HandsOnDeck2.Classes.Rendering;
using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HandsOnDeck2.Classes.GameObject.Entity
{
    public class Siren : IGameObject
    {
        public VisualElement VisualElement { get; set; }
        public SeaCoordinate Position { get; set; }
        public Vector2 Size { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public float Scale { get; set; }
        private DifficultySettings difficultySettings;
        private const float AttractionRadius = 1000f;

        public Siren(ContentManager content, SeaCoordinate position)
        {
            Position = position;
            Size = new Vector2(30, 30);
            Scale = 1f;
            Origin = Size / 2f;
            Rotation = 0f;

            var texture = content.Load<Texture2D>("Sprites\\siren");
            difficultySettings = DifficultySettings.Instance;
            VisualElement = new VisualElement(texture, Color.White, SpriteEffects.None, 0f);
        }

        public void Update(GameTime gameTime)
        {
            PlayerBoat playerBoat = Map.Instance.player;
            Vector2 directionToSiren = Position.GetShortestDirection(playerBoat.Position);
            float distanceToPlayer = directionToSiren.Length();

            if (distanceToPlayer <= AttractionRadius)
            {
                float attractionFactor = 1 - Math.Min(distanceToPlayer / AttractionRadius, 1);
                float attractionStrength = difficultySettings.GetSirenPullingPower() * attractionFactor;

                float angleToSiren = (float)Math.Atan2(directionToSiren.Y, directionToSiren.X);
                float rotationDifference = MathHelper.WrapAngle(angleToSiren - playerBoat.Rotation);

                playerBoat.ApplySirenEffect(rotationDifference * attractionStrength);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            VisualElement.Draw(spriteBatch, Position, Origin, Scale, Rotation);
        }
    }
}