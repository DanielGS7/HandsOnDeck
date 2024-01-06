using Microsoft.Xna.Framework;
using HandsOnDeck.Classes.UI;
using HandsOnDeck.Classes.Managers;
using HandsOnDeck.Classes.UI.Screens;
using HandsOnDeck.Enums;
using HandsOnDeck;

public class PauseScreen : UIScreen
{
    private Button resumeButton;
    private Button settingsButton;
    private Button exitButton;

    public override void Initialize()
    {
        base.Initialize();
        resumeButton = new Button("Resume", new Vector2(100, 100), ResumeGame);
        settingsButton = new Button("Settings", new Vector2(100, 200), OpenSettings);
        exitButton = new Button("Exit to Main Menu", new Vector2(100, 300), ExitToMainMenu);

        AddUIElement(resumeButton);
        AddUIElement(settingsButton);
        AddUIElement(exitButton);
    }

    internal override void LoadContent()
    {

    }
    private void ResumeGame()
    {
        GameStateManager.Instance.ChangeState(GameState.Game);
    }

    private void OpenSettings()
    {
        GameStateManager.Instance.ChangeState(GameState.Settings);
    }

    private void ExitToMainMenu()
    {
        GameStateManager.Instance.ChangeState(GameState.Start);
    }
}
