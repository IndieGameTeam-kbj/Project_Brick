using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private LogoAlphabet[] _logoAlphabets = new LogoAlphabet[6];
    [SerializeField] private TMP_Text _bestScoreText;
    [SerializeField] private GameObject _newGameButton;
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private Toggle _soundToggle;
    [SerializeField] private Sprite _soundImage;
    [SerializeField] private Sprite _muteImage;

    [SerializeField] private float _logoAnimationDelay = 0.05f;
    private bool _isMute;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        InitSound();
        InitContinueButton();

        StartCoroutine(PlayLogoAnimation());
    }

    public void Refresh()
    {
        _bestScoreText.text = ScoreManager.Instance.BestScore.ToString();
    }

    private void InitContinueButton()
    {
        bool hasSaveData = SaveManager.Instance.HasSaveData();

        _continueButton.SetActive(hasSaveData);
    }

    private void InitSound()
    {
        _isMute = SaveManager.Instance.LoadMute();

        _soundToggle.SetIsOnWithoutNotify(!_isMute);

        ApplySoundSetting();
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
        GameManager.Instance.StartNewGame();
        SaveManager.Instance.DeleteGameSave();
    }

    public void OnClickContinueButton()
    {
        SoundManager.Instance.PlayButtonClick();
    }

    public void OnSoundToggleChanged()
    {
        _isMute = !_soundToggle.isOn;

        ApplySoundSetting();
        SaveManager.Instance.SaveMute(_isMute);

        if (!_isMute)
        {
            StartCoroutine(PlayClickSound());
        }
    }

    private void ApplySoundSetting()
    {
        SoundManager.Instance.SetMute(_isMute);

        Image image = _soundToggle.GetComponent<Image>();
        image.sprite = _isMute ? _muteImage : _soundImage;
    }

    private IEnumerator PlayClickSound()
    {
        yield return null;
        SoundManager.Instance.PlayButtonClick();
    }

}
