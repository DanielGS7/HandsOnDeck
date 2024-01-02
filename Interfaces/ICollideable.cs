﻿using System;
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
        public Hitbox Hitbox { get; set;}
        public HitboxType type { get; set; }
        
        public void HitboxType(HitboxType giventype)
        {
            type = giventype;
        }
    }
}
