using UnityEngine;
using UnityEngine.InputSystem;

public class DraggableBlock : MonoBehaviour
{
    [Header("드래그 설정")]

    // 드래그하는 동안 블록을 다른 스프라이트보다 앞에 보여주기 위한 순서
    // 숫자가 클수록 화면 앞쪽에 표시됨
    [SerializeField] private int draggingSortingOrder = 100;

    // 손가락보다 블록을 얼마나 위에 표시할지
    [SerializeField] private float dragHeight = 1f;

    // 화면 좌표를 게임 월드 좌표로 변환할 때 사용할 메인 카메라
    private Camera mainCamera;

    // 블록을 정확하게 눌렀는지 검사할 Collider2D
    private Collider2D blockCollider;

    // 블록의 화면 출력 순서를 변경할 SpriteRenderer
    private SpriteRenderer spriteRenderer;

    // 드래그하기 전 블록의 원래 출력 순서
    private int originalSortingOrder;

    // 현재 블록을 드래그하고 있는지 저장
    private bool isDragging;

    // 블록이 처음 생성된 위치
    private Vector3 spawnPosition;


    // 오브젝트가 생성될 때 가장 먼저 실행됨
    private void Awake()
    {
        mainCamera = Camera.main;
        blockCollider = GetComponentInChildren<Collider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalSortingOrder = spriteRenderer.sortingOrder;
    }
    private void Start()
    {
        // 블록이 처음 생성된 위치를 저장
        spawnPosition = transform.position;
    }

    // 게임이 실행되는 동안 매 프레임 호출됨
    private void Update()
    {
        // 현재 마우스나 터치 입력이 없다면 종료
        if (Pointer.current == null)
            return;

        Vector2 pointerPosition = GetPointerWorldPosition();

        // 처음 누른 순간
        if (Pointer.current.press.wasPressedThisFrame)
        {
            // 누른 위치가 이 블록의 Collider2D 안에 있는지 확인
            if (blockCollider.OverlapPoint(pointerPosition))
            {
                // 블록 드래그 시작
                BeginDrag(pointerPosition);
            }
        }

        // 계속 누르고 있는 동안
        if (isDragging && Pointer.current.press.isPressed)
        {
            Drag(pointerPosition);
        }

        // 손을 놓은 순간
        if (isDragging && Pointer.current.press.wasReleasedThisFrame)
        {
            EndDrag();
        }
    }

    // 화면상의 마우스/터치 위치를 게임 월드 좌표로 바꾸는 함수
    private Vector2 GetPointerWorldPosition()
    {
        // 새 Input System에서 마우스 또는 터치의 화면 좌표를 가져옴
        Vector2 screenPosition =
            Pointer.current.position.ReadValue();

        // 화면 좌표를 게임 월드 좌표로 변환 및 반환
        return mainCamera.ScreenToWorldPoint(screenPosition);
    }

    // 블록을 처음 누른 순간 실행되는 함수
    private void BeginDrag(Vector2 pointerPosition)
    {
        isDragging = true;

        spriteRenderer.sortingOrder =
            draggingSortingOrder;

        // 누르는 즉시 블록을 손가락 위로 이동
        Drag(pointerPosition);
    }

    // 손가락을 움직이는 동안 실행되는 함수
    private void Drag(Vector2 pointerPosition)
    {
        transform.position = new Vector3(
            pointerPosition.x,
            pointerPosition.y + dragHeight,
            transform.position.z
        );
    }

    // 손가락을 놓았을 때 실행되는 함수
    private void EndDrag()
    {
        isDragging = false;

        spriteRenderer.sortingOrder =
            originalSortingOrder;

        // 타일맵 밖에서 손을 놓으면 스폰 위치로 복귀
        if (!PlacementPointManager.Instance.IsInsideBoard(
            transform.position))
        {
            transform.position = spawnPosition;
            return;
        }
    }
}