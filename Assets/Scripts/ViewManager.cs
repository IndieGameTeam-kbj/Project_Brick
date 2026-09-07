using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ViewManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _mainMenu;
    [SerializeField] private MainMenuController _mainMenuController;
    [SerializeField] private GameObject _game;
    [SerializeField] private GameObject _dimBackGround;
    [SerializeField] private GameObject _gameOverPopup;
    [SerializeField] private Image _screenTransition;

    private float _transitionDuration = 0.4f;
    private float _popupDuration = 0.2f;
    private float _popupStartScale = 0.9f;

    public static ViewManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SetTransitionAlpha(0.0f);
    }

    public void ShowMainMenu()
    {
        SetMainMenuActive(true);
        _game.SetActive(false);
        _dimBackGround.SetActive(false);
        _gameOverPopup.SetActive(false);
        _mainMenuController.Refresh();
    }

    public void ShowGame()
    {
        SetMainMenuActive(false);
        _game.SetActive(true);
        _dimBackGround.SetActive(false);
        _gameOverPopup.SetActive(false);
    }

    public void ShowGameOver()
    {
        _dimBackGround.SetActive(true);
        _gameOverPopup.SetActive(true);

        GameOverPopup popup = _gameOverPopup.GetComponent<GameOverPopup>();
        popup.Init();
        PlayPopupOpenAnimation(popup.GetComponent<RectTransform>(), () =>
        {
            popup.Refresh();
        });
    }

    public void Transition(Action onSwap, Action onComplete)
    {
        _screenTransition.DOKill();
        SetTransitionAlpha(0.0f);
        _screenTransition.DOFade(1.0f, _transitionDuration).SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                onSwap?.Invoke();
                _screenTransition.DOFade(0.0f, _transitionDuration).SetEase(Ease.InOutQuad)
                    .OnComplete(() =>
                    {
                        onComplete?.Invoke();
                    });
            });
    }

    private void PlayPopupOpenAnimation(RectTransform popup, Action onComplete)
    {
        popup.DOKill();
        popup.localScale = Vector3.one * _popupStartScale;
        popup.DOScale(Vector3.one, _popupDuration).SetEase(Ease.OutBack, 1.2f).SetUpdate(true)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }

    private void SetMainMenuActive(bool active)
    {
        foreach (GameObject menuObject in _mainMenu)
        {
            menuObject.SetActive(active);
        }
    }

    private void SetTransitionAlpha(float alpha)
    {
        Color color = _screenTransition.color;
        color.a = alpha;
        _screenTransition.color = color;
    }

}
