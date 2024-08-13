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

    private void Awake()
    {
        currentHp = maxHp;
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        // ���� ���� ���°� ��� �����̸� �ڵ带 �������� �ʴ´�.
        if (isDie == true) return;

        // ���� ü���� damage��ŭ ����
        currentHp -= damage;

        StopCoroutine("HItAlphaAnimation");
        StartCoroutine("HItAlphaAnimation");

        // ü���� 0 ���� = �� ĳ���� ���
        if( currentHp <= 0)
        {
            isDie = true;
            // �� ĳ���� ���
            enemy.OnDie();
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
}
