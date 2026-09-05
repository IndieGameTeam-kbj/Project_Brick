using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private BrickSpawner _brickSpawner;
    [SerializeField] private Board _board;

    private BrickController[] _preparedBricks;
    private Camera _mainCamera;
    private BrickController _draggingBrick;
    private float _dragScreenYOffset = 100.0f;

    public static BoardManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _mainCamera = Camera.main;
    }

    private void Start()
    {
        _preparedBricks = _brickSpawner.SpawnBricks();
    }

    private void Update()
    {
        if (InputManager.Instance.IsPointerPressed)
        {
            TryBeginDrag();
        }

        if (_draggingBrick != null && InputManager.Instance.IsPointerHeld)
        {
            Vector2 dragWorldPosition = GetDragWorldPosition();
            _draggingBrick.Drag(dragWorldPosition);
        }

        if (_draggingBrick != null && InputManager.Instance.IsPointerReleased)
        {
            TryPlaceBrick();
            _draggingBrick = null;
        }
    }

    private void TryBeginDrag()
    {
        if (_draggingBrick != null) return;

        Vector2 pointerWorldPosition = ScreenToWorldPosition(InputManager.Instance.PointerScreenPosition, 0.0f);

        Collider2D collider = Physics2D.OverlapPoint(pointerWorldPosition);
        if (collider == null) return;

        BrickController brick = collider.GetComponent<BrickController>();
        if (brick == null || brick.State != BrickState.Prepared) return;
        
        _draggingBrick = brick;
        Vector2 dragWorldPosition = GetDragWorldPosition();
        _draggingBrick.BeginDrag(dragWorldPosition);
    }

    private Vector2 GetDragWorldPosition()
    {
        Vector2 screenPosition = InputManager.Instance.PointerScreenPosition;
        screenPosition.y += _dragScreenYOffset;
        return ScreenToWorldPosition(screenPosition, _draggingBrick.DragZ);
    }

    private Vector2 ScreenToWorldPosition(Vector2 screenPosition, float worldZ)
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);
        Plane plane = new Plane(Vector3.forward, new Vector3(0.0f, 0.0f, worldZ));

        if (!plane.Raycast(ray, out float distance))
        {
            return Vector2.zero;
        }

        Vector3 worldPosition = ray.GetPoint(distance);
        return new Vector2(worldPosition.x, worldPosition.y);
    }

    private void TryPlaceBrick()
    {
        Vector2 dropPosition = GetDragWorldPosition();

        if (!_board.TryPlaceBrick(_draggingBrick, dropPosition))
        {
            _draggingBrick.CancelDrag();
            return;
        }

        RemovePreparedBrick(_draggingBrick);
    }

    private void RemovePreparedBrick(BrickController brick)
    {
        for (int i = 0; i < _preparedBricks.Length; i++)
        {
            if (_preparedBricks[i] != brick) continue;

            _preparedBricks[i] = null;
            break;
        }

        if (AreAllBricksPlaced())
        {
            _preparedBricks = _brickSpawner.SpawnBricks();
        }
    }

    private bool AreAllBricksPlaced()
    {
        foreach (BrickController brick in _preparedBricks)
        {
            if (brick != null)
            {
                return false;
            }
        }

        return true;
    }

}
