using UnityEngine;

public class LineBlock : MonoBehaviour
{
    [Header("상하좌우")]
    [SerializeField] private bool up;
    [SerializeField] private bool down;
    [SerializeField] private bool left;
    [SerializeField] private bool right;

    [Header("대각선")]
    [SerializeField] private bool upLeft;
    [SerializeField] private bool upRight;
    [SerializeField] private bool downLeft;
    [SerializeField] private bool downRight;

    public bool HasConnection(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
            return up;

        if (direction == Vector2Int.down)
            return down;

        if (direction == Vector2Int.left)
            return left;

        if (direction == Vector2Int.right)
            return right;

        if (direction == new Vector2Int(-1, 1))
            return upLeft;

        if (direction == new Vector2Int(1, 1))
            return upRight;

        if (direction == new Vector2Int(-1, -1))
            return downLeft;

        if (direction == new Vector2Int(1, -1))
            return downRight;

        return false;
    }
}