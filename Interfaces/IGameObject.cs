using HandsOnDeck.Classes.Collisions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Interfaces
{
    internal interface IGameObject:ICollideable
    {
        void Update(GameTime gameTime);
        void Draw();
    }
}