using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandsOnDeck.Classes.Collisions;

namespace HandsOnDeck.Interfaces
{
    internal interface ICollideable
    {
        CollisionHandler CollisionHandler { get; }
        bool CollidesWith(ICollideable other);
    }
}
