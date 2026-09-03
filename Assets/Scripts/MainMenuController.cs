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

    private float _logoAnimationDelay = 0.5f;
    private bool _isMute = false;

    private void Awake()
    {
        _continueButton.SetActive(false);
        StartCoroutine(PlayLogoAnimation());
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
        Debug.Log($"New Game");
    }

    public void OnClickContinueButton()
    {
        Debug.Log($"Continue");
    }

    public void OnClickSoundToggle()
    {
        _isMute = !_isMute;
        Image image = _soundToggle.GetComponent<Image>();

        if (_isMute)
        {
            image.sprite = _muteImage;
        }
        else
        {
            image.sprite = _soundImage;
        }
    }

}
