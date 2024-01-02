using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandsOnDeck.Classes.Animations;
using HandsOnDeck.Classes.Collisions;
using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck.Classes.Object.Entity
{
    internal class Player : IEntity
    {
        
        private Rectangle spriteSelection;
        private Vector2 size = new Vector2(128,128);
        private Animation boot = new Animation("image1", new Vector2(672, 243), 0, 1, 1, 0, false);
        public Player()
        {
            
            spriteSelection = new Rectangle(0, 0, 180, 247);
            Hitbox = new Hitbox(new Rectangle(0, 0, 128, 128));
            type = HitboxType.Physical;
        }

        public Hitbox Hitbox { get; set ; }
        public HitboxType type { get ; set ; }
       

        public void LoadContent()
        {
            boot.LoadContent();
        }

        public void Draw()
        {
            GraphicsDevice _graphics = GraphicsDeviceSingleton.Instance;
            Vector2 position = new Vector2(_graphics.Viewport.Width / 2f-336, _graphics.Viewport.Height / 2f-121);
            boot.Draw(position);
        }

        public void Update(GameTime gameTime)
        {
            boot.Update(gameTime);
        }
    }
}
