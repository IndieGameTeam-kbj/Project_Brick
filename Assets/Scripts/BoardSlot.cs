using UnityEngine;

public class BoardSlot : MonoBehaviour
{
    private LineBlock placedBlock;

    public bool IsOccupied => placedBlock != null;
    public LineBlock PlacedBlock => placedBlock;

    // 블록이 슬롯에 배치됐을 때 호출
    public void Occupy(LineBlock block)
    {
        placedBlock = block;
    }

    // 블록이 제거됐을 때 호출
    public void Clear()
    {
        placedBlock = null;
    }
}