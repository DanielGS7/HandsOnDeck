using HandsOnDeck;
using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Classes.UI.UIElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static System.Net.Mime.MediaTypeNames;

public class Toggle : UIElement
{
    private Vector2 position;
    private Action<bool> action;
    private SpriteFont font;
    private Texture2D texture;
    private Texture2D hoverTexture;
    private Rectangle bounds;
    private bool isToggled;
    private bool isHovered;
    private bool isClicked;
    private string text;

    public Toggle(string text, Vector2 position, bool initialState, Action<bool> action)
    {
        this.text = text;
        this.position = position;
        this.action = action;
        this.font = Game1.DefaultFont;
        this.texture = ContentLoader.Load<Texture2D>("button");
        this.hoverTexture = ContentLoader.Load<Texture2D>("buttonH");
        this.isToggled = initialState;

        Vector2 size = font.MeasureString(text);
        this.bounds = new Rectangle((int)position.X, (int)position.Y, (int)size.X + 20, (int)size.Y);
    }

    internal override void LoadContent()
    {
        this.texture = ContentLoader.Load<Texture2D>("button");
        this.hoverTexture = ContentLoader.Load<Texture2D>("buttonH");
    }

    internal override void Initialize()
    {
        this.font = Game1.DefaultFont ?? throw new InvalidOperationException("DefaultFont not loaded.");
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
            isToggled = !isToggled;
            action.Invoke(isToggled);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        Color toggleColor = isToggled ? Color.Green : Color.Red;
        Color textColor = isHovered ? Color.Gray : Color.White;
        SpriteBatchManager.Instance.Draw(texture, bounds, textColor);
        SpriteBatchManager.Instance.DrawString(font, text, position, Color.Black);

        Rectangle toggleButtonBounds = new Rectangle(bounds.Right + 5, bounds.Y, bounds.Height, bounds.Height);
        SpriteBatchManager.Instance.Draw(texture, toggleButtonBounds, toggleColor);
    }
}
