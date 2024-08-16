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
    private const float RepairTime = 6f;
    private const float FadeOutTime = 1f;

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
                    hole.IsFadingOut = true;
                    hole.IsRepairing = false;
                    hole.FadeProgress = 0f;
                }
            }
            if (hole.IsFadingOut)
            {
                hole.FadeProgress += deltaTime / FadeOutTime;
                if (hole.FadeProgress >= 1f)
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

        foreach (var hole in holes)
        {
            Vector2 holePos = position + (hole.Position - origin) * backgroundScale;
            Color holeColor = Color.White * (1f - hole.FadeProgress);
            spriteBatch.Draw(holeTexture, holePos, null, holeColor, rotation, new Vector2(holeTexture.Width / 2f, holeTexture.Height / 2f), backgroundScale, SpriteEffects.None, 0f);

            if (hole.IsRepairing || hole.IsFadingOut)
            {
                float plankAlpha = hole.IsRepairing ? hole.RepairProgress : (1f - hole.FadeProgress);
                Color plankColor = Color.White * plankAlpha;
                spriteBatch.Draw(plankTexture, holePos, null, plankColor, rotation, new Vector2(plankTexture.Width / 2f, plankTexture.Height / 2f), backgroundScale, SpriteEffects.None, 0f);
            }
        }
    }

    public void StartRepair()
    {
        var holeToRepair = holes.Find(h => !h.IsRepairing && !h.IsFadingOut);
        if (holeToRepair != null)
        {
            holeToRepair.IsRepairing = true;
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
        holes.Add(new DamageHole(new Vector2(
            new Random().Next(0, hullBackgroundTexture.Width),
            new Random().Next(0, hullBackgroundTexture.Height)
        )));
    }

    private class DamageHole
    {
        public Vector2 Position { get; set; }
        public bool IsRepairing { get; set; }
        public bool IsFadingOut { get; set; }
        public float RepairProgress { get; set; }
        public float FadeProgress { get; set; }

        public DamageHole(Vector2 position)
        {
            Position = position;
            IsRepairing = false;
            IsFadingOut = false;
            RepairProgress = 0f;
            FadeProgress = 0f;
        }
    }
}