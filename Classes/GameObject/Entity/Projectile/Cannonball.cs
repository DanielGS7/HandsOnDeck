using HandsOnDeck2.Classes.Global;
using HandsOnDeck2.Classes.Rendering;
using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using HandsOnDeck2.Classes.Sound;

public class Cannonball : IProjectile
{
    private const float Speed = 300f;
    private const float Lifetime = 5f;
    private const float RotationSpeed = 5f;

    public VisualElement VisualElement { get; set; }
    public SeaCoordinate Position { get; set; }
    public Vector2 Size { get; set; }
    public float Rotation { get; set; }
    public Vector2 Origin { get; set; }
    public float Scale { get; set; }
    public bool IsColliding { get; set; }
    public bool IsExpired { get; internal set; }
    public IGameObject Parent { get; }

    private Vector2 Direction { get; }
    private float timeAlive = 0f;
    private Rectangle viewport;

    public Cannonball(ContentManager content, SeaCoordinate position, Vector2 direction, IGameObject parent)
    {
        Position = position;
        Direction = Vector2.Normalize(direction);
        Parent = parent;
        Size = new Vector2(35, 35);
        Scale = 1f;
        Origin = Size / 2f;
        var texture = content.Load<Texture2D>("cannonball");
        VisualElement = new VisualElement(texture, Color.White, SpriteEffects.None, 0f);
        viewport = Map.Instance.Camera.GetViewport();
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Position = new SeaCoordinate(
            Position.X + Direction.X * Speed * deltaTime,
            Position.Y + Direction.Y * Speed * deltaTime
        );

        Rotation += RotationSpeed * deltaTime;

        timeAlive += deltaTime;
        if (timeAlive >= Lifetime && !IsInViewport())
        {
            IsExpired = true;
        }

        if (Parent != Map.Instance.player)
        {
            float distanceToPlayer = Vector2.Distance(Position.ToVector2(), Map.Instance.player.Position.ToVector2());
            if (distanceToPlayer < 50 && !IsExpired)
            {
                AudioManager.Instance.Play("cannonball_flyby");
                IsExpired = true;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        VisualElement.Draw(spriteBatch, Position, Origin, Scale, Rotation);
    }

    public virtual void OnCollision(ICollideable other)
    {
        if (other != Parent)
        {
            IsExpired = true;
        }
    }

    private bool IsInViewport()
    {
        return viewport.Contains(Position.ToVector2());
    }
}