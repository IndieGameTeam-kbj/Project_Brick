using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputActions _inputActions;

    private InputAction _pointAction;
    private InputAction _clickAction;
    private Camera _mainCamera;

    public static InputManager Instance { get; private set; }

    public Vector2 PointerScreenPosition => _pointAction.ReadValue<Vector2>();
    public bool IsPointerPressed => _clickAction.WasPressedThisFrame();
    public bool IsPointerHeld => _clickAction.IsPressed();
    public bool IsPointerReleased => _clickAction.WasReleasedThisFrame();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _inputActions = new InputActions();
        _mainCamera = Camera.main;
        _pointAction = _inputActions.FindAction("Point");
        _clickAction = _inputActions.FindAction("Click");
    }

    public Vector2 ScreenToWorldPosition(Vector2 screenPosition, float worldZ)
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);
        Plane plane = new Plane(Vector3.forward, new Vector3(0f, 0f, worldZ));

        if (!plane.Raycast(ray, out float distance))
        {
            return Vector2.zero;
        }

        Vector3 worldPosition = ray.GetPoint(distance);

        return new Vector2(worldPosition.x, worldPosition.y);
    }

    public Vector2 GetPointerWorldPosition(float worldZ)
    {
        return ScreenToWorldPosition(PointerScreenPosition, worldZ);
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

}
