using UnityEngine;

public class BoardSlot : MonoBehaviour
{
    // 현재 이 슬롯에 블록이 있는지
    private bool isOccupied;

    public bool IsOccupied => isOccupied;

    // 슬롯에 블록이 배치됐을 때 호출
    public void Occupy()
    {
        isOccupied = true;
    }

    // 슬롯에서 블록이 제거됐을 때 호출
    public void Clear()
    {
        isOccupied = false;
    }
}