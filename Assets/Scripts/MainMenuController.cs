using DG.Tweening.Core.Easing;
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
        //GameManager.Instance.StartNewGame();
        SoundManager.Instance.PlayButtonClick();
    }

    public void OnClickContinueButton()
    {
        Debug.Log($"Continue");
        SoundManager.Instance.PlayButtonClick();
    }

    public void OnSoundToggleChanged(bool isSoundOn)
    {
        _isMute = !isSoundOn;

        ApplySoundSetting();
        SaveManager.Instance.SaveMute(_isMute);

        if (!_isMute)
        {
            SoundManager.Instance.PlayButtonClick();
        }
    }

    private void ApplySoundSetting()
    {
        SoundManager.Instance.SetMute(_isMute);

        Image image = _soundToggle.GetComponent<Image>();
        image.sprite = _isMute ? _muteImage : _soundImage;
    }

}
