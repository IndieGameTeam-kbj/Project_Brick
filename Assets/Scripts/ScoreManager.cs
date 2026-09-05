using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] Board _board;
    [SerializeField] private TMP_Text _scoreText;

    private int _score = 0;
    private float _punchScale = 1.2f;
    private float _punchDuration = 0.2f;
    private float _punchRotation = 10.0f;

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

    private void AddScore(int amount)
    {
        _score += amount;
        _scoreText.text = _score.ToString();
        PlayScoreAnimation();
    }

    private void PlayScoreAnimation()
    {
        Transform target = _scoreText.transform;

        target.DOKill();
        target.localScale = Vector3.one;
        target.localRotation = Quaternion.identity;

        Sequence sequence = DOTween.Sequence();

        // 커지면서 왼쪽으로 기울기
        sequence.Append(target.DOScale(Vector3.one * _punchScale, _punchDuration * 0.6f).SetEase(Ease.OutQuad));
        sequence.Join(target.DOLocalRotate(new Vector3(0.0f, 0.0f, _punchRotation), _punchDuration * 0.6f).SetEase(Ease.OutQuad));

        // 빠르게 원래대로 복귀
        sequence.Append(target.DOScale(Vector3.one, _punchDuration * 0.4f).SetEase(Ease.InQuad));
        sequence.Join(target.DOLocalRotate(Vector3.zero,_punchDuration * 0.4f).SetEase(Ease.InQuad));
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
