using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck2.Classes
{
    public class Collider
    {
        public Rectangle Bounds { get; private set; }
        public Color PassiveColor { get; set; }
        public Color ActivatedColor { get; set; }
        public bool IsTrigger { get; set; }

        public bool currentlyCollides;
        public Collider(Rectangle bounds, bool isTrigger)
        {
            Bounds = bounds;
            IsTrigger = isTrigger;
            PassiveColor = IsTrigger ? Color.Green : Color.Pink;
            ActivatedColor = IsTrigger ? Color.Red : Color.Purple;
        }
        public void Collides(){
            currentlyCollides = true;
        }
        public void UpdateBounds(Rectangle newBounds)
        {
            Bounds = newBounds;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Draw(Texture2DHelper.GetWhiteTexture(graphicsDevice), Bounds, PassiveColor * 0.5f);
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, float rotation)
        {
            Vector2 origin = new Vector2(Bounds.Width / 2f, Bounds.Height / 2f);
            Vector2 position = new Vector2(Bounds.X, Bounds.Y);
            Rectangle drawRect = new Rectangle((int)position.X, (int)position.Y, Bounds.Width, Bounds.Height);
            Color currentColor = currentlyCollides ? PassiveColor : ActivatedColor;
            spriteBatch.Draw(Texture2DHelper.GetWhiteTexture(graphicsDevice), position, drawRect, currentColor * 0.3f, rotation, origin, new Vector2(1, 1), SpriteEffects.None, 0f);
            PassiveColor = IsTrigger ? Color.Green : Color.Blue;
            currentlyCollides = false;
        }
    }
}
