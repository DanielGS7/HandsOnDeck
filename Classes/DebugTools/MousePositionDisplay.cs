﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HandsOnDeck;

public class MousePositionDisplay
{
    private Vector2 position;
    private SpriteFont font;

    public MousePositionDisplay()
    {
        font = Game1.DefaultFont;
    }

    public void Update(GameTime gameTime)
    {
        MouseState mouseState = Mouse.GetState();
        position = new Vector2(mouseState.X, mouseState.Y);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        string positionText = $"X: {position.X}, Y: {position.Y}";
        Vector2 textSize = font.MeasureString(positionText);
        Vector2 drawPosition = new Vector2(200, 200);
        spriteBatch.DrawString(font, positionText, drawPosition, Color.White);
    }
}

