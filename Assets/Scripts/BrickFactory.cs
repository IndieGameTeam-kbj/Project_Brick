using System.Collections.Generic;
using UnityEngine;

public class BrickFactory : MonoBehaviour
{
    [SerializeField] private GameObject[] _brickPrefabs;
    [SerializeField] private Transform _brickParent;

    private GameObject GetPrefabByType(BrickType type)
    {
        foreach (GameObject prefab in _brickPrefabs)
        {
            BrickController brick = prefab.GetComponent<BrickController>();

            if (brick.Types[0] == type)
            {
                return prefab;
            }
        }

        throw new System.InvalidOperationException(
            $"타입에 맞는 프리팹이 없습니다: {type}"
        );
    }

    private BrickController Create(
        BrickType type,
        Vector3 position,
        bool placed)
    {
        GameObject spawnedObject = Instantiate(
            GetPrefabByType(type),
            position,
            Quaternion.identity,
            _brickParent
        );

        BrickController brick = spawnedObject.GetComponent<BrickController>();

        brick.Init(position);
        brick.RestoreAt(position, placed);

        return brick;
    }

    public void RestoreBoard(
        List<BrickSaveData> data,
        BoardSlot[,] slots)
    {
        foreach (BrickSaveData saved in data)
        {
            BoardSlot slot = slots[saved.row, saved.column];

            BrickController brick = Create(saved.type, slot.transform.position, true);

            slot.Place(brick);
        }
    }

    public BrickController[] RestorePrepared(
        int[] types,
        Transform[] points)
    {
        BrickController[] bricks = new BrickController[points.Length];

        for (int i = 0; i < bricks.Length; i++)
        {
            if (types[i] == -1) continue;

            bricks[i] = Create( (BrickType)types[i], points[i].position, false);
        }

        return bricks;
    }
}