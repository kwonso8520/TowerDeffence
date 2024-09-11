using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Posion : Projectile
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;
        if (collision.transform != Target) return;

        collision.GetComponent<EnemyHp>().TakeDamage(Damage); // 적 사망함수 호출
        collision.GetComponent<EnemyHp>().Tick = TowerWeapons.Tick;
        Destroy(gameObject); // 발사체 오브젝트 삭제
    }
}
