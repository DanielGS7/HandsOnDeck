using HandsOnDeck2.Classes.GameObject.Entity;
using HandsOnDeck2.Classes.Global;
using HandsOnDeck2.Classes.Rendering;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

class RivalBoat : Enemy
{
    private float timeSinceLastShot = 0f;
    private const float DesiredDistance = 200f;
    private IProjectileFactory cannonballFactory;
    private const float RivalBoatSpeed = 130f;
    private const float RivalBoatRotationSpeed = 0.4f;
    private const float FlankingDistance = 250f;
    private const float ShootAngle = MathHelper.PiOver4 / 2;
    private bool canShoot = true;

    public RivalBoat(ContentManager content, SeaCoordinate position, IProjectileFactory projectileFactory)
        : base(content, position, new Vector2(200, 85), RivalBoatSpeed, 3, 6)
    {
        this.cannonballFactory = projectileFactory;
        var texture = content.Load<Texture2D>("rival_boat");
        VisualElement = new VisualElement(texture, Color.White, SpriteEffects.None, 0f);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 directionToPlayer = GetShortestDirectionToPlayer();
        float distanceToPlayer = directionToPlayer.Length();

        if (!canShoot)
        {
            timeSinceLastShot += deltaTime;
            if (timeSinceLastShot >= DifficultySettings.Instance.GetEnemyShootInterval())
            {
                canShoot = true;
                timeSinceLastShot = 0f;
            }
        }

        if (canShoot && distanceToPlayer <= DesiredDistance * 1.2f && IsInShootingPosition())
        {
            Shoot();
            canShoot = false;
        }

        UpdateTargetRotation(deltaTime, Vector2.Zero);
    }

    protected override void UpdateTargetRotation(float deltaTime, Vector2 avoidanceForce)
    {
        PlayerBoat playerBoat = Map.Instance.player;
        Vector2 playerDirection = new Vector2((float)Math.Cos(playerBoat.Rotation), (float)Math.Sin(playerBoat.Rotation));
        Vector2 perpendicularDirection = new Vector2(-playerDirection.Y, playerDirection.X);

        Vector2 leftFlankingPosition = playerBoat.Position.ToVector2() - perpendicularDirection * FlankingDistance;
        Vector2 rightFlankingPosition = playerBoat.Position.ToVector2() + perpendicularDirection * FlankingDistance;

        Vector2 flankingPosition = (Position.X < playerBoat.Position.X) ? leftFlankingPosition : rightFlankingPosition;
        Vector2 directionToFlankingPos = Position.GetShortestDirection(new SeaCoordinate(flankingPosition.X, flankingPosition.Y));

        targetRotation = (float)Math.Atan2(directionToFlankingPos.Y, directionToFlankingPos.X);

        if (avoidanceForce != Vector2.Zero)
        {
            float avoidanceAngle = (float)Math.Atan2(avoidanceForce.Y, avoidanceForce.X);
            float angleDifference = MathHelper.WrapAngle(avoidanceAngle - targetRotation);
            angleDifference = MathHelper.Clamp(angleDifference, -MaxAvoidanceAngle, MaxAvoidanceAngle);
            targetRotation = MathHelper.WrapAngle(targetRotation + angleDifference);
        }

        Rotation = SeaCoordinate.LerpAngle(Rotation, targetRotation, RivalBoatRotationSpeed * deltaTime);
    }

    protected void Shoot()
    {
        Vector2 directionToPlayer = GetShortestDirectionToPlayer();
        bool shootFromRight = Vector2.Dot(directionToPlayer, new Vector2((float)Math.Cos(Rotation + MathHelper.PiOver2), (float)Math.Sin(Rotation + MathHelper.PiOver2))) > 0;

        float baseAngle = shootFromRight ? Rotation + MathHelper.PiOver2 : Rotation - MathHelper.PiOver2;
        float[] shootAngles = { baseAngle - ShootAngle, baseAngle, baseAngle + ShootAngle };

        foreach (float angle in shootAngles)
        {
            Vector2 shootDirection = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            Vector2 spawnOffset = shootDirection * (Size.X / 2 + 10); 
            SeaCoordinate spawnPosition = new SeaCoordinate(Position.X + spawnOffset.X, Position.Y + spawnOffset.Y);

            IProjectile cannonball = cannonballFactory.CreateProjectile(spawnPosition, shootDirection, this);
            Map.Instance.AddProjectile(cannonball);
        }
    }

    private bool IsInShootingPosition()
    {
        Vector2 directionToPlayer = GetShortestDirectionToPlayer();
        float angleToPlayer = (float)Math.Atan2(directionToPlayer.Y, directionToPlayer.X);
        float angleDifference = Math.Abs(MathHelper.WrapAngle(angleToPlayer - Rotation));

        return angleDifference > MathHelper.PiOver2 - 0.2f && angleDifference < MathHelper.PiOver2 + 0.2f;
    }
}