using HandsOnDeck2.Classes.Global;
using Microsoft.Xna.Framework;

namespace HandsOnDeck2.Interfaces
{
    public interface IProjectileFactory
    {
        IProjectile CreateProjectile(SeaCoordinate position, Vector2 direction, IGameObject parent);
    }
}