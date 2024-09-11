using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private TowerWeapon towerWeapon;
    private MoveMent2D moveMent2D;
    private Transform target;
    private float damage;

    public float Damage => damage;
    public Transform Target => target;
    public TowerWeapon TowerWeapons => towerWeapon;

    public void Setup(Transform target, float damage, TowerWeapon towerWeapon)
    {
        moveMent2D = GetComponent<MoveMent2D>();
        this.target = target; // Ÿ���� �������� target
        this.damage = damage; // Ÿ���� �������� damage
        this.towerWeapon = towerWeapon;
    }

    protected void Update()
    {
        if(target != null) // target�� �����ϸ�
        {
            // �߻�ü�� target�� ��ġ�� �̵�
            Vector3 direction = (target.position - transform.position).normalized;
            moveMent2D.MoveTo(direction);
        }
        else
        {
            Debug.Log("TargetNone");
            // �߻�ü ������Ʈ ����
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;
        if(collision.transform != target) return;

        collision.GetComponent<EnemyHp>().TakeDamage(damage); // �� ����Լ� ȣ��
        Destroy(gameObject); // �߻�ü ������Ʈ ����
    }
}
