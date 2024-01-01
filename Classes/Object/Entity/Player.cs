using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandsOnDeck.Classes.Collisions;
using HandsOnDeck.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck.Classes.Object.Entity
{
    internal class Player : IEntity
    {
        private Texture2D texture;
        private Rectangle spriteSelection;
        private Vector2 size = new Vector2(128,128);
        public Player(Texture2D texture)
        {
            this.texture = texture;
            spriteSelection = new Rectangle(0, 0, 180, 247);
            Hitbox = new Hitbox(new Rectangle(0, 0, 128, 128));
        }

        public Hitbox Hitbox { get; set ; }

        public void Draw()
        {
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
