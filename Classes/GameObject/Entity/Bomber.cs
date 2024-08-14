using HandsOnDeck2.Classes.GameObject.Entity;
using HandsOnDeck2.Classes.Global;
using HandsOnDeck2.Classes.Rendering;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

class Bomber : Enemy
{
    private float timeSinceLastDrop = 0f;
    private const float DesiredDistance = 150f;
    private IProjectileFactory projectileFactory;
    private const float BomberSpeed = 150f;
    private const float BomberRotationSpeed = 0.3f;
    private bool canDrop = true;

    public Bomber(ContentManager content, SeaCoordinate position, IProjectileFactory projectileFactory)
        : base(content, position, new Vector2(150, 78), BomberSpeed, 2)
    {
        this.projectileFactory = projectileFactory;
        var texture = content.Load<Texture2D>("bomber");
        VisualElement = new VisualElement(texture, Color.White, SpriteEffects.None, 0f);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (!canDrop)
        {
            timeSinceLastDrop += deltaTime;
            if (timeSinceLastDrop >= DifficultySettings.Instance.GetEnemyShootInterval())
            {
                canDrop = true;
                timeSinceLastDrop = 0f;
            }
        }

        if (canDrop && IsInFrontOfPlayer())
        {
            Shoot();
            canDrop = false;
        }

        Vector2 avoidanceForce = CalculateAvoidanceForce();
        UpdateTargetRotation(deltaTime, avoidanceForce);
    }

    protected override void UpdateTargetRotation(float deltaTime, Vector2 avoidanceForce)
    {
        Boat playerBoat = Map.Instance.player;
        Vector2 playerDirection = new Vector2((float)Math.Cos(playerBoat.Rotation), (float)Math.Sin(playerBoat.Rotation));
        Vector2 perpendicularDirection = new Vector2(-playerDirection.Y, playerDirection.X);
        Vector2 leftTarget = playerBoat.Position.ToVector2() - perpendicularDirection * DesiredDistance;
        Vector2 rightTarget = playerBoat.Position.ToVector2() + perpendicularDirection * DesiredDistance;

        Vector2 targetPoint = (Position.X < playerBoat.Position.X) ? leftTarget : rightTarget;
        Vector2 directionToTarget = Position.GetShortestDirection(new SeaCoordinate(targetPoint.X, targetPoint.Y));

        targetRotation = (float)Math.Atan2(directionToTarget.Y, directionToTarget.X);

        if (avoidanceForce != Vector2.Zero)
        {
            float avoidanceAngle = (float)Math.Atan2(avoidanceForce.Y, avoidanceForce.X);
            float angleDifference = MathHelper.WrapAngle(avoidanceAngle - targetRotation);
            angleDifference = MathHelper.Clamp(angleDifference, -MaxAvoidanceAngle, MaxAvoidanceAngle);
            targetRotation = MathHelper.WrapAngle(targetRotation + angleDifference);
        }

        Rotation = SeaCoordinate.LerpAngle(Rotation, targetRotation, BomberRotationSpeed * deltaTime);
    }

    protected void Shoot()
    {
        IProjectile bomb = projectileFactory.CreateProjectile(Position, Vector2.Zero, this);
        Map.Instance.AddProjectile(bomb);
    }

    private bool IsInFrontOfPlayer()
    {
        Boat playerBoat = Map.Instance.player;
        Vector2 directionToPlayer = GetShortestDirectionToPlayer();
        Vector2 playerForward = new Vector2((float)Math.Cos(playerBoat.Rotation), (float)Math.Sin(playerBoat.Rotation));

        float angleToPlayer = (float)Math.Atan2(directionToPlayer.Y, directionToPlayer.X);
        float playerAngle = (float)Math.Atan2(playerForward.Y, playerForward.X);

        float angleDifference = Math.Abs(MathHelper.WrapAngle(angleToPlayer - playerAngle));
        return angleDifference < MathHelper.PiOver4 || angleDifference > MathHelper.Pi - MathHelper.PiOver4;
    }
}