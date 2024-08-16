using HandsOnDeck2.Classes.GameObject.Entity;
using HandsOnDeck2.Classes.Global;
using HandsOnDeck2.Classes.Rendering;
using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

public class Bomb : IProjectile, ICollideable
{
    private const float ExplosionDelay = 3f;
    private const float ExplosionRadius = 100f;
    private const int TotalFrames = 5;
    private const float FloatAmplitude = 5f;
    private const float FloatFrequency = 2f;

    public VisualElement VisualElement { get; set; }
    public SeaCoordinate Position { get; set; }
    public Vector2 Size { get; set; }
    public float Rotation { get; set; }
    public Vector2 Origin { get; set; }
    public float Scale { get; set; }
    public bool IsColliding { get; set; }
    public bool IsExpired { get; private set; }
    public IGameObject Parent { get; }

    private float timeAlive = 0f;
    private float timePerFrame;
    private int currentFrame = 0;
    private Texture2D spriteSheet;
    private SeaCoordinate initialPosition;
    private Vector2 floatOffset;

    public Bomb(ContentManager content, SeaCoordinate position, IGameObject parent)
    {
        Position = position;
        initialPosition = position;
        Parent = parent;
        Size = new Vector2(50, 50);
        Scale = 1f;
        Origin = Size / 2f;

        spriteSheet = content.Load<Texture2D>("bomb");
        timePerFrame = ExplosionDelay / TotalFrames;

        Rectangle sourceRectangle = new Rectangle(0, 0, (int)Size.X, (int)Size.Y);
        VisualElement = new VisualElement(spriteSheet, Color.White, SpriteEffects.None, 0f, sourceRectangle);
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        timeAlive += deltaTime;

        int newFrame = (int)(timeAlive / timePerFrame);
        if (newFrame != currentFrame && newFrame < TotalFrames)
        {
            currentFrame = newFrame;
            UpdateVisualElement();
        }

        UpdateFloatOffset(timeAlive);

        if (timeAlive >= ExplosionDelay)
        {
            Explode();
        }
    }

    private void UpdateFloatOffset(float time)
    {
        float xOffset = FloatAmplitude * 0.5f * (float)Math.Sin(time * FloatFrequency * 2 * Math.PI);
        float yOffset = FloatAmplitude * (float)Math.Sin(time * FloatFrequency * Math.PI);

        floatOffset = new Vector2(xOffset, yOffset);
        Position = new SeaCoordinate(
            initialPosition.X + floatOffset.X,
            initialPosition.Y + floatOffset.Y
        );
    }

    private void UpdateVisualElement()
    {
        Rectangle sourceRectangle = new Rectangle(
            currentFrame * (int)Size.X, 0, (int)Size.X, (int)Size.Y);
        VisualElement.SetSourceRectangle(sourceRectangle);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        VisualElement.Draw(spriteBatch, Position, Origin, Scale, Rotation);
    }

    public void OnCollision(ICollideable other)
    {
        if (other != Parent)
        {
            Explode();
        }
    }

    private void Explode()
    {
        IsExpired = true;

        foreach (var collideable in Map.Instance.GetCollideables())
        {
            if (collideable != this && collideable != Parent)
            {
                float distance = Vector2.Distance(Position.ToVector2(), collideable.Position.ToVector2());
                if (distance <= ExplosionRadius)
                {
                    if (collideable is Enemy enemy)
                    {
                        enemy.TakeDamage();
                    }
                    else if (collideable is PlayerBoat playerBoat)
                    {
                        playerBoat.TakeDamage();
                    }
                }
            }
        }
    }
}