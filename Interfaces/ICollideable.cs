using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandsOnDeck.Classes.Collisions;

namespace HandsOnDeck.Interfaces
{
    internal interface ICollideable
    {
        public Hitbox Hitbox { get; set;}
    }
}
