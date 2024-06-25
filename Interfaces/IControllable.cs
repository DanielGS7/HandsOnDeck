using HandsOnDeck2.Enums;

namespace HandsOnDeck2.Interfaces
{
    public interface IControllable
    {
        void HandleInput(GameAction action);
    }
}
