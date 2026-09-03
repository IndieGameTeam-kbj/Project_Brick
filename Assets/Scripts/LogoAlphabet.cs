using DG.Tweening;
using UnityEngine;

public class LogoAlphabet : MonoBehaviour
{
    private const float StartOffset = 1500.0f;
    private const float AnimationDuration = 1.5f;
    private const float Rotation = 360.0f;

    private RectTransform _rectTransform;
    private Vector2 _originPosition;
    private Vector3 _originRotation;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originPosition = _rectTransform.anchoredPosition;
        _originRotation = _rectTransform.localEulerAngles;
    }

    public void Init()
    {
        _rectTransform.anchoredPosition = _originPosition + Vector2.right * StartOffset;
        _rectTransform.localRotation = Quaternion.identity;
    }

    public void PlayAnimation()
    {
        _rectTransform.DOAnchorPos(_originPosition, AnimationDuration)
            .SetEase(Ease.OutQuart);

        _rectTransform.DOLocalRotate(_originRotation + Vector3.forward * Rotation, AnimationDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuart);
    }
    
}
