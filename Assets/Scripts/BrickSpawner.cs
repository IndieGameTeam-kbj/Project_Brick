using System.Collections;
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
}
