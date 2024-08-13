using HandsOnDeck2.Classes.Global;
using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

public interface IProjectileFactory
{
    IProjectile CreateProjectile(SeaCoordinate position, Vector2 direction, IGameObject parent);
}

public class CannonballFactory : IProjectileFactory
{
    private ContentManager content;

    public CannonballFactory(ContentManager content)
    {
        this.content = content;
    }

    public IProjectile CreateProjectile(SeaCoordinate position, Vector2 direction, IGameObject parent)
    {
        return new Cannonball(content, position, direction, parent);
    }
}

public class BombFactory : IProjectileFactory
{
    private ContentManager content;

    public BombFactory(ContentManager content)
    {
        this.content = content;
    }

    public IProjectile CreateProjectile(SeaCoordinate position, Vector2 direction, IGameObject parent)
    {
        return new Bomb(content, position, parent);
    }
}