using System;
using HandsOnDeck2.Classes.GameObject.Entity;
using HandsOnDeck2.Classes.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck2.Classes.UI
{
    public class CannonStatus : UIElement
    {
        private Texture2D loadedTexture;
        private Texture2D emptyTexture;
        private PlayerBoat playerBoat;
        private bool isLeftCannon;

        public CannonStatus(ContentManager content, Vector2 positionPercentage, Vector2 sizePercentage, PlayerBoat playerBoat, bool isLeftCannon)
            : base(positionPercentage, sizePercentage, 0f)
        {
            loadedTexture = content.Load<Texture2D>("UI\\cannon_loaded");
            emptyTexture = content.Load<Texture2D>("UI\\cannon_empty");
            this.playerBoat = playerBoat;
            this.isLeftCannon = isLeftCannon;

            VisualElement = new VisualElement(loadedTexture, Color.White, SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateVisualElement();
        }

        private void UpdateVisualElement()
        {
            bool isLoaded = isLeftCannon ? playerBoat.IsLeftCannonLoaded : playerBoat.IsRightCannonLoaded;
            VisualElement = new VisualElement(isLoaded ? loadedTexture : emptyTexture, Color.White, SpriteEffects.None, 0f);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D currentTexture = isLeftCannon ? (playerBoat.IsLeftCannonLoaded ? loadedTexture : emptyTexture) 
                                                    : (playerBoat.IsRightCannonLoaded ? loadedTexture : emptyTexture);
            Vector2 origin = new Vector2(currentTexture.Width / 2f, currentTexture.Height / 2f);
            float scale = Math.Min(size.X / currentTexture.Width, size.Y / currentTexture.Height);

            spriteBatch.Draw(currentTexture, position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 0f);
        }
    }
}