using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private MoveMent2D moveMent2D;
    private Transform target;
    private float damage;

   public void Setup(Transform targe, float damage)
    {
        moveMent2D = GetComponent<MoveMent2D>();
        this.target = target; // 타워가 설정해준 target
        this.damage = damage; // 타워가 설정해준 damage
    }

    private void Update()
    {
        if(target != null) // target이 존재하면
        {
            // 발사체를 target의 위치로 이동
            Vector3 direction = (target.position - transform.position).normalized;
            moveMent2D.MoveTo(direction);
        }
        else
        {
            // 발사체 오브젝트 삭제
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;
        if(collision.transform != target) return;

        collision.GetComponent<EnemyHp>().TakeDamage(damage); // 적 사망함수 호출
        Destroy(gameObject); // 발사체 오브젝트 삭제
    }
}
