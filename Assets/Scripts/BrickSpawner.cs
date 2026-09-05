using System.Collections;
using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _brickPrefabs;
    [SerializeField] private Transform _brickSpawnPoint;
    [SerializeField] private Transform[] _brickPreparedPoints;
    
    private float _spawnInterval = 0.2f;

    public BrickController[] SpawnBricks()
    {
        BrickController[] bricks = new BrickController[_brickPreparedPoints.Length];
        StartCoroutine(SpawnBricksRoutine(bricks));
        return bricks;
    }

    private IEnumerator SpawnBricksRoutine(BrickController[] bricks)
    {
        for (int i = 0; i < _brickPreparedPoints.Length; i++)
        {
            bricks[i] = SpawnBrick(i);
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    public BrickController SpawnBrick(int index)
    {
        int randomIndex = Random.Range(0, _brickPrefabs.Length); 
        GameObject spawnedObject = Instantiate(_brickPrefabs[randomIndex], _brickSpawnPoint.position, Quaternion.identity);
        BrickController brick = spawnedObject.GetComponent<BrickController>();
        brick.Init(_brickPreparedPoints[index].position);
        return brick;
    }
    
}