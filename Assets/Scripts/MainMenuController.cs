using System.Collections;
using TMPro;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private LogoAlphabet[] _logoAlphabets = new LogoAlphabet[6];
    [SerializeField] private TMP_Text _bestScoreText;
    [SerializeField] private TMP_Text _playButtonText;
    [SerializeField] private SoundToggle _soundToggle;
    
    [SerializeField] private float _logoAnimationDelay = 0.05f;

    private void Start()
    {
        StartCoroutine(PlayLogoAnimation());
    }

    public void Init()
    {
        _bestScoreText.text = SaveManager.Instance.LoadBestScore().ToString();
        InitPlayButton();
        _soundToggle.Init();
    }

    private void InitPlayButton()
    {
        bool hasSaveData = SaveManager.Instance.HasSaveData();

        _playButtonText.text = hasSaveData
            ? "Continue"
            : "New Game";
    }

    private IEnumerator PlayLogoAnimation()
    {
        for (int i = 0; i < _logoAlphabets.Length; i++)
        {
            _logoAlphabets[i].Init();
        }

        for (int i = 0; i < _logoAlphabets.Length; i++)
        {
            _logoAlphabets[i].PlayAnimation();
            yield return new WaitForSeconds(_logoAnimationDelay);
        }
    }

    public void OnClickPlayButton()
    {
        SoundManager.Instance.PlayButtonClick();

        if (SaveManager.Instance.HasSaveData())
        {
            GameManager.Instance.ContinueGame();
        }
        else
        {
            GameManager.Instance.StartNewGame();
        }
    }
}
