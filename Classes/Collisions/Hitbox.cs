using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace HandsOnDeck.Classes.Collisions
{
    public class Hitbox
    {
        internal Rectangle bounds { get; set;}

        public Hitbox(Rectangle SourceRectangle)
        {
            bounds = SourceRectangle;
        }

        internal void Update(Vector2 position)
        {
        }
    }
}
