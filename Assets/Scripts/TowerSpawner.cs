using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate towerTemplate; //Ÿ�� ����
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    public void SpawnTower(Transform tileTransform)
    {
        Tile tile = tileTransform.GetComponent<Tile>();

        // Ÿ�� �Ǽ� ���� ���� Ȯ��
        // 1. ���� Ÿ���� ��ġ�� �̹� Ÿ���� �Ǽ��Ǿ� ������ Ÿ�� �Ǽ� x
        if(tile.IsBuildTower == true)
        {
            // ���� ��ġ�� Ÿ�� �Ǽ��� �Ұ����ϴٰ� ���
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }

        // 2.Ÿ���� �Ǽ��� ���� ���ٸ� Ÿ�� �Ǽ� x
        if (towerTemplate.weapon[0].cost > playerGold.CurrentGold)
        {
            // ��尡 �����ؼ� Ÿ�� �Ǽ��� �Ұ����ϴٰ� ���
            systemTextViewer.PrintText(SystemType.Money);
            return ;
        }

        // Ÿ���� �Ǽ��Ǿ� �������� ����
        tile.IsBuildTower = true;
        // ������ Ÿ���� ��ġ�� Ÿ�� �Ǽ�
        Vector3 position = tileTransform.position + Vector3.back;
        GameObject clone = Instantiate(towerTemplate.towerPrefab, position, Quaternion.identity);
        // Ÿ�� �Ǽ��� ����� ��常ŭ ����
        playerGold.CurrentGold -= towerTemplate.weapon[0].cost;
        // Ÿ�� ���⿡ enemySpawner ���� ����
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner, playerGold, tile);
    }
}
