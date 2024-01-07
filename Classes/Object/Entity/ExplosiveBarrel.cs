using HandsOnDeck.Classes.Animations;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HandsOnDeck.Classes.Object.Entity
{
    internal class ExplosiveBarrel : CollideableGameObject
    {
        private Animation explosiveBarrel = new Animation("Unstable_Barrel",new Vector2(360,360),0,1,1,0, false);
        private float activationRange = 100f;

        public ExplosiveBarrel(Vector2 startPosition) 
        {
            position = startPosition;
        }

       

        public override void LoadContent()
        {
            explosiveBarrel.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            explosiveBarrel.Update(gameTime);
        }


        public override void Draw(GameTime gameTime, Player player)
        {
            if(player.position - position< activationRange)
            {
                explosiveBarrel.Draw(position);
            }

        }
    }
}
