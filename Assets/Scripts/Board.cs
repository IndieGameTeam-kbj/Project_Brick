using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private int _rowCount;
    [SerializeField] private int _columnCount;
    [SerializeField] private BoardSlot[] _slotReferences;

    private Collider2D _boardArea;
    private BoardSlot[,] _slots;
    private List<List<BoardSlot>> _destructionOrder = new List<List<BoardSlot>>();
    private float _destroyInterval = 0.2f;

    public event Action<int> LineDestroyed;

    private void Awake()
    {
        _boardArea = GetComponent<Collider2D>();

        _slots = new BoardSlot[_rowCount, _columnCount];

        for (int row = 0; row < _rowCount; row++)
        {
            for (int column = 0; column < _columnCount; column++)
            {
                int index = row * _columnCount + column;
                _slots[row, column] = _slotReferences[index];
                _slots[row, column].Init(row, column);
            }
        }
    }

    public bool TryPlaceBrick(BrickController brick, Vector2 position)
    {
        if (!TryGetPlacementSlot(position, out BoardSlot slot)) return false;

        slot.Place(brick);
        brick.Place(slot.transform.position);
        CheckLine(slot);
        return true;
    }

    public bool TryGetPlacementSlot(Vector2 position, out BoardSlot slot)
    {
        slot = null;

        if (!_boardArea.OverlapPoint(position)) return false;

        float nearestDistance = Mathf.Infinity;

        foreach (BoardSlot candidate in _slots)
        {
            float distance = Vector2.Distance(position, candidate.transform.position);

            if (distance >= nearestDistance) continue;

            nearestDistance = distance;
            slot = candidate;
        }

        return slot != null && !slot.IsPlaced;
    }

    private void CheckLine(BoardSlot startSlot)
    {
        BrickController startBrick = startSlot.PlacedBrick;

        if (startBrick == null) return;

        _destructionOrder.Clear();

        foreach (BrickType type in startBrick.Types)
        {
            List<List<BoardSlot>> destructionOrder = new List<List<BoardSlot>>
            {
                new List<BoardSlot> { startSlot }
            };

            var offset = GetOffset(type);

            int row = startSlot.Row;
            int column = startSlot.Column;

            int rowOffsetPlus = offset.rowOffset;
            int columnOffsetPlus = offset.columnOffset;

            int rowOffsetMinus = -offset.rowOffset;
            int columnOffsetMinus = -offset.columnOffset;

            bool plus = true;
            bool minus = true;

            bool plusWall = false;
            bool minusWall = false;

            int count = 0;

            while (plus || minus)
            {
                count++;

                List<BoardSlot> currentLevel = new List<BoardSlot>();

                if (plus)
                {
                    if (TryGetLineSlot(row, column, rowOffsetPlus, columnOffsetPlus, count, type, out BoardSlot slot))
                    {
                        currentLevel.Add(slot);
                    }
                    else
                    {
                        int targetRow = row + rowOffsetPlus * count;
                        int targetColumn = column + columnOffsetPlus * count;

                        if (IsOutsideBoard(targetRow, targetColumn))
                        {
                            plus = false;
                            plusWall = true;
                        }
                        else
                        {
                            plus = false;
                        }
                    }
                }

                if (minus)
                {
                    if (TryGetLineSlot(row, column, rowOffsetMinus, columnOffsetMinus, count, type, out BoardSlot slot))
                    {
                        currentLevel.Add(slot);
                    }
                    else
                    {
                        int targetRow = row + rowOffsetMinus * count;
                        int targetColumn = column + columnOffsetMinus * count;

                        if (IsOutsideBoard(targetRow, targetColumn))
                        {
                            minus = false;
                            minusWall = true;
                        }
                        else
                        {
                            minus = false;
                        }
                    }
                }

                if (currentLevel.Count > 0)
                {
                    destructionOrder.Add(currentLevel);
                }

                if (plusWall && minusWall)
                {
                    break;
                }

                if (!plus && !minus)
                {
                    break;
                }
            }

            if (!plusWall || !minusWall)
            {
                continue;
            }

            for (int level = 0; level < destructionOrder.Count; level++)
            {
                if (_destructionOrder.Count <= level)
                {
                    _destructionOrder.Add(new List<BoardSlot>());
                }

                foreach (BoardSlot slot in destructionOrder[level])
                {
                    if (!_destructionOrder[level].Contains(slot))
                    {
                        _destructionOrder[level].Add(slot);
                    }
                }
            }
        }

        if (_destructionOrder.Count > 0)
        {
            List<List<BoardSlot>> destructionOrder = _destructionOrder;
            _destructionOrder = new List<List<BoardSlot>>();
            StartCoroutine(DestroyLine(destructionOrder));
        }
    }

    private bool TryGetLineSlot(int row, int column, int rowOffset, int columnOffset, int count, BrickType type, out BoardSlot slot)
    {
        slot = null;

        int targetRow = row + rowOffset * count;
        int targetColumn = column + columnOffset * count;

        if (IsOutsideBoard(targetRow, targetColumn))
        {
            return false;
        }

        slot = _slots[targetRow, targetColumn];

        if (slot == null || !slot.IsPlaced)
        {
            slot = null;
            return false;
        }

        BrickController brick = slot.PlacedBrick;

        if (brick == null)
        {
            slot = null;
            return false;
        }

        if (brick.State != BrickState.Placed)
        {
            slot = null;
            return false;
        }

        if (!HasBrickType(brick, type))
        {
            slot = null;
            return false;
        }

        return true;
    }

    private bool IsOutsideBoard(int row, int column)
    {
        return row < 0 || row >= _rowCount || column < 0 || column >= _columnCount;
    }

    private bool HasBrickType(BrickController brick, BrickType type)
    {
        foreach (BrickType brickType in brick.Types)
        {
            if (brickType == type)
            {
                return true;
            }
        }

        return false;
    }

    private (int rowOffset, int columnOffset) GetOffset(BrickType type)
    {
        switch (type)
        {
            case BrickType.Horizontal:
                return (0, 1);

            case BrickType.Vertical:
                return (1, 0);

            case BrickType.DiagonalUpRight:
                return (-1, 1);

            case BrickType.DiagonalDownRight:
                return (1, 1);

            default:
                return (0, 0);
        }
    }

    private IEnumerator DestroyLine(List<List<BoardSlot>> destructionOrder)
    {
        int destroyedBrickCount = 0;

        foreach (List<BoardSlot> level in destructionOrder)
        {
            foreach (BoardSlot slot in level)
            {
                BrickController brick = slot.PlacedBrick;

                if (brick == null) continue;

                brick.Destroyed += slot.Clear;
                brick.Destroy();
                destroyedBrickCount++;
            }

            yield return new WaitForSeconds(_destroyInterval);
        }

        LineDestroyed?.Invoke(destroyedBrickCount);
        _destructionOrder.Clear();
    }

}
