using System;
using UnityEngine;
using DG.Tweening;

public class BrickAnimationController : MonoBehaviour
{
    private float _spawnDuration = 0.5f;
    private float _spawnRotation = 360.0f;
    private float _destroyDuration = 0.5f;
    private float _destroyScale = 1.1f;
    private Vector3 _originalScale;

    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    public void PlaySpawnAnimation(Vector3 targetPosition, Action onComplete)
    {
        transform.DOKill();

        Vector3 startPosition = transform.position;
        Vector3 endPosition = targetPosition;

        transform.localScale = _originalScale;
        transform.localRotation = Quaternion.identity;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(endPosition, _spawnDuration).SetEase(Ease.OutCubic));
        sequence.OnComplete(() =>
        {
            transform.position = targetPosition;
            transform.localRotation = Quaternion.identity;
            onComplete?.Invoke();
        });

        DOVirtual.Float(0.0f, 1.0f, _spawnDuration, value =>
        {
            float rotationAmount;

            if (value < 0.5f)
            {
                float t = value / 0.5f;
                rotationAmount = Mathf.Lerp(0.0f, _spawnRotation, t);
            }
            else
            {
                float t = (value - 0.5f) / 0.5f;
                rotationAmount = Mathf.Lerp(_spawnRotation, 0.0f, t);
            }

            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -rotationAmount);
        })
        .SetEase(Ease.OutCubic);
    }

    public void PlayDestroyAnimation(Action OnComplete)
    {
        transform.DOKill();

        transform.DOScale(_originalScale * _destroyScale, _destroyDuration * 0.3f).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                transform.DOScale(Vector3.zero, _destroyDuration * 0.7f).SetEase(Ease.InBack)
                    .OnComplete(() =>
                    {
                        OnComplete?.Invoke();
                    });
            });

        transform.DORotate(new Vector3(0.0f, 0.0f, UnityEngine.Random.Range(-8.0f, 8.0f)), _destroyDuration).SetEase(Ease.InQuad);
    }

}