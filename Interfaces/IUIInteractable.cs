using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck2.Interfaces
{
    public interface IUIInteractable: IUIElement
    {
        void HandleInput(MouseState mouseState);
        
    }
}
