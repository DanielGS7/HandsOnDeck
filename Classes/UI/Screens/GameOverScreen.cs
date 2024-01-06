using Microsoft.Xna.Framework;
using HandsOnDeck.Classes.UI;
using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Classes.UI.Screens;
using HandsOnDeck.Enums;
using HandsOnDeck;

public class GameOverScreen : UIScreen
{
    private Button restartButton;
    private Button exitButton;

    public override void Initialize()
    {
        base.Initialize();

        restartButton = new Button("Restart", new Vector2(100, 100), RestartGame);
        exitButton = new Button("Exit to Main Menu", new Vector2(100, 200), ExitToMainMenu);

        AddUIElement(restartButton);
        AddUIElement(exitButton);
    }

    internal override void LoadContent()
    {

    }

    private void RestartGame()
    {
        // Restart game logic
        GameStateManager.Instance.ChangeState(GameState.Game);
    }

    private void ExitToMainMenu()
    {
        GameStateManager.Instance.ChangeState(GameState.Start);
    }
}
