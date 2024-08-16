using HandsOnDeck2.Classes;
using HandsOnDeck2.Classes.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

public class ActionTimer : UIElement
{
    private Texture2D timerTexture;
    private SpriteFont font;
    private string label;
    private float currentTime;
    private float maxTime;
    private const int TotalFrames = 9;
    private bool hasFinished;
    private Action finishedEvent;

    public ActionTimer(ContentManager content, Vector2 positionPercentage, Vector2 sizePercentage, string label, float maxTime, Action finishedEvent = null)
        : base(positionPercentage, sizePercentage, 0f)
    {
        this.label = label;
        this.maxTime = maxTime;
        this.finishedEvent = finishedEvent;
        timerTexture = content.Load<Texture2D>("timer");
        font = content.Load<SpriteFont>("default");

        VisualElement = new VisualElement(timerTexture, Color.White, SpriteEffects.None, 0f);
    }

    public void Trigger()
    {
        if (currentTime <= 0)
        {
            currentTime = maxTime;
            hasFinished = false;
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (currentTime > 0)
        {
            currentTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (currentTime <= 0)
            {
                currentTime = 0;
                if (!hasFinished)
                {
                    FinishedTimerEvent();
                    hasFinished = true;
                }
            }
        }
    }

    private void FinishedTimerEvent()
    {
        finishedEvent?.Invoke();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Vector2 origin = new Vector2(timerTexture.Width / (2f * TotalFrames), timerTexture.Height / 2f);
        float timerScale = Math.Min(size.X / (timerTexture.Width / TotalFrames), size.Y / timerTexture.Height);

        int frame = (int)((TotalFrames - 1) * (1 - currentTime / maxTime));
        Rectangle sourceRect = new Rectangle(frame * (timerTexture.Width / TotalFrames), 0, timerTexture.Width / TotalFrames, timerTexture.Height);

        spriteBatch.Draw(timerTexture, position, sourceRect, Color.White, rotation, origin, timerScale, SpriteEffects.None, 0f);

        Vector2 textSize = font.MeasureString(label);
        float textScale = Math.Min(size.X / textSize.X, size.Y / textSize.Y) * 0.5f;
        Vector2 textPosition = position + new Vector2(size.X * 0.6f, 0);
        Vector2 textOrigin = textSize / 2f;

        spriteBatch.DrawString(font, label, textPosition, Color.White, rotation, textOrigin, textScale, SpriteEffects.None, 0f);
    }

    public bool IsComplete()
    {
        return currentTime <= 0;
    }
}