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
        GameManager.Instance.Home();
    }

    public void OnClickRestartButton()
    {
        GameManager.Instance.Restart();
    }

    public void OnClickResumeButton()
    {
        GameManager.Instance.Resume();
    }

}
