using HandsOnDeck;
using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Classes.UI.UIElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Button : UIElement
{
    private string text;
    private Vector2 position;
    private Action action;
    private SpriteFont font;
    private Texture2D texture;
    private Texture2D hoverTexture;
    private Rectangle bounds;
    private bool isHovered;
    private bool isClicked;

    public Button(string text, Vector2 position, Action action)
    {
        this.text = text;
        this.position = position;
        this.action = action;
        this.font = Game1.DefaultFont;
        this.texture = ContentLoader.Load<Texture2D>("button");
        this.hoverTexture = ContentLoader.Load<Texture2D>("buttonH");

        Vector2 size = font.MeasureString(text);
        this.bounds = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
    }

    public override void Update(GameTime gameTime)
    {
        MouseState mouseState = Mouse.GetState();
        Point mousePosition = new Point(mouseState.X, mouseState.Y);

        isHovered = bounds.Contains(mousePosition);
        if (isHovered && mouseState.LeftButton == ButtonState.Pressed)
        {
            isClicked = true;
        }
        else if (isClicked && mouseState.LeftButton == ButtonState.Released)
        {
            isClicked = false;
            action.Invoke();
        }
    }

    public override void Draw(GameTime gameTime)
    {
        Color color = isHovered ? Color.RosyBrown : Color.White; // Change color on hover
        Draw9Slice(texture, bounds, null, 10); // 10 is border size example
        SpriteBatchManager.Instance.DrawString(font, text, position, Color.Black);
    }
}
