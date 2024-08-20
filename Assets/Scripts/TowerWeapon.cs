using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {Cannon = 0, Laser, }
public enum WeaponState { SearchTarget = 0, TryAttackCannon, TryAttackLaser, }

public class TowerWeapon : MonoBehaviour
{
    [Header("Commons")]
    [SerializeField]
    private TowerTemplate towerTemplate;
    [SerializeField]
    private Transform spawnPoint; // �߻�ü ���� ��ġ
    [SerializeField]
    private WeaponType weaponType;

    [Header("Cannon")]
    [SerializeField]
    private GameObject projectilePrefab; // �߻�ü ������

    [Header("Lazer")]
    [SerializeField]
    private LineRenderer lineRenderer; // �������� ���Ǵ� ��
    [SerializeField]
    private Transform hitEffect; // Ÿ�� ȿ��
    [SerializeField]
    private LayerMask targetLayer; // ������ �ε����� ���̾� ����

    private int level = 0;
    private WeaponState weaponState = WeaponState.SearchTarget; //Ÿ�� ������ ����
    private Transform attackTarget = null; // ���� ���
    private SpriteRenderer spriteRenderer; // Ÿ�� ������Ʈ �̹��� �����
    private EnemySpawner enemySpawner; // ���ӿ� �����ϴ� �� ���� ȹ���
    private PlayerGold playerGold; // �÷��̾��� ��� ���� ȹ�� �� ����
    private Tile ownerTile;// ���� Ÿ���� ��ġ�Ǿ� �ִ� Ÿ��

    public Sprite TowerSprite => towerTemplate.weapon[level].sprite;
    public float Damage => towerTemplate.weapon[level].damage;
    public float Rate => towerTemplate.weapon[level].rate;
    public float Range => towerTemplate.weapon[level].range;
    public int Level => level + 1;
    public int MaxLevel => towerTemplate.weapon.Length;

    public void Setup(EnemySpawner enemySpawner, PlayerGold playerGold, Tile ownerTile)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.enemySpawner = enemySpawner;
        this.playerGold = playerGold;
        this.ownerTile = ownerTile;

