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

    public bool Up => up;
    public bool Down => down;
    public bool Left => left;
    public bool Right => right;

    public bool UpLeft => upLeft;
    public bool UpRight => upRight;
    public bool DownLeft => downLeft;
    public bool DownRight => downRight;
}