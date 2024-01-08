using HandsOnDeck.Enums;
using Microsoft.Xna.Framework;

namespace HandsOnDeck.Classes.Collisions
{
    public class Hitbox
    {
        internal Rectangle bounds { get; set;}
        internal HitboxType type { get; set; }
        public Hitbox(Rectangle SourceRectangle, HitboxType hitboxType)
        {
            bounds = SourceRectangle;
            type = hitboxType;
        }
    }
}
