using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck2.Classes;
using HandsOnDeck2.Classes.UI.Screens;
using System.Collections.Generic;
using System;

public class DamageDisplay : UIElement
{
    private Texture2D hullBackgroundTexture;
    private Texture2D holeTexture;
    private Texture2D plankTexture;
    private GameplayScreen gameplayScreen;
    private List<DamageHole> holes;
    private const int MaxHoles = 5;
    private const float RepairTime = 4f;
    private const float HoleSizeFactor = 0.7f;
    private Random random = new Random();

    public DamageDisplay(ContentManager content, Vector2 positionPercentage, Vector2 sizePercentage, GameplayScreen gameplayScreen) 
        : base(positionPercentage, sizePercentage, 0f)
    {
        this.gameplayScreen = gameplayScreen;
        hullBackgroundTexture = content.Load<Texture2D>("hull_background");
        holeTexture = content.Load<Texture2D>("hole");
        plankTexture = content.Load<Texture2D>("plank");
        holes = new List<DamageHole>();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        UpdateHoles(gameplayScreen.GetHoleCount());

        for (int i = holes.Count - 1; i >= 0; i--)
        {
            var hole = holes[i];
            if (hole.IsRepairing)
            {
                hole.RepairProgress += deltaTime / RepairTime;
                if (hole.RepairProgress >= 1f)
                {
                    holes.RemoveAt(i);
                    gameplayScreen.RepairHole();
                }
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Vector2 origin = new Vector2(hullBackgroundTexture.Width / 2f, hullBackgroundTexture.Height / 2f);
        float backgroundScale = Math.Min(size.X / hullBackgroundTexture.Width, size.Y / hullBackgroundTexture.Height);

        spriteBatch.Draw(hullBackgroundTexture, position, null, Color.White, rotation, origin, backgroundScale, SpriteEffects.None, 0f);

        Vector2 topLeft = position - (origin * backgroundScale);

        foreach (var hole in holes)
        {
            Vector2 holePos = topLeft + (hole.Position * backgroundScale);
            float holeScale = backgroundScale * HoleSizeFactor;

            spriteBatch.Draw(holeTexture, holePos, null, Color.White, hole.Rotation, new Vector2(holeTexture.Width / 2f, holeTexture.Height / 2f), holeScale, SpriteEffects.None, 0f);

            if (hole.IsRepairing)
            {
                spriteBatch.Draw(plankTexture, holePos, null, Color.White * hole.RepairProgress, hole.Rotation, new Vector2(plankTexture.Width / 2f, plankTexture.Height / 2f), holeScale, SpriteEffects.None, 0f);
            }
        }
    }

    public void StartRepair()
    {
        var holeToRepair = holes.Find(h => !h.IsRepairing);
        if (holeToRepair != null)
        {
            holeToRepair.IsRepairing = true;
            holeToRepair.RepairProgress = 0f;
        }
    }

    public void UpdateHoles(int holeCount)
    {
        while (holes.Count < holeCount && holes.Count < MaxHoles)
        {
            AddHole();
        }
        while (holes.Count > holeCount)
        {
            holes.RemoveAt(holes.Count - 1);
        }
    }

    private void AddHole()
    {
        float margin = HoleSizeFactor / 2f;
        float x = (float)random.NextDouble() * (1f - 2 * margin) + margin;
        float y = (float)random.NextDouble() * (1f - 2 * margin) + margin;
        float rotation = (float)random.NextDouble() * MathHelper.TwoPi;

        holes.Add(new DamageHole(new Vector2(x, y) * new Vector2(hullBackgroundTexture.Width, hullBackgroundTexture.Height), rotation));
    }

    private class DamageHole
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public bool IsRepairing { get; set; }
        public float RepairProgress { get; set; }

        public DamageHole(Vector2 position, float rotation)
        {
            Position = position;
            Rotation = rotation;
            IsRepairing = false;
            RepairProgress = 0f;
        }
    }
}