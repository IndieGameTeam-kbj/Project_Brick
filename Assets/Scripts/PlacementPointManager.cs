using UnityEngine;

public class PlacementPointManager : MonoBehaviour
{
    public static PlacementPointManager Instance { get; private set; }

    [Header("타일맵 영역")]
    [SerializeField] private Collider2D boardArea;

    [Header("블록 배치 슬롯")]
    [SerializeField] private BoardSlot[] slots;

    private void Awake()
    {
        Instance = this;
    }

    // 블록 위치가 타일맵 안에 있는지 확인
    public bool IsInsideBoard(Vector2 position)
    {
        return boardArea.OverlapPoint(position);
    }

    public BoardSlot GetNearestEmptySlot(Vector2 blockPosition)
    {
        BoardSlot nearestSlot = null;

        // 가장 가까운 거리를 저장
        // 처음에는 어떤 거리보다도 큰 값으로 설정
        float nearestDistance = Mathf.Infinity;

        foreach (BoardSlot slot in slots)
        {
            // 이미 블록이 있는 슬롯은 검사하지 않음
            if (slot.IsOccupied)
                continue;

            // 블록과 현재 슬롯 사이의 거리 계산
            float distance = Vector2.Distance(
                blockPosition,
                slot.transform.position
            );

            // 기존에 찾은 슬롯보다 더 가까운 경우
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestSlot = slot;
            }
        }

        // 가장 가까운 빈 슬롯 반환
        return nearestSlot;
    }
}