using HandsOnDeck2.Classes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck2.Interfaces
{
    internal interface IUIelement
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        VisualElement VisualElement { get; set; }
    }
}
