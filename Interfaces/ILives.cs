using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Interfaces
{
    internal interface ILives
    {
        internal int lifePoints { get; set; }
        
        void TakeDamage();

        void ResetLives();

    }
}
