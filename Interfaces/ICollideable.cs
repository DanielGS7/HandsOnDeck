﻿using HandsOnDeck.Classes.Collisions;
using HandsOnDeck.Enums;

namespace HandsOnDeck.Interfaces
{
    internal interface ICollideable
    {
        internal Hitbox Hitbox { get; set;}
        internal HitboxType Type { get; set; }
    }
}
