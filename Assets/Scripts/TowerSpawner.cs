using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private int towerBuildGold = 50;
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private PlayerGold playerGold;
    public void SpawnTower(Transform tileTransform)
    {
        Tile tile = tileTransform.GetComponent<Tile>();

        // 타워 건설 가능 여부 확인
        // 1. 현재 타일의 위치에 이미 타워가 건설되어 있으면 타워 건설 x
        if(tile.IsBuildTower == true)
        {
            return;
        }

        // 2.타워를 건설할 돈이 없다면 타워 건설 x
        if(towerBuildGold > playerGold.CurrentGold)
        {
            return ;
        }

        // 타워가 건설되어 있음으로 설정
        tile.IsBuildTower = true;
        // 선택한 타일의 위치에 타워 건설
        GameObject clone = Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
        // 타워 건설에 사용한 골드만큼 감소
        playerGold.CurrentGold -= towerBuildGold;
        // 타워 무기에 enemySpawner 정보 전달
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner);
    }
}
