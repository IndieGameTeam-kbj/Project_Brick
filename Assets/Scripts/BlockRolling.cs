using UnityEngine;

public class BlockRolling : MonoBehaviour
{
    [Header("회전 설정")]
    [SerializeField] private float rotationMultiplier = 80f; // 회전 속도를 결정하는 계수
    [SerializeField] private float minimumSpeed = 0.1f; // 회전이 멈추는 최소 속도
    [SerializeField] private float maxRotationSpeed = 360f; // 최대 회전 속도
    [SerializeField] private float rotationSmoothing = 8f; // 회전 속도 보간 계수

    private Rigidbody2D rb; // Rigidbody2D 컴포넌트 참조
    private float currentRotationSpeed; // 현재 회전 속도

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Rigidbody2D의 초당 속도를 가져옴
        Vector2 velocity = rb.linearVelocity;

        // 속도가 최소 속도보다 작으면 회전을 멈춤
        float rotationDirection;

        // 좌우 이동 중이면 좌우 방향을 기준으로 회전
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            // 오른쪽 이동 = 시계 방향
            rotationDirection = -Mathf.Sign(velocity.x);
        }
        else
        {
            // 상하 이동만 해도 회전 방향 생성
            rotationDirection = -Mathf.Sign(velocity.y);
        }

        // 타겟 회전 속도를 계산 (속도 * 회전 계수 * 회전 방향)
        float targetRotationSpeed =
            velocity.magnitude *
            rotationMultiplier *
            rotationDirection;

        // 타겟 회전 속도를 최대 회전 속도로 제한
        targetRotationSpeed = Mathf.Clamp(
            targetRotationSpeed,
            -maxRotationSpeed,
            maxRotationSpeed
        );

        // 회전 속도를 보간하여 부드럽게 변경
        currentRotationSpeed = Mathf.Lerp(
            currentRotationSpeed,
            targetRotationSpeed,
            rotationSmoothing * Time.fixedDeltaTime
        );

        // 현재 회전속도 리지드바디에 적용
        rb.angularVelocity = currentRotationSpeed;
    }
}