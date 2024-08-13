using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab; // 적 프리팹
    [SerializeField]
    private GameObject enemyHpSliderPrefab; // 적 체력 Ui
    [SerializeField]
    private Transform canvasTransform; // 캔버스 오브젝트의 Transform
    [SerializeField]
    private float spawnTime; // 적 생성 주기
    [SerializeField]
    private Transform[] wayPoints; // 현재 스테이지의 이동 경로
    [SerializeField]
    private PlayerHp playerHp; // 플레이어의 체력 컴포넌트
    [SerializeField]
    private PlayerGold playerGold; // 플레이어의 골드 컴포넌트
    private List<Enemy> enemyList; // 현재 맵에 존재하는 모든 적의 정보

    // 적의 생성과 삭제는 EnemySpawner에서 하기 떄문에 Set은 필요하지 않음
    public List<Enemy> EnemyList => enemyList;

    private void Awake()
    {
        // 적 리스트 메모리 할당
        enemyList = new List<Enemy>();
        // 적 생성 코루틴 호출 
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject clone = Instantiate(enemyPrefab); // 적 오브젝트 생성
            Enemy enemy = clone.GetComponent<Enemy>(); // 방금 생성된 적의 Enemy컴포넌트

            enemy.Setup(this, wayPoints); // wayPoint 정보를 매개변수로 Setup() 호출
            enemyList.Add(enemy); // 리스트에서 방금 생성한 적 정보 저장

            SpawnEnemyHpSlider(clone);

            yield return new WaitForSeconds(spawnTime); // spawnTime 시간 동안 대기
        }
    }
    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold)
    {
        // 적이 목표지점까지 도달했을 경우
        if (type == EnemyDestroyType.Arrive)
        {
            // 플레이어 체력 1감소
            playerHp.TakeDamage(1);
        }
        else if (type == EnemyDestroyType.Kill)
        {
            //적의 종류에 따라 사망시 골드 획득
            playerGold.CurrentGold += gold;
        }
        // 리스트에서 사망한 적 정보 삭제
        enemyList.Remove(enemy);
        // 적 오브젝트 삭제
        Destroy(enemy.gameObject);
    }
    private void SpawnEnemyHpSlider(GameObject enemy)
    {
        // 적 체력을 나타내는 Ui생성
        GameObject sliderClone = Instantiate(enemyHpSliderPrefab);  
        sliderClone.transform.SetParent(canvasTransform);
        sliderClone.transform.localScale = Vector3.one;

        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        sliderClone.GetComponent<EnemyHpViewer>().Setup(enemy.GetComponent<EnemyHp>());
    }
}
