using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HandsOnDeck;
using HandsOnDeck.Classes.Managers;

public class MousePositionDisplay
{
    private WorldCoordinate position;
    private SpriteFont font;

    public MousePositionDisplay()
    {
        font = Renderer.DefaultFont;
    }

    public void Update(GameTime gameTime)
    {
        MouseState mouseState = Mouse.GetState();
        position = new WorldCoordinate(mouseState.X, mouseState.Y);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        string positionText = $"X: {position.X}, Y: {position.Y}";
        Vector2 textSize = font.MeasureString(positionText);
        Vector2 drawPosition = new Vector2(200, 200);
        spriteBatch.DrawString(font, positionText, drawPosition, Color.White);
    }
}

