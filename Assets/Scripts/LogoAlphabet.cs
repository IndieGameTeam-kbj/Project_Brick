using DG.Tweening;
using UnityEngine;

public class LogoAlphabet : MonoBehaviour
{
    [SerializeField] private float _startOffset = 1500.0f;
    [SerializeField] private float _animationDuration = 1.5f;
    [SerializeField] private float _rotation = 360.0f;

    private RectTransform _rectTransform;
    private Vector2 _originPosition;
    private Quaternion _originRotation;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originPosition = _rectTransform.anchoredPosition;
        _originRotation = _rectTransform.localRotation;
    }

    public void Init()
    {
        _rectTransform.anchoredPosition = _originPosition + Vector2.right * _startOffset;
        _rectTransform.localRotation = _originRotation;
    }

    public void PlayAnimation()
    {
        _rectTransform.DOAnchorPos(_originPosition, _animationDuration).SetEase(Ease.OutQuart);

        DOVirtual.Float(0.0f, 1.0f, _animationDuration, value =>
        {
            float rotationAmount;

            if (value < 0.5f)
            {
                float t = value / 0.5f;
                rotationAmount = Mathf.Lerp(0.0f, _rotation, t);
            }
            else
            {
                float t = (value - 0.5f) / 0.5f;
                rotationAmount = Mathf.Lerp(_rotation, 0.0f, t);
            }

            _rectTransform.localRotation = _originRotation * Quaternion.Euler(0.0f, 0.0f, -rotationAmount);
        })
        .SetEase(Ease.OutQuart);
    }
    
}
