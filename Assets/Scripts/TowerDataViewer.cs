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
        // ����ؾ��ϴ� Ÿ�� ������ �޾ƿͼ� ����
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
            // Ÿ�� ���׷��̵� �õ�(���� : true, ���� : false)
            UpdateTowerData();
            // Ÿ�� �ֺ��� ���̴� ���� ������ ����
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
        }
        else
        {
            // Ÿ�� ���׷��̵忡 �ʿ��� ����� �����ϴٰ� ���   
            systemTextViewer.PrintText(SystemType.Money);
        }
    }
    public void OnClickEventTowerSell()
    {
        // Ÿ�� �Ǹ�
        currentTower.Sell();
        // ������ Ÿ���� ������� Panel, ���� ���� off
        OffPanel();
    }
}
