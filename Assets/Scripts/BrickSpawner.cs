using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _brickPrefabs;
    [SerializeField] private Transform _brickSpawnPoint;
    [SerializeField] private Transform[] _brickPreparedPoints;
    [SerializeField] private Transform _brickParent;

    public Transform[] PreparedPoints => _brickPreparedPoints;
    private Coroutine _spawnCoroutine;
    private float _spawnInterval = 0.2f;

    public void Reset()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
    }

    public BrickController[] SpawnBricks()
    {
        BrickController[] bricks = new BrickController[_brickPreparedPoints.Length];

        for (int i = 0; i < _brickPreparedPoints.Length; i++)
        {
            bricks[i] = SpawnBrick(i);
        }

        _spawnCoroutine = StartCoroutine(SpawnBricksRoutine(bricks));
        return bricks;
    }

    private BrickController SpawnBrick(int index)
    {
        int randomIndex = Random.Range(0, _brickPrefabs.Length); 
        GameObject spawnedObject = Instantiate(_brickPrefabs[randomIndex], _brickSpawnPoint.position, Quaternion.identity, _brickParent);
        BrickController brick = spawnedObject.GetComponent<BrickController>();
        brick.Init(_brickPreparedPoints[index].position);
        return brick;
    }

    private IEnumerator SpawnBricksRoutine(BrickController[] bricks)
    {
        SoundManager.Instance.PlayBlockSpawn();

        for (int i = 0; i < bricks.Length; i++)
        {
            bricks[i].Spawn();
            yield return new WaitForSeconds(_spawnInterval);
        }

        _spawnCoroutine = null;
    }

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

    public BrickController[] RestorePrepared(int[] types)
    {
        BrickController[] bricks = new BrickController[_brickPreparedPoints.Length];

        for (int i = 0; i < bricks.Length; i++)
        {
            if (types[i] == -1) continue;

            bricks[i] = Create( (BrickType)types[i], _brickPreparedPoints[i].position, false );
        }

        return bricks;
    }
}