        // ���� ���¸� WeaponState.SearchTarget���� ����
        ChangeState(WeaponState.SearchTarget);
    }
    public void ChangeState(WeaponState newState)
    {
        // ������ ������̴� ���� ����
        StopCoroutine(weaponState.ToString());
        // ���� ����
        weaponState = newState;
        StartCoroutine(weaponState.ToString());

    }
    void Update()
    {
        if(attackTarget != null)
        {
            RotateToTarget();
        }
    }
    private void RotateToTarget()
    {
        // �������κ����� �Ÿ��� ���������κ����� ������ �̿��� ��ġ�� ���ϴ� �� ��ǥ�� �̿�
        // ���� = arctan(y / x)
        // x, y ������ ���ϱ�
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;
        // x, y �������� �������� ���� ���ϱ�
        // ������ radian �����̱� ������ Mathf.Rad2Deg�� ���� �� ������ ����
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }
    private IEnumerator SearchTarget()
    {
        while (true)
        {
            // ���� Ÿ���� ���� ������ �ִ� �� Ž��
            attackTarget = FindClossesAttackTarget();

            if (attackTarget != null)
            {
                if (weaponType == WeaponType.Cannon)
                {
                    ChangeState(WeaponState.TryAttackCannon);
                }
                else if (weaponType == WeaponType.Laser)
                {
                    ChangeState(WeaponState.TryAttackLaser);
                }
            }
            yield return null;
        }
    }
    private IEnumerator TryAttackCannon()
    {
        while (true)
        {
            // target�� �����ϴ°� �������� �˻� 
            if(IsPossibleToAttackTarget() == false)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // attackRate �ð���ŭ ���
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);

            // ĳ�� ���� 
            SpawnProjectile();
        }
    }
    private IEnumerator TryAttackLaser()
    {
        // ������, ������ Ÿ�� ȿ�� Ȱ��ȭ
        EnableLaser();

        while (true)
        {
            // target�� �����ϴ°� �������� �˻� 
            if (IsPossibleToAttackTarget() == false)
            {
                DisableLaser();
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // ĳ�� ���� 
            SpawnLaser();

            yield return null;
        }
    }
    private Transform FindClossesAttackTarget()
    {
        // ���� ������ �ִ� ���� ã�� ���� ���� �Ÿ��� �ִ��� ũ�� ����
        float closestDistSqr = Mathf.Infinity;
        // EnemySpawner�� EnemyList�� �ִ� ���� �ʿ� �����ϴ� ��� �� �˻�
        for (int i = 0; i < enemySpawner.EnemyList.Count; ++i)
        {
            float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
            // ���� �˻����� ������ �Ÿ��� ���� ���� ���� �ְ�, ������� �˻��� ������ �Ÿ��� ������
            if(distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
            {
                closestDistSqr = distance;
                attackTarget = enemySpawner.EnemyList[i].transform;
            }
        }
        return attackTarget;
    }
    private bool IsPossibleToAttackTarget()
    {
        // target�� �ִ��� �˻�(�ٸ� �߻�ü�� ���� ����, Goal �������� �̵��� ���� ��)
        if (attackTarget == null)
        {
            return false;
        }

        // target�� ���� ���� �ȿ� �ִ��� �˻�(���� ������ ����� ���ο� �� Ž��)
        float distacne = Vector3.Distance(attackTarget.position, transform.position);
        if (distacne > towerTemplate.weapon[level].range)
        {
            attackTarget = null;
            return false;
        }
        return true;
    }

    private void SpawnProjectile()
    {
        GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        clone.GetComponent<Projectile>().Setup(attackTarget, towerTemplate.weapon[level].damage);
    }
    private void EnableLaser()
    {
        lineRenderer.gameObject.SetActive(true);
        hitEffect.gameObject.SetActive(true);
    }
    private void DisableLaser()
    {
        lineRenderer.gameObject.SetActive(false);
        hitEffect.gameObject.SetActive(false);
    }
    private void SpawnLaser()
    {
        Vector3 direction = attackTarget.position - spawnPoint.position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(spawnPoint.position, direction, towerTemplate.weapon[level].range, targetLayer);

        // ���� �������� ���� ���� ������ ���� �� �� ���� attackTarget�� ������ ������Ʈ ����
        for(int i = 0; i < hit.Length; ++i)
        {
            if(hit[i].transform == attackTarget)
            {
                // ���� ���� ����
                lineRenderer.SetPosition(0, spawnPoint.position);
                // ���� ��ǥ ����
                lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, 0) + Vector3.back);
                // Ÿ�� ȿ�� ��ġ ����
                hitEffect.position = hit[i].point;
                // �� ü�� ����
                attackTarget.GetComponent<EnemyHp>().TakeDamage(towerTemplate.weapon[level].damage * Time.deltaTime);
            }
        }
    }
    public bool Upgrade()
    {
        if (playerGold.CurrentGold < towerTemplate.weapon[level + 1].cost)
        {
            return false;
        }
        // Ÿ�� ���� ����
        level++;
        // Ÿ�� ���� ����
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;
        // ��� ����
        playerGold.CurrentGold -= towerTemplate.weapon[level].cost;

        // ���� �Ӽ��� �������̸�
        if (weaponType == WeaponType.Laser)
        {
            // ������ ���� �������� ���� ����
            lineRenderer.startWidth = 0.05f + level * 0.05f;
            lineRenderer.endWidth = 0.05f;
        }

        return true;
    }
    public void Sell()
    {
        // ��� ����
        playerGold.CurrentGold += towerTemplate.weapon[level].sell;
        // ���� Ÿ�Ͽ� �ٽ� Ÿ�� �Ǽ��� �����ϵ��� ����
        ownerTile.IsBuildTower = false; 
        // Ÿ�� �ı�
        Destroy(gameObject);
    }
}
