using Microsoft.Xna.Framework;
using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Classes.UI.Screens;
using HandsOnDeck.Enums;
using HandsOnDeck.Classes.MonogameAccessibility;
using HandsOnDeck;
using Microsoft.Xna.Framework.Graphics;

public class GameOverScreen : UIScreen
{
    private SpriteFont titleFont;
    private string gameOverText = "GAME OVER :(";
    private string winText = " YOU WON!!!";
    private Button quitButton;
    private Button exitButton;
    private bool hasWon = true;

    private static GameOverScreen instance;

    public static GameOverScreen GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameOverScreen();
                instance.Initialize();
            }
            return instance;
        }
    }
    public void Initialize()
    {
        base.Initialize();
        quitButton = new Button("Quit", new Vector2(1000, 850), QuitGame);
        exitButton = new Button("Exit to Main Menu", new Vector2(1000, 650), ExitToMainMenu);

        AddUIElement(quitButton);
        AddUIElement(exitButton);
    }

    internal void HasWon(bool didWin)
    {
        hasWon = didWin;
    }

    internal override void LoadContent()
    {
        quitButton.LoadContent();
        exitButton.LoadContent();
    }
    public override void Draw(GameTime gameTime)
    {
        titleFont = Game1.DefaultFont;
        float scale = 5.0f;
        Vector2 textSize = titleFont.MeasureString(gameOverText) * scale;
        Vector2 textPosition = new Vector2(1000, 400) - textSize / 2;
        Color goldColor = new Color(255, 215, 0);
        Color redColor = new Color(136, 8, 8);
        if (hasWon)
        {
            SpriteBatchManager.Instance.DrawString(titleFont, winText, textPosition, goldColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            base.Draw(gameTime);
        }
        else
        {
            SpriteBatchManager.Instance.DrawString(titleFont, gameOverText, textPosition, redColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            base.Draw(gameTime);
        }
    }

    private void QuitGame()
    {
        System.Environment.Exit(0);
    }

    private void ExitToMainMenu()
    {
        GameStateManager.GetInstance.ChangeState(GameState.Start);
    }
}
