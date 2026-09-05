using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] Board _board;
    [SerializeField] private TMP_Text _scoreText;

    private int _score = 0;

    public static ScoreManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void AddScore()
    {
        _score++;
        _scoreText.text = _score.ToString();
    }

    private void OnEnable()
    {
        _board.BrickDestroyed += AddScore;
    }

    private void OnDisable()
    {
        _board.BrickDestroyed -= AddScore;
    }

}
