namespace HandsOnDeck2.Interfaces
{
    public interface ICollideable : IGameObject
    {
        bool IsColliding { get; set; }
        void OnCollision(ICollideable other);
    }
}