using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandsOnDeck.Classes.Collisions;
using HandsOnDeck.Enums;

namespace HandsOnDeck.Interfaces
{
    internal interface ICollideable
    {
        internal Hitbox Hitbox { get; set;}
        internal HitboxType type { get; set; }
    }
}
