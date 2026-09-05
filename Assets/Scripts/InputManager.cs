using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputActions _inputActions;

    private InputAction _pointAction;
    private InputAction _clickAction;

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
        _pointAction = _inputActions.FindAction("Point");
        _clickAction = _inputActions.FindAction("Click");
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
