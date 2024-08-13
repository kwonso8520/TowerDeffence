using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int wayPointCount; // 이동 경로 개수
    private Transform[] wayPoints; // 이동 경로 정보
    private int currentIndex = 0; // 현재 목표 지점 인덱스
    private MoveMent2D moveMent2D; // 오브젝트 이동 제어
    private EnemySpawner enemySpawner; // 적의 삭제를 본인이 하지 않고 EnemySpawner에 알려서 삭제
    public void Setup(EnemySpawner enemySpawner, Transform[] wayPoints)
    {
        moveMent2D = GetComponent<MoveMent2D>();
        this. enemySpawner = enemySpawner;

        // 적 이동경로 wayPoints 정보 설정
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        // 적의 위치를 첫번째 wayPoint 위치로 설정
        transform.position = wayPoints[currentIndex].position;

        // 적 이동/목표지점 설정 코루틴 함수 시작
        StartCoroutine("OnMove");
    }
    private IEnumerator OnMove()
    {
        NextMoveTo();

        while (true)
        {
            // 적 오브젝트 회전
            transform.Rotate(Vector3.forward * 10);

            // 적의 현재 위치와 목표 위치의 거리가 0.02 *  moveMent2D.MoveSpeed보다 작을 떄 if 조건문 실행
            // Tip. moveMent2D.MoveSpeed를 곱해주는 이유는 속도가 빠르면 한 프레임에 0.02보다 크게 움직이기 때문에
            // if 조건문에 걸리지 않고 경로를 탈주하는 오브젝트가 발생할 수 있다.
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * moveMent2D.MoveSpeed)
            {
                NextMoveTo();
            }
            yield return null;
        }
    }
    private void NextMoveTo()
    {
        // 아직 이동할 wayPoints가 남아있다면
        if (currentIndex < wayPoints.Length - 1)
        {
            // 적의 위치를 정확하게  목표 위치로 설정(wayPoints)
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            moveMent2D.MoveTo(direction);
        }
        // 현재 위치가 마지막 wayPoints이면
        else
        {
            // 적 오브젝트 삭제
            OnDie();
        }
    }
    public void OnDie()
    {
        // EnemySpawner에서 리스트로 적 정보를 관리하기 때문에 Destroy()를 직접하지 않고
        // EnemySpawner에서 본인이 삭제될 때 필요한 처리를 하도록 DestroyEnemy() 함수 호출
        enemySpawner.DestroyEnemy(this);
    }
}
    