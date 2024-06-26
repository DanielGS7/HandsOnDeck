using HandsOnDeck2.Interfaces;
using HandsOnDeck2.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck2.Classes
{
    internal class Button : IUIInteractable
    {
        public VisualElement VisualElement { get ; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
           
        }

        public void HandleInput(MouseState mouseState)
        {
                
        }

        public void Update(GameTime gameTime)
        {
            
        }

        protected void Draw9Slice(Texture2D texture, Rectangle destination, Rectangle? source = null, int borderSize = 0)
        {
            int textureWidth = source?.Width ?? texture.Width;
            int textureHeight = source?.Height ?? texture.Height;
            int centerWidth = textureWidth - borderSize * 2;
            int centerHeight = textureHeight - borderSize * 2;
            int rightSide = textureWidth - borderSize;
            int bottomSide = textureHeight - borderSize;
            int destCenterWidth = destination.Width - borderSize * 2;
            int destCenterHeight = destination.Height - borderSize * 2;

            Rectangle topLeftSource = new Rectangle(source?.X ?? 0, source?.Y ?? 0, borderSize, borderSize);
            Rectangle topRightSource = new Rectangle((source?.X ?? 0) + rightSide, source?.Y ?? 0, borderSize, borderSize);
            Rectangle bottomLeftSource = new Rectangle(source?.X ?? 0, (source?.Y ?? 0) + bottomSide, borderSize, borderSize);
            Rectangle bottomRightSource = new Rectangle((source?.X ?? 0) + rightSide, (source?.Y ?? 0) + bottomSide, borderSize, borderSize);
            Rectangle leftEdgeSource = new Rectangle(source?.X ?? 0, (source?.Y ?? 0) + borderSize, borderSize, centerHeight);
            Rectangle rightEdgeSource = new Rectangle((source?.X ?? 0) + rightSide, (source?.Y ?? 0) + borderSize, borderSize, centerHeight);
            Rectangle topEdgeSource = new Rectangle((source?.X ?? 0) + borderSize, source?.Y ?? 0, centerWidth, borderSize);
            Rectangle bottomEdgeSource = new Rectangle((source?.X ?? 0) + borderSize, (source?.Y ?? 0) + bottomSide, centerWidth, borderSize);
            Rectangle centerSource = new Rectangle((source?.X ?? 0) + borderSize, (source?.Y ?? 0) + borderSize, centerWidth, centerHeight);

            // corners
            SpriteBatchManager.Instance.Draw(texture, new Rectangle(destination.X, destination.Y, borderSize, borderSize), topLeftSource, Color.White);
            SpriteBatchManager.Instance.Draw(texture, new Rectangle(destination.Right - borderSize, destination.Y, borderSize, borderSize), topRightSource, Color.White);
            SpriteBatchManager.Instance.Draw(texture, new Rectangle(destination.X, destination.Bottom - borderSize, borderSize, borderSize), bottomLeftSource, Color.White);
            SpriteBatchManager.Instance.Draw(texture, new Rectangle(destination.Right - borderSize, destination.Bottom - borderSize, borderSize, borderSize), bottomRightSource, Color.White);

            // edges
            SpriteBatchManager.Instance.Draw(texture, new Rectangle(destination.X + borderSize, destination.Y, destCenterWidth, borderSize), topEdgeSource, Color.White);
            SpriteBatchManager.Instance.Draw(texture, new Rectangle(destination.X + borderSize, destination.Bottom - borderSize, destCenterWidth, borderSize), bottomEdgeSource, Color.White);
            SpriteBatchManager.Instance.Draw(texture, new Rectangle(destination.X, destination.Y + borderSize, borderSize, destCenterHeight), leftEdgeSource, Color.White);
            SpriteBatchManager.Instance.Draw(texture, new Rectangle(destination.Right - borderSize, destination.Y + borderSize, borderSize, destCenterHeight), rightEdgeSource, Color.White);

            // center
            SpriteBatchManager.Instance.Draw(texture, new Rectangle(destination.X + borderSize, destination.Y + borderSize, destCenterWidth, destCenterHeight), centerSource, Color.White);
        }
    }
}
