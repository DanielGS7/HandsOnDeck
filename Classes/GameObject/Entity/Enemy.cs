using HandsOnDeck2.Classes.Collision;
using HandsOnDeck2.Classes.Global;
using HandsOnDeck2.Classes.Rendering;
using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace HandsOnDeck2.Classes.GameObject.Entity
{
    internal abstract class Enemy : IEntity, ICollideable
    {
        protected ContentManager content;
        public SeaCoordinate Position { get; set; }
        public Vector2 Size { get; set; }
        public float Speed { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public float Scale { get; set; }
        public bool IsColliding { get; set; }
        public VisualElement VisualElement { get; set; }
        public int Lives { get; protected set; }
        protected bool IsCollidingWithIsland { get; set; }
        protected Texture2D HeartTexture { get; set; }
        public Collider Collider { get; protected set; }

        protected float targetRotation;
        protected const float RotationSpeed = 0.8f;
        protected const float MinDistanceToPlayer = 100f;
        protected const float AvoidanceRadius = 500f;
        protected const float AvoidanceForce = 300f;
        protected const float MaxAvoidanceAngle = MathHelper.PiOver4; // 45 graden
        protected float currentSpeed;

        protected Enemy(ContentManager content, SeaCoordinate position, Vector2 size, float speed, int lives)
        {
            this.content = content;
            Position = position;
            Size = size;
            Speed = speed;
            Scale = 1f;
            Origin = Size / 2f;
            Lives = lives;
            HeartTexture = content.Load<Texture2D>("heart");
            Collider = new Collider(new Rectangle(0, 0, (int)size.X, (int)size.Y), false);
            CollisionManager.Instance.AddCollideable(this);
        }

        public virtual void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 avoidanceForce = CalculateAvoidanceForce();
            UpdateTargetRotation(deltaTime, avoidanceForce);
            Move(deltaTime, avoidanceForce);

            Collider.UpdateBounds(new Rectangle(
                (int)(Position.X - Origin.X),
                (int)(Position.Y - Origin.Y),
                (int)Size.X,
                (int)Size.Y
            ));
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            VisualElement.Draw(spriteBatch, Position, Origin, Scale, Rotation);
            DrawHearts(spriteBatch);
        }

        protected virtual void UpdateTargetRotation(float deltaTime, Vector2 avoidanceForce)
        {
            Vector2 directionToPlayer = GetShortestDirectionToPlayer();
            float distanceToPlayer = directionToPlayer.Length();

            if (distanceToPlayer > MinDistanceToPlayer)
            {
                targetRotation = (float)Math.Atan2(directionToPlayer.Y, directionToPlayer.X);
            }

            if (avoidanceForce != Vector2.Zero)
            {
                float avoidanceAngle = (float)Math.Atan2(avoidanceForce.Y, avoidanceForce.X);
                float angleDifference = MathHelper.WrapAngle(avoidanceAngle - targetRotation);
                angleDifference = MathHelper.Clamp(angleDifference, -MaxAvoidanceAngle, MaxAvoidanceAngle);
                targetRotation = MathHelper.WrapAngle(targetRotation + angleDifference);
            }

            Rotation = SeaCoordinate.LerpAngle(Rotation, targetRotation, RotationSpeed * deltaTime);
        }

        protected virtual void Move(float deltaTime, Vector2 avoidanceForce)
        {
            Vector2 forwardDirection = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
            float avoidanceStrength = avoidanceForce.Length();
            float speedMultiplier = MathHelper.Lerp(1f, 0.5f, avoidanceStrength / AvoidanceForce);

            currentSpeed = Speed * speedMultiplier;
            Vector2 movement = forwardDirection * currentSpeed * deltaTime;

            Position = new SeaCoordinate(Position.X + movement.X, Position.Y + movement.Y);
        }

        protected Vector2 CalculateAvoidanceForce()
        {
            Vector2 totalForce = Vector2.Zero;

            foreach (var island in Map.Instance.GetIslands())
            {
                Vector2 toIsland = Position.GetShortestDirection(island.Position);
                float distance = toIsland.Length();
                if (distance < AvoidanceRadius + island.Size.X / 2)
                {
                    Vector2 avoidForce = Vector2.Normalize(toIsland) * (AvoidanceForce / (distance * distance));
                    totalForce -= avoidForce;
                }
            }

            Vector2 toPlayer = GetShortestDirectionToPlayer();
            float playerDistance = toPlayer.Length();
            if (playerDistance < AvoidanceRadius)
            {
                Vector2 avoidForce = Vector2.Normalize(toPlayer) * (AvoidanceForce / (playerDistance * playerDistance));
                totalForce -= avoidForce;
            }

            return totalForce;
        }

        protected Vector2 GetShortestDirectionToPlayer()
        {
            Boat playerBoat = Map.Instance.player;
            return Position.GetShortestDirection(playerBoat.Position);
        }

        protected void DrawHearts(SpriteBatch spriteBatch)
        {
            Vector2 baseHeartPosition = new Vector2(Position.X, Position.Y - Size.Y / 2 - 20);
            Vector2[] positions = GetDrawPositions(baseHeartPosition);

            foreach (Vector2 pos in positions)
            {
                Vector2 heartPosition = pos;
                for (int i = 0; i < Lives; i++)
                {
                    spriteBatch.Draw(HeartTexture, heartPosition, null, Color.White, 0f,
                        new Vector2(HeartTexture.Width / 2f, HeartTexture.Height / 2f), 1f,
                        SpriteEffects.None, 0f);
                    heartPosition.X += HeartTexture.Width + 5;
                }
            }
        }
        private Vector2[] GetDrawPositions(SeaCoordinate position)
        {
            int mapWidth = Map.MapWidth;
            int mapHeight = Map.MapHeight;
            Vector2 pos = position.ToVector2();

            return new Vector2[]
            {
                pos,
                new Vector2(pos.X - mapWidth, pos.Y),
                new Vector2(pos.X + mapWidth, pos.Y),
                new Vector2(pos.X, pos.Y - mapHeight),
                new Vector2(pos.X, pos.Y + mapHeight),
                new Vector2(pos.X - mapWidth, pos.Y - mapHeight),
                new Vector2(pos.X + mapWidth, pos.Y - mapHeight),
                new Vector2(pos.X - mapWidth, pos.Y + mapHeight),
                new Vector2(pos.X + mapWidth, pos.Y + mapHeight)
            };
        }

        public virtual void TakeDamage()
        {
            if (!IsCollidingWithIsland)
            {
                Lives--;
                if (Lives <= 0)
                {
                    Map.Instance.RemoveEnemy(this);
                }
            }
        }

        public virtual void OnCollision(ICollideable other)
        {
            if (other is Island)
            {
                if (!IsCollidingWithIsland)
                {
                    TakeDamage();
                    IsCollidingWithIsland = true;
                }
            }
            else
            {
                IsCollidingWithIsland = false;
            }
        }
    }
}