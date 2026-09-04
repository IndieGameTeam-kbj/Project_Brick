using System.Collections.Generic;
using UnityEngine;

public class BlockDestroyer : MonoBehaviour
{
    public void DestroyBlocks(HashSet<BoardSlot> slotsToDestroy)
    {
        foreach (BoardSlot slot in slotsToDestroy)
        {
            LineBlock block = slot.PlacedBlock;

            // 슬롯을 먼저 빈 상태로 변경
            slot.Clear();

            if (block != null)
            {
                Destroy(block.gameObject);
            }
        }
    }
}