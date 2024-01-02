using HandsOnDeck.Classes.Collisions;
using HandsOnDeck.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Classes.Object
{
    internal class GameObject:IGameObject, ICollideable
    {
        public CollisionHandler CollisionHandler { get; set; }
        public Hitbox Hitbox { get; set; }
        public HitboxType type { get ; set ; }

        public string _gameObjectTextureName;
        public Texture2D _gameObjectTexture;

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw()
        {
           
        }

        public void LoadContent()
        {
           
        }
    }
}
