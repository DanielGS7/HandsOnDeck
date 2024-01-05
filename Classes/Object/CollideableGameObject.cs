using HandsOnDeck.Classes.Collisions;
using HandsOnDeck.Enums;
using HandsOnDeck.Interfaces;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Classes.Object
{
    public abstract class CollidableGameObject : GameObject, ICollideable
    {
        public CollisionHandler CollisionHandler { get; set; }
        public Hitbox Hitbox { get; set; }
        public HitboxType Type { get; set; }
    }
}