using HandsOnDeck.Classes.Animations;
using HandsOnDeck.Classes.Collisions;
using HandsOnDeck.Classes.MonogameAccessibility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsOnDeck.Classes.Object.Static
{
    internal class Island : CollideableGameObject
    {
        bool isLevelStart = false;
        bool isLevelDestination = false;
        Texture2D islandSheet;
        Animation islandSprite; 
        bool playerNearby = false;
        int rotation;

        public Island(Vector2 position, int islandIndex, int rotation)
        {
            this.size = new Vector2(400, 400);
            this.position = position;
            this.rotation = rotation;
            islandSprite = new Animation("islands", size, islandIndex, 4, 16, 0, false);
            Hitbox = new Hitbox(new Rectangle(position.ToPoint(), size.ToPoint()), Enums.HitboxType.Physical);
        }
        public override void LoadContent()
        {
            islandSprite.LoadContent();
        }

        public override void Update(GameTime gameTime) {
        
        }

        public override void Draw(GameTime gameTime) { }

        public override void Draw(GameTime gameTime, Vector2 position)
        {
            islandSprite.Draw(position,1, rotation, size/new Vector2(2,2));
        }

        public override void onCollision(CollideableGameObject other)
        {
            Debug.WriteLine("Collided with island");
        }

    }
}
