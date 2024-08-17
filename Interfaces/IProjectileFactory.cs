using HandsOnDeck2.Classes.Global;
using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

public interface IProjectileFactory
{
    IProjectile CreateProjectile(SeaCoordinate position, Vector2 direction, IGameObject parent);
}