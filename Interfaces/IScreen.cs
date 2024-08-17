using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HandsOnDeck2.Interfaces
{
    public interface IScreen
    {
        void Initialize();
        void LoadContent();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        void HandleInput();
        bool IsActive { get; set; }
    }
}