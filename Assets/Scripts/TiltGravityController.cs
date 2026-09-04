using UnityEngine;
using UnityEngine.InputSystem;

public class TiltGravityController : MonoBehaviour
{
    [SerializeField] private float gravityStrength = 15f;
    [SerializeField] private float deadZone = 0.05f;

    private Vector2 tiltInput;

    private void OnEnable()
    {
        if (Accelerometer.current != null)
        {
            InputSystem.EnableDevice(Accelerometer.current);
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        // 키를 놓으면 기본적으로 아래쪽 중력
        float x = 0f;
        float y = -1f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed ||
                Keyboard.current.leftArrowKey.isPressed)
            {
                x = -1f;
            }

            if (Keyboard.current.dKey.isPressed ||
                Keyboard.current.rightArrowKey.isPressed)
            {
                x = 1f;
            }

            if (Keyboard.current.wKey.isPressed ||
                Keyboard.current.upArrowKey.isPressed)
            {
                y = 1f;
            }

            if (Keyboard.current.sKey.isPressed ||
                Keyboard.current.downArrowKey.isPressed)
            {
                y = -1f;
            }
        }

        tiltInput = new Vector2(x, y);
#else
        if (Accelerometer.current != null)
        {
            Vector3 acceleration =
                Accelerometer.current.acceleration.ReadValue();

            tiltInput = new Vector2(
                acceleration.x / 9.81f,
                acceleration.y / 9.81f
            );

            tiltInput = Vector2.ClampMagnitude(tiltInput, 1f);
        }
#endif

        if (Mathf.Abs(tiltInput.x) < deadZone)
        {
            tiltInput.x = 0f;
        }

        if (Mathf.Abs(tiltInput.y) < deadZone)
        {
            tiltInput.y = 0f;
        }
    }

    private void FixedUpdate()
    {
        Physics2D.gravity = tiltInput * gravityStrength;
    }
}