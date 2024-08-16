using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HandsOnDeck2.Classes.Rendering;
using HandsOnDeck2.Classes.GameObject.Entity;
using HandsOnDeck2.Classes.Global;
using HandsOnDeck2.Classes.Sound;
using HandsOnDeck2.Enums;

namespace HandsOnDeck2.Classes.UI.Screens
{
    public class GameplayScreen : Screen
    {
        private Map gameMap;
        private GameOverlay gameOverlay;
        private float waterLevel = 0f;
        private float waterIncreaseRate = 0.01f;
        private int score = 0;
        private int holeCount = 0;
        private const int MaxHoles = 5;
        private const float MaxWaterLevel = 1f;
        private bool isBoatSinking = false;
        private float sinkingTimer = 0f;
        private const float SinkingDuration = 3f;

        public GameplayScreen(GraphicsDevice graphicsDevice, ContentManager content) : base(graphicsDevice, content)
        {
        }

        public override void Initialize()
        {
            if (gameMap == null)
            {
                gameMap = Map.Instance;
                gameMap.Initialize(content, graphicsDevice);
                gameOverlay = new GameOverlay(content, graphicsDevice.Viewport, gameMap.player, this);
                base.Initialize();
            }
        }

        public override void LoadContent()
        {
            gameMap.LoadContent();
            base.LoadContent();
        }


    public override void Update(GameTime gameTime)
    {
        if (IsActive)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!isBoatSinking)
            {
                HandleInput();
                gameMap.Update(gameTime);
                UpdateWaterLevel(deltaTime);
                gameOverlay.Update(gameTime);

                if (gameOverlay.IsWaterGaugeFull())
                {
                    isBoatSinking = true;
                    sinkingTimer = 0f;
                }
            }
            else
            {
                sinkingTimer += deltaTime;
                if (sinkingTimer >= SinkingDuration)
                {
                    EndGame();
                }
            }
        }
    }

        public override void HandleInput()
        {
            if (InputManager.Instance.IsKeyPressed(Keys.F))
            {
                TriggerRepair();
            }
            if (InputManager.Instance.IsKeyPressed(Keys.B))
            {
                TriggerBucket();
            }
            if (InputManager.Instance.IsKeyPressed(Keys.R))
            {
                TriggerReload();
            }
        }

        private void UpdateWaterLevel(float deltaTime)
        {
            waterLevel += waterIncreaseRate * holeCount * deltaTime;
            waterLevel = MathHelper.Clamp(waterLevel, 0f, 1f);
            gameOverlay.SetWaterLevel(waterLevel);
        }

        public void TriggerRepair()
        {
            gameOverlay.TriggerRepair();
        }

        public void TriggerBucket()
        {
            if (gameOverlay.TriggerBucket())
            {
                DecreaseWaterLevel(0.1f);
                AudioManager.Instance.Play("bucket");
            }
        }

        public void TriggerReload()
        {
            gameOverlay.TriggerReload();
        }

        private void DecreaseWaterLevel(float amount)
        {
            waterLevel = MathHelper.Max(0, waterLevel - amount);
            gameOverlay.SetWaterLevel(waterLevel);
        }

        public void AddDamage()
        {
            if (holeCount < MaxHoles)
            {
                holeCount++;
                gameOverlay.UpdateDamageDisplay();
                AudioManager.Instance.Play("damage");
            }
        }

        public void RepairHole()
        {
            if (holeCount > 0)
            {
                holeCount--;
                gameOverlay.UpdateDamageDisplay();
            }
        }

        public void AddScore(int points)
        {
            score += points;
            gameOverlay.UpdateScore(score);
            AudioManager.Instance.Play("score");
        }

        private bool IsBoatSinking()
        {
            return waterLevel >= MaxWaterLevel;
        }

        private void EndGame()
        {
            ScreenManager.Instance.ChangeScreen(ScreenType.GameOver);
            ((GameOverScreen)ScreenManager.Instance.screens[ScreenType.GameOver]).SetScore(score);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            gameMap.Draw(spriteBatch);
            gameOverlay.Draw(spriteBatch);
        }

        public int GetHoleCount() => holeCount;
        public float GetWaterLevel() => waterLevel;
        public int GetScore() => score;
    }
}