using UnityEngine;

public enum GameState
{
    MainMenu,
    Playing,
    GameOver,
}

public class GameManager : MonoBehaviour
{
    private GameState _state;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        ChangeState(GameState.MainMenu);
    }

    public void StartNewGame()
    {
        ChangeState(GameState.Playing);
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }

    public void OnClickHomeButton()
    {
        ChangeState(GameState.MainMenu);
    }

    public void OnClickRestartButton()
    {
        ChangeState(GameState.Playing);
    }

    private void ChangeState(GameState state)
    {
        _state = state;

        switch (_state)
        {
            case GameState.MainMenu:
                Time.timeScale = 1.0f;
                ViewManager.Instance.ShowMainMenu();
                break;

            case GameState.Playing:
                Time.timeScale = 1.0f;
                SoundManager.Instance.PlaySceneTransition();
                ViewManager.Instance.Transition(() =>
                    {
                        ViewManager.Instance.ShowGame();
                        BoardManager.Instance.Reset();
                        ScoreManager.Instance.Reset();
                    },() =>
                    {
                        BoardManager.Instance.StartGame();
                    });
                break;

            case GameState.GameOver:
                Time.timeScale = 0.0f;
                SoundManager.Instance.PlayGameOver();
                ViewManager.Instance.ShowGameOver();
                break;
        }
    }

}
