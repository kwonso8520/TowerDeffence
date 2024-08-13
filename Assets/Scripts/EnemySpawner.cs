using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab; // �� ������
    [SerializeField]
    private GameObject enemyHpSliderPrefab; // �� ü�� Ui
    [SerializeField]
    private Transform canvasTransform; // ĵ���� ������Ʈ�� Transform
    [SerializeField]
    private float spawnTime; // �� ���� �ֱ�
    [SerializeField]
    private Transform[] wayPoints; // ���� ���������� �̵� ���
    [SerializeField]
    private PlayerHp playerHp; // �÷��̾��� ü�� ������Ʈ
    [SerializeField]
    private PlayerGold playerGold; // �÷��̾��� ��� ������Ʈ
    private List<Enemy> enemyList; // ���� �ʿ� �����ϴ� ��� ���� ����

    // ���� ������ ������ EnemySpawner���� �ϱ� ������ Set�� �ʿ����� ����
    public List<Enemy> EnemyList => enemyList;

    private void Awake()
    {
        // �� ����Ʈ �޸� �Ҵ�
        enemyList = new List<Enemy>();
        // �� ���� �ڷ�ƾ ȣ�� 
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject clone = Instantiate(enemyPrefab); // �� ������Ʈ ����
            Enemy enemy = clone.GetComponent<Enemy>(); // ��� ������ ���� Enemy������Ʈ

            enemy.Setup(this, wayPoints); // wayPoint ������ �Ű������� Setup() ȣ��
            enemyList.Add(enemy); // ����Ʈ���� ��� ������ �� ���� ����

            SpawnEnemyHpSlider(clone);

            yield return new WaitForSeconds(spawnTime); // spawnTime �ð� ���� ���
        }
    }
    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold)
    {
        // ���� ��ǥ�������� �������� ���
        if (type == EnemyDestroyType.Arrive)
        {
            // �÷��̾� ü�� 1����
            playerHp.TakeDamage(1);
        }
        else if (type == EnemyDestroyType.Kill)
        {
            //���� ������ ���� ����� ��� ȹ��
            playerGold.CurrentGold += gold;
        }
        // ����Ʈ���� ����� �� ���� ����
        enemyList.Remove(enemy);
        // �� ������Ʈ ����
        Destroy(enemy.gameObject);
    }
    private void SpawnEnemyHpSlider(GameObject enemy)
    {
        // �� ü���� ��Ÿ���� Ui����
        GameObject sliderClone = Instantiate(enemyHpSliderPrefab);  
        sliderClone.transform.SetParent(canvasTransform);
        sliderClone.transform.localScale = Vector3.one;

        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        sliderClone.GetComponent<EnemyHpViewer>().Setup(enemy.GetComponent<EnemyHp>());
    }
}
