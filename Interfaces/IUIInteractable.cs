using Microsoft.Xna.Framework.Input;

namespace HandsOnDeck2.Interfaces
{
    public interface IUIInteractable: IUIElement
    {
        void HandleInput(MouseState mouseState);
        
    }
}
