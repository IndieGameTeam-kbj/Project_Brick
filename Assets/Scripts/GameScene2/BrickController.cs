using System;
using UnityEngine;

public enum BrickState
{
    Preparing,
    Prepared,
    Placing,
    Placed,
    Destroying,
}

public enum BrickType
{
    Horizontal,
    Vertical,
    DiagonalUpRight,
    DiagonalDownRight,
}

public class BrickController : MonoBehaviour
{
    [SerializeField] private BrickType[] _types;

    private BrickAnimationController _animationController;
    private float _originalZ;
    private Vector3 _originalPosition;
    private Vector3 _originalScale;
    private float _liftZ = -0.5f;
    private float _liftScale = 1.05f;
    private BrickState _state;

    public BrickType[] Types => _types;
    public BrickAnimationController AnimationController => _animationController;
    public float DragZ => _originalZ + _liftZ;
    public BrickState State => _state;

    public event Action Destroyed;

    public void Init(Vector3 targetPosition)
    {
        _animationController = GetComponent<BrickAnimationController>();
        _originalZ = transform.position.z;
        _originalPosition = targetPosition;
        _originalScale = transform.localScale;
        _state = BrickState.Preparing;
        _animationController.PlaySpawnAnimation(targetPosition, OnSpawnAnimationComplete);
    }

    public void BeginDrag(Vector2 worldPosition)
    {
        transform.localScale = _originalScale * _liftScale;
        Drag(worldPosition);
        _state = BrickState.Placing;
    }

    public void Drag(Vector2 worldPosition)
    {
        transform.position = new Vector3(worldPosition.x, worldPosition.y, _originalZ + _liftZ);
    }

    public void EndDrag()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, _originalZ);
        transform.localScale = _originalScale;
    }

    public void Place(Vector3 position)
    {
        transform.position = position;
        transform.localScale = _originalScale;
        _state = BrickState.Placed;
    }

    public void CancelDrag()
    {
        transform.position = _originalPosition;
        transform.localScale = _originalScale;
        _state = BrickState.Prepared;
    }

    public void Destroy()
    {
        _state = BrickState.Destroying;
        _animationController.PlayDestroyAnimation(OnDestroyAnimationComplete);
    }

    private void OnSpawnAnimationComplete()
    {
        _state = BrickState.Prepared;
    }

    private void OnDestroyAnimationComplete()
    {
        Destroyed?.Invoke();
        Destroy(gameObject);
    }

}
