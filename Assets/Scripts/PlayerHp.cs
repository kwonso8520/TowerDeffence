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
    private float maxHp = 20; // 최대 체력
    private float currentHp; // 현재체력

    public float MaxHp => maxHp;
    public float CurrentHp => currentHp;

    private void Awake()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        // 현재 체력을 damage만큼 감소
        currentHp -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        // 체력이 0이 되면 게임 오버
        if (currentHp <= 0)
        {

        }
    }
    private IEnumerator HitAlphaAnimation()
    {
        // screenImg의 투명도를 40%로 설정
        Color color = screenImg.color;
        color.a = 0.4f;
        screenImg.color = color;

        //투명도가 0이 될때까지 감소
        while (color.a >= 0.0f)
        {
            color.a -= Time.deltaTime;
            screenImg.color = color;

            yield return null;
        }
    }
}
