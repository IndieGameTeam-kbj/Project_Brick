using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    [SerializeField] private Sprite _soundImage;
    [SerializeField] private Sprite _muteImage;

    private Toggle _toggle;
    private bool _isMute;

    public void Awake()
    {
        _toggle = GetComponent<Toggle>();
    }

    public void Init()
    {
        _isMute = SaveManager.Instance.LoadMute();
        _toggle.SetIsOnWithoutNotify(!_isMute);
        Apply();
    }

    public void OnToggleChanged()
    {
        _isMute = !_toggle.isOn;

        Apply();
        SaveManager.Instance.SaveMute(_isMute);

        if (!_isMute)
        {
            StartCoroutine(PlayClickSound());
        }
    }

    private void Apply()
    {
        SoundManager.Instance.SetMute(_isMute);
        Image image = _toggle.GetComponent<Image>();
        image.sprite = _isMute ? _muteImage : _soundImage;
    }

    private IEnumerator PlayClickSound()
    {
        yield return null;
        SoundManager.Instance.PlayButtonClick();
    }

}
