using System;
using System.Collections;
using UnityEngine;

// 블록을 목표 위치까지 부드럽게 이동시키는 스크립트
public class BlockSnapMover : MonoBehaviour
{
    // 블록이 이동하는 속도, 숫자가 클수록 빠르게 이동
    [SerializeField] private float moveSpeed = 10f;

    // 이동 중인 코루틴을 저장하기 위한 변수
    private Coroutine moveCoroutine;

    // 목표 위치까지 이동시키는 MoveTo 함수
    public void MoveTo( Vector3 targetPosition, Action onCompleted = null)
    {
        // 이미 이동 중이면 기존 이동 중지
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        // 새로운 이동 코루틴 시작
        moveCoroutine = StartCoroutine(
            MoveRoutine(targetPosition, onCompleted)
        );
    }

    // 실제 이동을 수행하는 코루틴
    private IEnumerator MoveRoutine( Vector3 targetPosition, Action onCompleted)
    {
        while (Vector3.Distance( transform.position, targetPosition) > 0.01f)
        {
            // 현재 위치에서 목표 위치까지 부드럽게 이동
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        // 마지막 위치를 정확하게 맞춤
        transform.position = targetPosition;

        // 이동 완료 후 코루틴 종료
        moveCoroutine = null;

        // 이동 완료 후 전달받은 함수 실행
        onCompleted?.Invoke();
    }
}