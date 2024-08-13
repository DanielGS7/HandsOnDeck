using HandsOnDeck2.Classes.Global;
using HandsOnDeck2.Classes.GameObject.Entity;
using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class PlayerCannonball : Cannonball
{
    public PlayerCannonball(ContentManager content, SeaCoordinate position, Vector2 direction, IGameObject parent)
        : base(content, position, direction, parent)
    {
    }

    public override void OnCollision(ICollideable other)
    {
        if (other != Parent)
        {
            if (other is RivalBoat rivalBoat)
            {
                rivalBoat.TakeDamage();
                IsExpired = true;
            }
            else if (other is Bomber bomber)
            {
                bomber.TakeDamage();
                IsExpired = true;
            }
            else if (other is Bomb)
            {
                IsExpired = true;
            }
        }
    }
}