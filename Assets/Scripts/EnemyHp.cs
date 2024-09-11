using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    [SerializeField]
    private float maxHp; // �ִ� ü��
    private float currentHp; // ���� ü��
    private bool isDie = false; // ���� ��� �����̸� true�� ����
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;
    private float waitTime = 0.5f;
    private float poisionDamage = 0.5f;
    [SerializeField]
    private int tick;
    private TowerWeapon towerWeapon;

    public float MaxHp => maxHp;
    public float CurrentHp => currentHp;
    public int Tick
    {
        get { return tick; }
        set {tick = value;}
    }

    private void Awake()
    {
        currentHp = maxHp;
        enemy = GetComponent<Enemy>();
        towerWeapon = GameObject.FindAnyObjectByType<TowerWeapon>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        tick = towerWeapon.Tick;
    }

    public void TakeDamage(float damage)
    {
        if (tick > 0)
        {
            StartCoroutine(GetPoisionDamage(tick));
        }
        // ���� ���� ���°� ��� �����̸� �ڵ带 �������� �ʴ´�.
        if (isDie == true) return;

        // ���� ü���� damage��ŭ ����
        currentHp -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        // ü���� 0 ���� = �� ĳ���� ���
        if(currentHp <= 0)
        {
            isDie = true;
            // �� ĳ���� ���
            enemy.OnDie(EnemyDestroyType.Kill);
        }
    }

    private IEnumerator HitAlphaAnimation()
    {
        // ���� ���� ������ color ������ ����
        Color color = spriteRenderer.color;

        // ���� ������ 40%�� ����
        color.a = 0.4f;
        spriteRenderer.color = color;

        // 0.05�� ���� ���
        yield return new WaitForSeconds(0.05f);

        // ���� ������ 100%�� ����
        color.a = 1.0f;
        spriteRenderer.color = color;
    }

    private IEnumerator GetPoisionDamage(int count)
    {
        for(int i = 0; i < count; i++)
        {
            currentHp -= poisionDamage;
            tick--;
            Debug.Log("DoteDamage");
            yield return new WaitForSeconds(waitTime);
        }
    }
}
