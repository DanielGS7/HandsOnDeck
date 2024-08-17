using Microsoft.Xna.Framework;

namespace HandsOnDeck2.Classes.Collision
{
    public class Collider
    {
        public Rectangle Bounds { get; private set; }
        public Color PassiveColor { get; set; }
        public Color ActivatedColor { get; set; }
        public bool IsTrigger { get; set; }

        public bool currentlyCollides;
        public Collider(Rectangle bounds, bool isTrigger)
        {
            Bounds = bounds;
            IsTrigger = isTrigger;
            PassiveColor = IsTrigger ? Color.Green : Color.Pink;
            ActivatedColor = IsTrigger ? Color.Red : Color.Purple;
        }
        public void UpdateBounds(Rectangle newBounds)
        {
            Bounds = newBounds;
        }
    }
}
