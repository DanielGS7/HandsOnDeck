using HandsOnDeck2.Classes.Global;
using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using HandsOnDeck2.Classes.GameObject.Entity.Projectile;

public class PlayerCannonball : Cannonball
{
    private bool hasDamaged = false;
    public PlayerCannonball(ContentManager content, SeaCoordinate position, Vector2 direction, IGameObject parent)
        : base(content, position, direction, parent)
    {
    }

    public override void OnCollision(ICollideable other)
    {
        if (!hasDamaged && other != Parent)
        {
            if (other is RivalBoat rivalBoat)
            {
                rivalBoat.TakeDamage();
                hasDamaged = true;
                IsExpired = true;
            }
            else if (other is Bomber bomber)
            {
                bomber.TakeDamage();
                hasDamaged = true;
                IsExpired = true;
            }
            else if (other is Bomb)
            {
                IsExpired = true;
            }
        }
    }
}