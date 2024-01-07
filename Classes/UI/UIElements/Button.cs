using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using HandsOnDeck.Classes.UI.UIElements;
using HandsOnDeck;
using HandsOnDeck.Classes.MonogameAccessibility;

public class Button : UIElement
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
    private bool isHovered;
    private bool isClicked;

    public Button(string text, Vector2 centerPosition, Action action)
    {
        this.text = text;
        this.action = action;
        this.font = Game1.DefaultFont;

        Vector2 size = (font.MeasureString(text) + new Vector2(Padding * 2, Padding * 2)) * ScaleFactor;
        this.bounds = new Rectangle(
            (int)(centerPosition.X - size.X / 2),
            (int)(centerPosition.Y - size.Y / 2),
            (int)size.X,
            (int)size.Y
        );
        this.position = new Vector2(bounds.X, bounds.Y);
    }


    internal override void Initialize()
    {

    }

    internal override void LoadContent()
    {
        this.texture = ContentLoader.Load<Texture2D>("button/button");
        this.hoverTexture = ContentLoader.Load<Texture2D>("button/buttonH");
    }

    public override void Update(GameTime gameTime)
    {
        MouseState mouseState = Mouse.GetState();
        Point mousePosition = Game1.transformedMousePosition;
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
        Color color = isHovered ? Color.LightSeaGreen : Color.White;
        Texture2D buttonSprite = isHovered ? Game1.ButtonHoverSprite : Game1.ButtonSprite;
        Draw9Slice(buttonSprite, bounds, null, (int)(10 * ScaleFactor));
        Vector2 textPosition = new Vector2(bounds.X + bounds.Width / 2 - font.MeasureString(text).X / 2 * ScaleFactor,
                                           bounds.Y + bounds.Height / 2 - font.MeasureString(text).Y / 2 * ScaleFactor);
        SpriteBatchManager.Instance.DrawString(font, text, textPosition, color, 0f, Vector2.Zero, ScaleFactor, SpriteEffects.None, 0f);
    }
}

