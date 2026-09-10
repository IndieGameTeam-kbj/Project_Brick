using UnityEngine;

public enum GameState
{
    MainMenu,
    Playing,
    Pause,
    GameOver,
}

public class GameManager : MonoBehaviour
{
    private GameState _state;
    private bool _hasCurrentGame;
    public bool CanContinue =>
        _hasCurrentGame || SaveManager.Instance.HasSaveData();

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        int targetWidth = 1080;
        int targetHeight = (int)(((float)Screen.height / Screen.width) * targetWidth);
        Screen.SetResolution(targetWidth, targetHeight, true);
    }

    private void Start()
    {
        ScoreManager.Instance.Init();
        ChangeState(GameState.MainMenu);
    }

    public void StartNewGame()
    {
        SoundManager.Instance.PlaySceneTransition();
        ViewManager.Instance.Transition(
            () =>
            {
                BoardManager.Instance.Reset();
                ScoreManager.Instance.Reset();
                ChangeState(GameState.Playing);
            },
            () =>
            {
                BoardManager.Instance.StartGame();
            }
        );
    }

    public void ContinueGame()
    {
        SoundManager.Instance.PlaySceneTransition();

        ViewManager.Instance.Transition(
            () =>
            {
                ChangeState(GameState.Playing);
            },
            () =>
            {
                // 새 블록을 생성하거나 보드를 Reset하지 않는다.
            }
        );
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }

    public void Home()
    {
        ChangeState(GameState.MainMenu);
    }

    public void Restart()
    {
        StartNewGame();
    }

    public void Resume()
    {
        ChangeState(GameState.Playing);
    }

    public void OnClickPauseButton()
    {
        SoundManager.Instance.PlayButtonClick();
        ChangeState(GameState.Pause);
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
                ViewManager.Instance.ShowGame();
                break;

            case GameState.Pause:
                Time.timeScale = 0.0f;
                ViewManager.Instance.ShowPause();
                break;

            case GameState.GameOver:
                Time.timeScale = 0.0f;
                SoundManager.Instance.PlayGameOver();
                ViewManager.Instance.ShowGameOver();
                break;
        }
    }

    private void Update()
    {
        if (InputManager.Instance.IsBackPressed)
        {
            switch (_state)
            {
                case GameState.MainMenu:
                    Application.Quit();
                    break;

                case GameState.Playing:
                    OnClickPauseButton();
                    break;

                case GameState.Pause:
                    Resume();
                    break;

                case GameState.GameOver:
                    Home();
                    break;
            }
        }
    }

}
