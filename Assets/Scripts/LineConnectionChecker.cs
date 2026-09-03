using System.Collections.Generic;
using UnityEngine;

public class LineConnectionChecker : MonoBehaviour
{
    private enum BoardWall
    {
        Top,
        Bottom,
        Left,
        Right
    }

    [Header("보드 크기")]
    [SerializeField] private int rowCount = 5;
    [SerializeField] private int columnCount = 5;

    [Header("보드 슬롯")]
    [SerializeField] private BoardSlot[] slots;

    [Header("블록 파괴")]
    [SerializeField] private BlockDestroyer blockDestroyer;

    private BoardSlot[,] grid;

    private Dictionary<BoardSlot, Vector2Int> slotCoordinates;

    // 탐색할 8개 방향
    private readonly Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right,

        new Vector2Int(-1, 1),   // 왼쪽 위
        new Vector2Int(1, 1),    // 오른쪽 위
        new Vector2Int(-1, -1),  // 왼쪽 아래
        new Vector2Int(1, -1)    // 오른쪽 아래
    };

    private void Awake()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        int requiredSlotCount = rowCount * columnCount;

        if (slots.Length != requiredSlotCount)
        {
            Debug.LogError(
                $"슬롯이 {requiredSlotCount}개 필요합니다."
            );

            return;
        }

        grid = new BoardSlot[rowCount, columnCount];

        slotCoordinates = new Dictionary<BoardSlot, Vector2Int>();

        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                int index = row * columnCount + column;

                BoardSlot slot = slots[index];

                grid[row, column] = slot;

                slotCoordinates.Add( slot, new Vector2Int(row, column));
            }
        }
    }

    // 새로 배치한 슬롯에서 판정 시작
    public void CheckFrom(BoardSlot startSlot)
    {
        if (!slotCoordinates.TryGetValue(startSlot, out Vector2Int coordinate))
        {
            return;
        }

        HashSet<BoardSlot> connectedSlots = new HashSet<BoardSlot>();

        HashSet<BoardWall> touchedWalls = new HashSet<BoardWall>();

        Explore(coordinate, connectedSlots, touchedWalls);

        // 연결망이 서로 다른 벽 두 곳 이상에 닿으면 파괴
        if (touchedWalls.Count >= 2)
        {
            blockDestroyer.DestroyBlocks(connectedSlots);
        }
    }

    private void Explore(Vector2Int coordinate, HashSet<BoardSlot> visited, HashSet<BoardWall> touchedWalls)
    {
        if (!IsValidCoordinate(coordinate))
            return;

        BoardSlot currentSlot = grid[coordinate.x, coordinate.y];

        LineBlock currentBlock = currentSlot.PlacedBlock;

        if (currentBlock == null)
            return;

        // 이미 검사한 슬롯이면 종료
        if (!visited.Add(currentSlot))
            return;

        CheckTouchedWalls(coordinate, currentBlock,touchedWalls);

        foreach (Vector2Int direction in directions)
        {
            // 현재 블록에 해당 방향의 선이 없으면 제외
            if (!currentBlock.HasConnection(direction))
                continue;

            // 인접 슬롯 좌표 계산
            Vector2Int neighborCoordinate = new Vector2Int(
                coordinate.x - direction.y, // Row
                coordinate.y + direction.x  // Column
            );

            if (!IsValidCoordinate(neighborCoordinate))
                continue;

            // 인접 슬롯 가져오기
            BoardSlot neighborSlot = grid[neighborCoordinate.x, neighborCoordinate.y];

            // 인접 슬롯에 블록이 없으면 제외
            LineBlock neighborBlock = neighborSlot.PlacedBlock;

            if (neighborBlock == null)
                continue;

            // 반대 방향 연결 여부 검사
            Vector2Int oppositeDirection = -direction;

            if (!neighborBlock.HasConnection(oppositeDirection))
            {
                continue;
            }

            // 인접 슬롯 탐색
            Explore(neighborCoordinate, visited, touchedWalls);
        }
    }

    private void CheckTouchedWalls(
    Vector2Int coordinate,
    LineBlock block,
    HashSet<BoardWall> touchedWalls)
    {
        int row = coordinate.x;
        int column = coordinate.y;

        // 맨 위 행에 있으며 선이 위쪽 벽을 향하는 경우
        if (row == 0 &&
            (block.HasConnection(Vector2Int.up) ||
             block.HasConnection(new Vector2Int(-1, 1)) ||
             block.HasConnection(new Vector2Int(1, 1))))
        {
            touchedWalls.Add(BoardWall.Top);
        }

        // 맨 아래 행에 있으며 선이 아래쪽 벽을 향하는 경우
        if (row == rowCount - 1 &&
            (block.HasConnection(Vector2Int.down) ||
             block.HasConnection(new Vector2Int(-1, -1)) ||
             block.HasConnection(new Vector2Int(1, -1))))
        {
            touchedWalls.Add(BoardWall.Bottom);
        }

        // 맨 왼쪽 열에 있으며 선이 왼쪽 벽을 향하는 경우
        if (column == 0 &&
            (block.HasConnection(Vector2Int.left) ||
             block.HasConnection(new Vector2Int(-1, 1)) ||
             block.HasConnection(new Vector2Int(-1, -1))))
        {
            touchedWalls.Add(BoardWall.Left);
        }

        // 맨 오른쪽 열에 있으며 선이 오른쪽 벽을 향하는 경우
        if (column == columnCount - 1 &&
            (block.HasConnection(Vector2Int.right) ||
             block.HasConnection(new Vector2Int(1, 1)) ||
             block.HasConnection(new Vector2Int(1, -1))))
        {
            touchedWalls.Add(BoardWall.Right);
        }
    }

    private bool IsValidCoordinate(
        Vector2Int coordinate)
    {
        return coordinate.x >= 0 &&
               coordinate.x < rowCount &&
               coordinate.y >= 0 &&
               coordinate.y < columnCount;
    }
}