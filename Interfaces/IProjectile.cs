using HandsOnDeck2.Interfaces;
public interface IProjectile : IGameObject, ICollideable
{
    bool IsExpired { get; }
    IGameObject Parent { get; }
}