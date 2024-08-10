using Microsoft.Xna.Framework;

namespace HandsOnDeck2.Classes
{
    public class AudioListener
    {
        public Vector2 Position { get; set; }

        public AudioListener()
        {
            Position = Vector2.Zero;
        }

        public void Update(Vector2 newPosition)
        {
            Position = newPosition;
        }
    }
}