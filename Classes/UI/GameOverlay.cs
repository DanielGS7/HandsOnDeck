using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck2.Classes.GameObject.Entity;
using HandsOnDeck2.Classes.UI.Screens;
using System;
using HandsOnDeck2.Classes;

public class GameOverlay
{
    private ScoreDisplay scoreDisplay;
    private ActionTimer reloadTimer;
    private ActionTimer repairTimer;
    private ActionTimer bucketTimer;
    private WaterGauge waterGauge;
    private DamageDisplay damageDisplay;
    private CannonStatus leftCannon;
    private CannonStatus rightCannon;
    private PlayerBoat playerBoat;
    private GameplayScreen gameplayScreen;

    private const float ReloadTime = 2f;
    private const float RepairTime = 3f;
    private const float BucketTime = 4f;

    public GameOverlay(ContentManager content, Viewport viewport, PlayerBoat playerBoat, GameplayScreen gameplayScreen)
    {
        this.playerBoat = playerBoat;
        this.gameplayScreen = gameplayScreen;

        scoreDisplay = new ScoreDisplay(content, new Vector2(0.92f, 0.05f), new Vector2(0.12f, 0.12f));
        reloadTimer = new ActionTimer(content, new Vector2(0.05f, 0.05f), new Vector2(0.1f, 0.05f), "RELOAD", ReloadTime, OnReloadTimerFinished);
        repairTimer = new ActionTimer(content, new Vector2(0.05f, 0.15f), new Vector2(0.1f, 0.05f), "FIX", RepairTime);
        bucketTimer = new ActionTimer(content, new Vector2(0.05f, 0.25f), new Vector2(0.1f, 0.05f), "BUCKET", BucketTime);
        waterGauge = new WaterGauge(content, new Vector2(0.05f, 0.85f), new Vector2(0.2f, 0.2f));
        damageDisplay = new DamageDisplay(content, new Vector2(0.15f, 0.85f), new Vector2(0.20f, 0.20f), gameplayScreen);
        leftCannon = new CannonStatus(content, new Vector2(0.45f, 0.9f), new Vector2(0.05f, 0.05f), playerBoat, true);
        rightCannon = new CannonStatus(content, new Vector2(0.55f, 0.9f), new Vector2(0.05f, 0.05f), playerBoat, false);
    }

    public void Update(GameTime gameTime)
    {
        scoreDisplay.Update(gameTime);
        reloadTimer.Update(gameTime);
        repairTimer.Update(gameTime);
        bucketTimer.Update(gameTime);
        damageDisplay.Update(gameTime);
        leftCannon.Update(gameTime);
        rightCannon.Update(gameTime);
        waterGauge.Update(gameTime);
        waterGauge.SetWaterLevel(gameplayScreen.GetWaterLevel());
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        scoreDisplay.Draw(spriteBatch);
        reloadTimer.Draw(spriteBatch);
        repairTimer.Draw(spriteBatch);
        bucketTimer.Draw(spriteBatch);
        waterGauge.Draw(spriteBatch);
        damageDisplay.Draw(spriteBatch);
        leftCannon.Draw(spriteBatch);
        rightCannon.Draw(spriteBatch);
    }

    private void OnReloadTimerFinished()
    {
        playerBoat.FinishReload();
    }
    public void TriggerRepair()
    {
        if (CanTriggerAction())
        {
            repairTimer.Trigger();
            damageDisplay.StartRepair();
        }
    }

    public void TriggerReload()
    {
        if (CanTriggerAction())
        {
            reloadTimer.Trigger();
        }
    }
    public bool TriggerBucket()
    {
        if (CanTriggerAction())
        {
            bucketTimer.Trigger();
            return true;
        }
        else return false;
    }
    private bool CanTriggerAction()
    {
        return reloadTimer.IsComplete() && repairTimer.IsComplete() && bucketTimer.IsComplete();
    }

    public void UpdateDamageDisplay()
    {
        damageDisplay.UpdateHoles(gameplayScreen.GetHoleCount());
    }

    public void UpdateScore()
    {
        scoreDisplay.SetScore(GlobalInfo.Score);
    }

    public void SetWaterLevel(float level)
    {
        waterGauge.SetWaterLevel(level);
    }

    public bool IsWaterGaugeFull(){
        return waterGauge.IsFull();
    }
}