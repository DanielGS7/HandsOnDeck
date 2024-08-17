using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck2.Classes.UI
{
    public class WaterGauge : UIElement
    {
        private Texture2D gaugeTexture;
        private float waterLevel;
        private Color waterColor = new Color(0, 0, 255, 128);

        public WaterGauge(ContentManager content, Vector2 positionPercentage, Vector2 sizePercentage) 
            : base(positionPercentage, sizePercentage, 0f)
        {
            gaugeTexture = content.Load<Texture2D>("water_gauge");
            waterLevel = 0f;
        }

        public void SetWaterLevel(float level)
        {
            waterLevel = MathHelper.Clamp(level, 0f, 1f);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(gaugeTexture.Width / 2f, gaugeTexture.Height / 2f);
            float scale = Math.Min(size.X / gaugeTexture.Width, size.Y / gaugeTexture.Height);

            spriteBatch.Draw(gaugeTexture, position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 0f);

            float waterWidth = size.X * 0.1f;
            Rectangle waterRect = new Rectangle(
                (int)(position.X - (waterWidth / 2)),
                (int)(position.Y + (size.Y / 2) - (size.Y * waterLevel)),
                (int)waterWidth,
                (int)(size.Y * waterLevel)
            );

            spriteBatch.Draw(gaugeTexture, waterRect, new Rectangle(0, 0, 1, 1), waterColor);
        }
    }
}