using HandsOnDeck.Classes.Animations;
using HandsOnDeck.Classes.Object.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Classes.UI.UIElements
{
    internal class HeartContainer : UIElement
    {
        private Animation fullHealth;
        private Animation noHealth;
        private Animation fourHealth;
        private Animation oneHealth;
        private Animation twoHealth;
        private Animation threeHealth;
        private WorldCoordinate position;
        private int Xlives { get { return Player.GetInstance.lifePoints; } }

        public HeartContainer(WorldCoordinate givenPosition )
        {
            position = givenPosition;
            fullHealth = new Animation("heartshealth", new Vector2(283, 43), 0, 1, 6, 0, false);
            fourHealth = new Animation("heartshealth", new Vector2(283, 43), 1, 1, 6, 0, false);
            threeHealth = new Animation("heartshealth", new Vector2(283, 43), 2, 1, 6, 0, false);
            twoHealth = new Animation("heartshealth", new Vector2(283, 43), 3, 1, 6, 0, false);
            oneHealth = new Animation("heartshealth", new Vector2(283, 42), 4, 1, 6, 0, false);
            noHealth = new Animation("heartshealth", new Vector2(283, 42), 5, 1, 6, 0, false);

        }

        internal override void Initialize()
        {
            
        }
        internal override void LoadContent()
        {
            fullHealth.LoadContent();
            fourHealth.LoadContent();
            threeHealth.LoadContent();
            twoHealth.LoadContent();
            oneHealth.LoadContent();
            noHealth.LoadContent(); 
        }

        public override void Update(GameTime gameTime)
        {
            switch (Xlives) 
            {
                case 0:
                {
                    noHealth.Update(gameTime);
                    break;
                }
                
                case 1:
                {
                    oneHealth.Update(gameTime);
                    break;
                }
                
                case 2: 
                {
                    twoHealth.Update(gameTime); 
                    break;
                }
                    
                case 3: 
                {
                    threeHealth.Update(gameTime);
                    break;
                }

                case 4: 
                {
                    fourHealth.Update(gameTime);
                    break;
                }

                case 5: 
                {
                    fullHealth.Update(gameTime);
                    break;
                }
            }
        }
        
        public override void Draw(GameTime gameTime)
        {
            switch (Xlives)
            {
                case 0:
                    {
                        noHealth.Draw(position);
                        break;
                    }

                case 1:
                    {
                        oneHealth.Draw(position);
                        break;
                    }

                case 2:
                    {
                        twoHealth.Draw(position);
                        break;
                    }

                case 3:
                    {
                        threeHealth.Draw(position);
                        break;
                    }

                case 4:
                    {
                        fourHealth.Draw(position);
                        break;
                    }

                case 5:
                    {
                        fullHealth.Draw(position);
                        break;
                    }
            }
        }

       

    }
}
