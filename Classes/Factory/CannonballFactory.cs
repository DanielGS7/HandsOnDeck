using HandsOnDeck2.Classes.Global;
using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using HandsOnDeck2.Classes.GameObject.Entity;

namespace HandsOnDeck2.Classes.Factory
{
    public class CannonballFactory : IProjectileFactory
    {
        private readonly ContentManager content;

        public CannonballFactory(ContentManager content)
        {
            this.content = content;
        }

        public IProjectile CreateProjectile(SeaCoordinate position, Vector2 direction, IGameObject parent)
        {
            return new Cannonball(content, position, direction, parent);
        }
    }
}
