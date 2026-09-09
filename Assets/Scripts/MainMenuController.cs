using System.Collections;
using TMPro;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private LogoAlphabet[] _logoAlphabets = new LogoAlphabet[6];
    [SerializeField] private TMP_Text _bestScoreText;
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private SoundToggle _soundToggle;
    
    [SerializeField] private float _logoAnimationDelay = 0.05f;

    private void Start()
    {
        StartCoroutine(PlayLogoAnimation());
    }

    public void Init()
    {
        _bestScoreText.text = ScoreManager.Instance.BestScore.ToString();
        InitContinueButton();
        _soundToggle.Init();
    }

    private void InitContinueButton()
    {
        bool hasSaveData = SaveManager.Instance.HasSaveData();
        _continueButton.SetActive(hasSaveData);
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

    public void OnClickNewGameButton()
    {
        SoundManager.Instance.PlayButtonClick();
        SaveManager.Instance.DeleteGameSave();
        GameManager.Instance.StartNewGame();
    }

    public void OnClickContinueButton()
    {
        SoundManager.Instance.PlayButtonClick();
    }

}
