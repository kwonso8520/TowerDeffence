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
    private Text UpgradeCostTxt;
    [SerializeField]
    private Text SellCostTxt;
    [SerializeField]
    private TowerAttackRange towerAttackRange;
    [SerializeField]
    private Button upgradeBtn;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
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
        if (currentTower.WeaponType == WeaponType.Cannon || currentTower.WeaponType == WeaponType.Laser)
        {
            towerImg.rectTransform.sizeDelta = new Vector2(88, 59);
            damageTxt.text = "Damage : " + currentTower.Damage + "+" + "<color=red" + currentTower.AddedDamage.ToString("F1") +"</color>";
        }
        else if (currentTower.WeaponType == WeaponType.Slow)
        {
            towerImg.rectTransform.sizeDelta = new Vector2(59, 59);
            damageTxt.text = "Slow : " + currentTower.Slow * 100 + "%";
        }
        else if(currentTower.WeaponType == WeaponType.Buff)
        {
            damageTxt.text = "Buff : " + currentTower.Buff * 100 + "%";
        }

        towerImg.sprite = currentTower.TowerSprite;
        rateTxt.text = "Rate : " + currentTower.Rate;
        rangeTxt.text = "Range : " + currentTower.Range;
        levelTxt.text = "Level : " + currentTower.Level;
        UpgradeCostTxt.text = currentTower.UpgradeCost.ToString();
        SellCostTxt.text = currentTower.SellCost.ToString();

        upgradeBtn.interactable = currentTower.Level < currentTower.MaxLevel ? true : false;
    }
    public void OnClickEvnetTowerUpgrade()
    {
        bool isSuccess = currentTower.Upgrade();

        if (isSuccess == true)
        {
            // 타워 업그레이드 시도(성공 : true, 실패 : false)
            UpdateTowerData();
            // 타워 주변에 보이는 공격 범위도 갱신
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
        }
        else
        {
            // 타워 업그레이드에 필요한 비용이 부족하다고 출력   
            systemTextViewer.PrintText(SystemType.Money);
        }
    }
    public void OnClickEventTowerSell()
    {
        // 타워 판매
        currentTower.Sell();
        // 선택한 타워가 사라져서 Panel, 공격 범위 off
        OffPanel();
    }
}
