using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck2.Classes;
using System;

public class ScoreDisplay : UIElement
{
    private Texture2D woodTexture;
    private SpriteFont font;
    private int score;

    public ScoreDisplay(ContentManager content, Vector2 positionPercentage, Vector2 sizePercentage) 
        : base(positionPercentage, sizePercentage, 0f)
    {
        woodTexture = content.Load<Texture2D>("wood_name");
        font = content.Load<SpriteFont>("default");
        score = 0;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Vector2 origin = new Vector2(woodTexture.Width / 2f, woodTexture.Height / 2f);
        float woodScale = Math.Min(size.X / woodTexture.Width, size.Y / woodTexture.Height);

        spriteBatch.Draw(woodTexture, position, null, Color.White, rotation, origin, woodScale, SpriteEffects.None, 0f);

        string scoreText = score.ToString();
        Vector2 textSize = font.MeasureString(scoreText);
        float textScale = Math.Min(size.X / textSize.X, size.Y / textSize.Y) * 0.5f;
        Vector2 textPosition = position;
        Vector2 textOrigin = textSize / 2f;

        spriteBatch.DrawString(font, scoreText, textPosition, Color.White, rotation, textOrigin, textScale, SpriteEffects.None, 0f);
    }

    public void SetScore(int newScore)
    {
        score = newScore;
    }

    public int GetScore()
    {
        return score;
    }
}