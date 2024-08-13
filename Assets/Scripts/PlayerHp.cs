using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    [SerializeField]
    private Image screenImg;

    [SerializeField]
    private float maxHp = 20; // �ִ� ü��
    private float currentHp; // ����ü��

    public float MaxHp => maxHp;
    public float CurrentHp => currentHp;

    private void Awake()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        // ���� ü���� damage��ŭ ����
        currentHp -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        // ü���� 0�� �Ǹ� ���� ����
        if (currentHp <= 0)
        {

        }
    }
    private IEnumerator HitAlphaAnimation()
    {
        // screenImg�� ������ 40%�� ����
        Color color = screenImg.color;
        color.a = 0.4f;
        screenImg.color = color;

        //������ 0�� �ɶ����� ����
        while (color.a >= 0.0f)
        {
            color.a -= Time.deltaTime;
            screenImg.color = color;

            yield return null;
        }
    }
}
