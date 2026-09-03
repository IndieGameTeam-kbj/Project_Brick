using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [Header("생성할 블록 프리팹")]
    [SerializeField] private GameObject[] blockPrefabs;

    [Header("블록 생성 위치 3개")]
    [SerializeField] private Transform[] spawnPoints;

    // 생성된 블록들을 저장할 배열
    private GameObject[] spawnedBlocks;

    // 시작 시점에 블록을 생성하는 메서드 호출
    private void Start()
    {
        SpawnThreeBlocks();
    }

    // 블록을 생성하는 메서드
    public void SpawnThreeBlocks()
    {
        // spawnedBlocks 배열을 spawnPoints 배열의 길이로 초기화
        spawnedBlocks = new GameObject[spawnPoints.Length];

        // spawnPoints 배열의 각 위치에 대해 블록을 생성
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            // 랜덤으로 블록 프리팹을 선택
            int randomIndex = Random.Range(0, blockPrefabs.Length);
            GameObject selectedPrefab = blockPrefabs[randomIndex];

            // 선택된 프리팹을 해당 위치에 생성하고 spawnedBlocks 배열에 저장
            spawnedBlocks[i] = Instantiate(
                selectedPrefab,
                spawnPoints[i].position,
                Quaternion.identity
            );
        }
    }
}