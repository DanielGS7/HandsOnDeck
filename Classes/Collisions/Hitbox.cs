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
