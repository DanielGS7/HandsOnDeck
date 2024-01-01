using HandsOnDeck.Classes.Collisions;
using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Classes
{
    internal class CollisionHandler
    {
        internal List<Hitbox> hitboxes;

        internal CollisionHandler()
        {
            this.hitboxes = new List<Hitbox>();
        }

        internal void AddHitbox(Hitbox hitbox)
        {
            this.hitboxes.Add(hitbox);
        }

        internal bool CheckForCollisions()
        {
            return this.hitboxes.Any(_ => _.Intersects(Hitbox));
        }

        internal void Update(GameTime gameTime)
        {
            CheckForCollisions();
        }
    }
