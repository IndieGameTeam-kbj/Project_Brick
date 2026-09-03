using UnityEngine;

public class PlacementPointManager : MonoBehaviour
{
    public static PlacementPointManager Instance { get; private set; }

    [Header("타일맵 영역")]
    [SerializeField] private Collider2D boardArea;

    [Header("블록 배치 포인트")]
    [SerializeField] private Transform[] placementPoints;

    private void Awake()
    {
        Instance = this;
    }

    // 블록 위치가 타일맵 안에 있는지 확인
    public bool IsInsideBoard(Vector2 position)
    {
        return boardArea.OverlapPoint(position);
    }
}