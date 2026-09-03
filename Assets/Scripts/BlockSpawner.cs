using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [Header("생성할 블록 프리팹")]
    [SerializeField] private GameObject[] blockPrefabs;

    [Header("블록 생성 위치 3개")]
    [SerializeField] private Transform[] spawnPoints;

    // 현재 묶음에서 배치된 블록 개수
    private int placedBlockCount;

    // 시작 시점에 블록을 생성하는 메서드 호출
    private void Start()
    {
        SpawnThreeBlocks();
    }

    // 블록을 생성하는 메서드
    public void SpawnThreeBlocks()
    {
        placedBlockCount = 0;
        // spawnPoints 배열의 각 위치에 대해 블록을 생성
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            int randomIndex =
                Random.Range(0, blockPrefabs.Length);

            GameObject spawnedBlock = Instantiate(
                blockPrefabs[randomIndex],
                spawnPoints[i].position,
                Quaternion.identity
            );

            // 생성된 블록에 자신의 참조를 전달
            DraggableBlock draggableBlock =
                spawnedBlock.GetComponent<DraggableBlock>();

            draggableBlock.Initialize(this);
        }
    }

    // 블록 하나가 보드에 정상 배치됐을 때 호출
    public void NotifyBlockPlaced()
    {
        placedBlockCount++;

        // 생성된 블록 3개가 모두 배치됐다면
        if (placedBlockCount >= spawnPoints.Length)
        {
            SpawnThreeBlocks();
        }
    }
}