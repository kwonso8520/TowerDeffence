using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    private Image towerImg;
    [SerializeField]
    private Text damageTxt;
    [SerializeField]
    private Text rateTxt;
    [SerializeField]
    private Text rangeTxt;
    [SerializeField]
    private Text levelTxt;
    [SerializeField]
    private TowerAttackRange towerAttackRange;

    private TowerWeapon currentTower;

    private void Awake()
    {
        OffPanel();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }
    public void OnPanel(Transform towerWeapon)
    {
        // 출력해야하는 타워 정보를 받아와서 저장
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        gameObject.SetActive(true);
        UpdateTowerData();
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }
    public void OffPanel()
    {
        gameObject.SetActive(false);
        towerAttackRange.OffAttackRange();
    }
    private void UpdateTowerData()
    {
        damageTxt.text = "Damage : " + currentTower.Damage;
        rateTxt.text = "Rate : " + currentTower.Rate;
        rangeTxt.text = "Range : " + currentTower.Range;
        levelTxt.text = "Level : " + currentTower.Level;
    }
}
