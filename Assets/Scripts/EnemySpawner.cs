using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    /*[SerializeField]
    private GameObject enemyPrefab; // �� ������*/
    [SerializeField]
    private GameObject enemyHpSliderPrefab; // �� ü�� Ui
    [SerializeField]
    private Transform canvasTransform; // ĵ���� ������Ʈ�� Transform
   /* [SerializeField]
    private float spawnTime; // �� ���� �ֱ�*/
    [SerializeField]
    private Transform[] wayPoints; // ���� ���������� �̵� ���
    [SerializeField]
    private PlayerHp playerHp; // �÷��̾��� ü�� ������Ʈ
    [SerializeField]
    private PlayerGold playerGold; // �÷��̾��� ��� ������Ʈ
    private Wave currnetWave;
    private int currentEnemyCount;
    private List<Enemy> enemyList; // ���� �ʿ� �����ϴ� ��� ���� ����

    // ���� ���̺��� �����ִ� ��, �ִ� �� ����
    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currnetWave.maxEnemyCount;

    // ���� ������ ������ EnemySpawner���� �ϱ� ������ Set�� �ʿ����� ����
    public List<Enemy> EnemyList => enemyList;

    private void Awake()
    {
        // �� ����Ʈ �޸� �Ҵ�
        enemyList = new List<Enemy>();
        // �� ���� �ڷ�ƾ ȣ�� 
        //StartCoroutine("SpawnEnemy");
    }
    public void StartWave(Wave wave)
    {
        // �Ű������� �޾ƿ� ���̺� ���� ����
        currnetWave = wave;
        // ���� ���̺��� �ִ� �� ���ڸ� ����
        currentEnemyCount = currnetWave.maxEnemyCount;
        // ���� ���̺� ����
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        int spawnEnemyCount = 0;
        while (spawnEnemyCount <  currnetWave.maxEnemyCount)
        {
            int enemyIndex = Random.Range(0, currnetWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currnetWave.enemyPrefabs[enemyIndex]);
            Enemy enemy = clone.GetComponent<Enemy>(); // ��� ������ ���� Enemy������Ʈ

            enemy.Setup(this, wayPoints); // wayPoint ������ �Ű������� Setup() ȣ��
            enemyList.Add(enemy); // ����Ʈ���� ��� ������ �� ���� ����

            SpawnEnemyHpSlider(clone);
            
            //���� ���̺꿡 ������ ���� ���� 1�߰�
            spawnEnemyCount++;

            yield return new WaitForSeconds(currnetWave.spawnTime); // spawnTime �ð� ���� ���
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
        // ���� ������ �� �� �ϳ� ����
        currentEnemyCount --;
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
