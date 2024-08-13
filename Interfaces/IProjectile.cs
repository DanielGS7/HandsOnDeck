using Microsoft.Xna.Framework;
using HandsOnDeck2.Interfaces;
using HandsOnDeck2.Classes.Global;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public interface IProjectile : IGameObject, ICollideable
{
    bool IsExpired { get; }
    IGameObject Parent { get; }
}