using HandsOnDeck;
using HandsOnDeck.Classes.MonogameAccessibility;
using HandsOnDeck.Classes.UI.UIElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Toggle : UIElement
{
    private const float ScaleFactor = 3.0f;
    private const int Padding = 10;
    private string text;
    private Vector2 position;
    private Action action;
    private SpriteFont font;
    private Texture2D texture;
    private Texture2D hoverTexture;
    private Rectangle bounds;
    private bool isToggled;
    private bool isHovered;
    private bool isClicked;

    public Toggle(string text, Vector2 centerPosition, bool initialState, Action action)
    {
        this.text = text;
        this.action = action;
        this.font = Game1.DefaultFont;
        this.isToggled = initialState;

        Vector2 size = (font.MeasureString(text) + new Vector2(Padding * 2, Padding * 2)) * ScaleFactor;
        this.bounds = new Rectangle(
            (int)(centerPosition.X - size.X / 2),
            (int)(centerPosition.Y - size.Y / 2),
            (int)size.X + 200,
            (int)size.Y
        );
        this.position = new Vector2(bounds.X + Padding * ScaleFactor, bounds.Y + Padding * ScaleFactor);
    }

    internal override void Initialize()
    {
        this.font = Game1.DefaultFont ?? throw new InvalidOperationException("DefaultFont not loaded.");
        Vector2 size = font.MeasureString(text);
        this.bounds = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
    }

    internal override void LoadContent()
    {
        this.texture = ContentLoader.Load<Texture2D>("button/button");
        this.hoverTexture = ContentLoader.Load<Texture2D>("button/buttonH");
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
            action.Invoke();
        }
    }

    public override void Draw(GameTime gameTime)
    {
        Texture2D buttonSprite = isHovered ? Game1.ButtonHoverSprite : Game1.ButtonSprite;
        Draw9Slice(buttonSprite, bounds, null, (int)(10 * ScaleFactor));

        Vector2 textPosition = new Vector2(
            bounds.X + bounds.Width / 2 - font.MeasureString(text).X / 2 * ScaleFactor - 50,
            bounds.Y + bounds.Height / 2 - font.MeasureString(text).Y / 2 * ScaleFactor
        );
        SpriteBatchManager.Instance.DrawString(font, text, textPosition, Color.White, 0f, Vector2.Zero, ScaleFactor, SpriteEffects.None, 0f);

        Color toggleColor = isToggled ? Color.Green : Color.Red;
        Rectangle toggleIndicatorBounds = new Rectangle(
            bounds.X + bounds.Width - (int)(Padding * 1.5f) -125,
            bounds.Y + (int)(Padding * 0.5f * ScaleFactor) +25,
            (int)(bounds.Height - Padding * ScaleFactor)-50,
            (int)(bounds.Height - Padding * ScaleFactor)-50
        );
        SpriteBatchManager.Instance.Draw(Game1.ButtonSprite, toggleIndicatorBounds, toggleColor);
    }
}
