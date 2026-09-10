using UnityEngine;

public class PausePopup : MonoBehaviour
{
    [SerializeField] private SoundToggle _soundToggle;

    public void Init()
    {
        _soundToggle.Init();
    }

    public void OnClickHomeButton()
    {
        SoundManager.Instance.PlayButtonClick();
        GameManager.Instance.Home();
    }

    public void OnClickRestartButton()
    {
        SoundManager.Instance.PlayButtonClick();
        GameManager.Instance.Restart();
    }

    public void OnClickResumeButton()
    {
        SoundManager.Instance.PlayButtonClick();
        GameManager.Instance.Resume();
    }

}
