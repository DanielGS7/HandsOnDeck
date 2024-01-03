using HandsOnDeck.Classes.Object;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Classes.UI
{
    internal abstract class UIInteractable : GameObject
    {
        public new abstract void Update(GameTime gameTime);
        public new abstract void Draw();
        
    }
}
