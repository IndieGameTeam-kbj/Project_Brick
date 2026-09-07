using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] Board _board;
    [SerializeField] private TMP_Text _scoreText;

    private int _score = 0;
    private int _bestScore = 0;
    private int _prevBestScore = 0;
    private bool _isNewBestScore = false;
    private float _punchScale = 1.2f;
    private float _punchDuration = 0.2f;
    private float _countDuration = 0.1f;

    public int Score => _score;
    public int BestScore => _bestScore;
    public int PrevBestScore => _prevBestScore;
    public bool IsNewBestScore => _isNewBestScore;

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

    public void Reset()
    {
        _score = 0;
        _scoreText.text = _score.ToString();
        _isNewBestScore = false;
    }

    private void AddScore(int amount)
    {
        int prevScore = _score;
        _score += amount;
        PlayScoreAnimation(prevScore, _score);
    }

    private void PlayScoreAnimation(int previousScore, int targetScore)
    {
        Transform target = _scoreText.transform;

        target.DOKill();
        target.localScale = Vector3.one;

        DOTween.To(() => previousScore, value =>
            {
                _scoreText.text = value.ToString();
            },
            targetScore, _countDuration
        )
        .SetEase(Ease.OutQuad);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(target.DOScale(Vector3.one * _punchScale, _punchDuration * 0.5f).SetEase(Ease.OutQuad));
        sequence.Append(target.DOScale(Vector3.one * 0.95f, _punchDuration * 0.2f).SetEase(Ease.InOutQuad));
        sequence.Append(target.DOScale(Vector3.one, _punchDuration * 0.3f).SetEase(Ease.OutQuad));
    }

    public void UpdateBestScore()
    {
        _prevBestScore = _bestScore;
        _isNewBestScore = _score > _bestScore;

        if (_isNewBestScore) _bestScore = _score;
    }

    private void OnEnable()
    {
        _board.LineDestroyed += AddScore;
    }

    private void OnDisable()
    {
        _board.LineDestroyed -= AddScore;
    }

}
