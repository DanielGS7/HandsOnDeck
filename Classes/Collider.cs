using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HandsOnDeck2.Classes
{
    public class Collider
    {
        public Rectangle Bounds { get; private set; }
        public Color Color { get; set; }
        public bool IsTrigger { get; set; }

        public Collider(Rectangle bounds, bool isTrigger)
        {
            Bounds = bounds;
            IsTrigger = isTrigger;
        }

        public void UpdateBounds(Rectangle newBounds)
        {
            Bounds = newBounds;
            if(IsTrigger) Color = Color.LightYellow; 
            else Color = Color.Green;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, float rotation)
        {
            Vector2 origin = new Vector2(Bounds.Width / 2f, Bounds.Height / 2f);
            Vector2 position = new Vector2(Bounds.X + origin.X, Bounds.Y + origin.Y);

            spriteBatch.Draw(Texture2DHelper.GetWhiteTexture(graphicsDevice), position, null, Color * 0.5f, rotation, origin, new Vector2(Bounds.Width, Bounds.Height), SpriteEffects.None, 0f);
        }
    }
}
