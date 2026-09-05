using UnityEngine;

public class BoardSlot : MonoBehaviour
{
    private BrickController _placedBrick;
    private int _row;
    private int _column;

    public bool IsPlaced => _placedBrick != null;
    public BrickController PlacedBrick => _placedBrick;
    public int Row => _row;
    public int Column => _column;

    public void Init(int row, int column)
    {
        _row = row;
        _column = column;
    }

    public void Place(BrickController brick)
    {
        _placedBrick = brick;
    }

    public void Clear()
    {
        _placedBrick = null;
    }

}
