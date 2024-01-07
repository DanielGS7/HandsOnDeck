using HandsOnDeck.Classes.Collisions;
using HandsOnDeck.Enums;
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
    public abstract class GameObject : IGameObject
    {
        public string _gameObjectTextureName;
        public Texture2D _gameObjectTexture;
        public Vector2 position;

        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, Vector2 position);
    }
}
