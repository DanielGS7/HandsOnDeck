using HandsOnDeck2.Classes.Global;
using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace HandsOnDeck2.Classes.Factory
{
    public class BombFactory : IProjectileFactory
    {
        private readonly ContentManager content;

        public BombFactory(ContentManager content)
        {
            this.content = content;
        }

        public IProjectile CreateProjectile(SeaCoordinate position, Vector2 direction, IGameObject parent)
        {
            return new Bomb(content, position, parent);
        }
    }
}
