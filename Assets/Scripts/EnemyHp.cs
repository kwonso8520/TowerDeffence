using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    [SerializeField]
    private float maxHp; // 최대 체력
    private float currentHp; // 현재 체력
    private bool isDie = false; // 적이 사망 상태이면 true로 설정
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
        // 현재 적의 상태가 사망 상태이면 코드를 실행하지 않는다.
        if (isDie == true) return;

        // 현재 체력을 damage만큼 감소
        currentHp -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        // 체력이 0 이하 = 적 캐릭터 사망
        if(currentHp <= 0)
        {
            isDie = true;
            // 적 캐릭터 사망
            enemy.OnDie(EnemyDestroyType.Kill);
        }
    }

    private IEnumerator HitAlphaAnimation()
    {
        // 현재 적의 색상을 color 변수에 저장
        Color color = spriteRenderer.color;

        // 적의 투명도를 40%로 설정
        color.a = 0.4f;
        spriteRenderer.color = color;

        // 0.05초 동안 대기
        yield return new WaitForSeconds(0.05f);

        // 적의 투명도를 100%로 설정
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
