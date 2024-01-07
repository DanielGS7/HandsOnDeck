using Microsoft.Xna.Framework;
using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Classes.UI.Screens;
using HandsOnDeck.Enums;

public class GameOverScreen : UIScreen
{
    private Button restartButton;
    private Button exitButton;

    public void Initialize()
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
        GameStateManager.GetInstance.ChangeState(GameState.Game);
    }

    private void ExitToMainMenu()
    {
        GameStateManager.GetInstance.ChangeState(GameState.Start);
    }
}
