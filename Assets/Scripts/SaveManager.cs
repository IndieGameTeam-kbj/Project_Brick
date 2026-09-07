using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private const string MuteKey = "IsMute";
    private const string HasSaveKey = "HasSaveData";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveMute(bool isMuted)
    {
        int value = isMuted ? 1 : 0;

        PlayerPrefs.SetInt(MuteKey, value);
        PlayerPrefs.Save();
    }

    public bool LoadMute()
    {
        int value = PlayerPrefs.GetInt(MuteKey, 0);

        return value == 1;
    }

    public void SetHasSaveData(bool hasSaveData)
    {
        int value = hasSaveData ? 1 : 0;

        PlayerPrefs.SetInt(HasSaveKey, value);
        PlayerPrefs.Save();
    }

    public bool HasSaveData()
    {
        int value = PlayerPrefs.GetInt(HasSaveKey, 0);

        return value == 1;
    }

    public void DeleteGameSave()
    {
        // 음소거 설정은 지우지 않고 게임 저장 여부만 지운다.
        PlayerPrefs.DeleteKey(HasSaveKey);
        PlayerPrefs.Save();
    }
}