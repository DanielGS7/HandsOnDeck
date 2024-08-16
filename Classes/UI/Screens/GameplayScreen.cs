using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HandsOnDeck2.Classes.Rendering;
using HandsOnDeck2.Classes.GameObject.Entity;
using HandsOnDeck2.Classes.Global;
using HandsOnDeck2.Classes.Sound;
using HandsOnDeck2.Enums;
using HandsOnDeck2.Classes.CodeAccess;

namespace HandsOnDeck2.Classes.UI.Screens
{
public class GameplayScreen : Screen
{
    private Map gameMap;
    private GameOverlay gameOverlay;
    private float waterLevel = 0f;
    private float waterIncreaseRate;
    private int holeCount = 0;
    private const int MaxHoles = 5;
    private const float MaxWaterLevel = 1f;
    private bool isSinking = false;
    private float sinkingTimer = 0f;
    private const float SinkingDuration = 3f;
    private Color sinkingOverlayColor = new Color(0, 0, 139, 0);
    private float timeSurvived;
    private int minutesSurvived;
    private int cannonballsDodged;
    private Texture2D pixel;

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
            waterIncreaseRate = DifficultySettings.Instance.GetWaterIncreaseRate();
            gameMap.player.OnDamageTaken += AddDamage;
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
            if (!isSinking)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                HandleInput();
                gameMap.Update(gameTime);
                UpdateWaterLevel(gameTime);
                gameOverlay.Update(gameTime);
                UpdateScoring(deltaTime);
                if (waterLevel >= MaxWaterLevel)
                {
                    StartSinking();
                }
            }
            else
            {
                UpdateSinkingEffect(gameTime);
            }
        }
    }

    public override void HandleInput()
    {
        if (InputManager.Instance.IsKeyPressed(Keys.R))
        {
            TriggerReload();
        }
        if (InputManager.Instance.IsKeyPressed(Keys.F))
        {
            TriggerRepair();
        }
        if (InputManager.Instance.IsKeyPressed(Keys.B))
        {
            TriggerBucket();
        }
    }

    private void UpdateWaterLevel(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        waterLevel += waterIncreaseRate * holeCount * deltaTime;
        waterLevel = MathHelper.Clamp(waterLevel, 0f, MaxWaterLevel);
        gameOverlay.SetWaterLevel(waterLevel);
    }

    private void UpdateScoring(float deltaTime)
    {
        timeSurvived += deltaTime;
        int newMinutesSurvived = (int)(timeSurvived / 60f);
        if (newMinutesSurvived > minutesSurvived)
        {
            int pointsEarned = 2 + (2 * newMinutesSurvived);
            AddScore(pointsEarned);
            minutesSurvived = newMinutesSurvived;
        }
    }

    public void TriggerReload()
    {
        gameOverlay.TriggerReload();
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
        }
    }

    public void AddDamage()
    {
        if (holeCount < MaxHoles)
        {
            holeCount++;
            gameOverlay.UpdateDamageDisplay();
        }
    }

    public void AddScore(int points)
    {
        GlobalInfo.Score += points;
        AudioManager.Instance.Play("score");
        gameOverlay.UpdateScore();
    }

    public void RepairHole()
    {
        if (holeCount > 0)
        {
            holeCount--;
            gameOverlay.UpdateDamageDisplay();
        }
    }

    public void DecreaseWaterLevel(float amount)
    {
        waterLevel = MathHelper.Max(0, waterLevel - amount);
        gameOverlay.SetWaterLevel(waterLevel);
    }

    internal void EnemyDestroyed(Enemy enemy)
    {
        AddScore(enemy.PointValue);
    }

    public void CannonballDodged()
    {
        if (!gameMap.player.IsInvincible)
        {
            cannonballsDodged++;
            AddScore(1);
        }
    }
    private void StartSinking()
    {
        isSinking = true;
        sinkingTimer = 0f;
    }

    private void UpdateSinkingEffect(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        sinkingTimer += deltaTime;

        float progress = sinkingTimer / SinkingDuration;
        sinkingOverlayColor.A = (byte)(progress * 255 * 0.5f);

        if (sinkingTimer >= SinkingDuration)
        {
            EndGame();
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        gameMap.Draw(spriteBatch);
        gameOverlay.Draw(spriteBatch);

        if (isSinking)
        {
            spriteBatch.Draw(Texture2DHelper.GetWhiteTexture(graphicsDevice), GraphDev.GetInstance.Viewport.Bounds, sinkingOverlayColor);
        }
    }

    private void EndGame()
    {
        ScreenManager.Instance.ChangeScreen(ScreenType.GameOver);
        ((GameOverScreen)ScreenManager.Instance.screens[ScreenType.GameOver]).SetScore(GlobalInfo.Score);
    }

    public int GetHoleCount() => holeCount;
    public float GetWaterLevel() => waterLevel;
}
}