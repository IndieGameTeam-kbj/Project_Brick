using System.Collections;
using UnityEngine;

// 블록이 생성될 때 재생되는 효과를 담당하는 클래스
public class BlockSpawnEffect : MonoBehaviour
{
    // 블록이 생성될 때 재생되는 효과의 지속 시간
    [Header("생성 효과")]
    [SerializeField] private float duration = 0.25f;

    // 블록이 생성될 때 크기 변화를 나타내는 애니메이션 곡선
    [SerializeField]
    private AnimationCurve scaleCurve =
        new AnimationCurve(
            new Keyframe(0f, 0f),
            new Keyframe(0.7f, 1.1f),
            new Keyframe(1f, 1f)
        );

    // 블록의 원래 크기를 저장하기 위한 변수
    private Vector3 originalScale;

    // 블록의 Collider2D를 비활성화하기 위한 변수
    private Collider2D blockCollider;

    private void Awake()
    {
        //  블록의 원래 크기를 저장
        originalScale = transform.localScale;

        // 블록의 Collider2D를 가져옴
        blockCollider = GetComponentInChildren<Collider2D>();
    }

    public void Play()
    {
        // 블록 생성 효과를 재생하는 코루틴 시작
        StartCoroutine(PlayRoutine());
    }

    // 블록 생성 효과를 재생하는 코루틴
    private IEnumerator PlayRoutine()
    {
        // 블록의 Collider2D를 비활성화하여 드래그 방지
        if (blockCollider != null)
        {
            blockCollider.enabled = false;
        }

        // 시작 크기
        transform.localScale = Vector3.zero;

        // 경과 시간 초기화
        float elapsedTime = 0f;

        // 블록 생성 효과를 재생하는 동안 반복
        while (elapsedTime < duration)
        {
            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            // 진행률 계산 (0~1)
            float progress = elapsedTime / duration;

            //  애니메이션 곡선을 사용하여 크기 변화 계산
            float scaleValue = scaleCurve.Evaluate(progress);

            // 블록의 크기 업데이트
            transform.localScale = originalScale * scaleValue;

            // 다음 프레임까지 대기
            yield return null;
        }

        // 블록의 크기를 원래 크기로 복원
        transform.localScale = originalScale;

        // 블록의 Collider2D를 다시 활성화하여 드래그 가능하게 함
        if (blockCollider != null)
        {
            blockCollider.enabled = true;
        }
    }
}