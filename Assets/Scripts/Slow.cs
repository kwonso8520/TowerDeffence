using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : MonoBehaviour
{
    private TowerWeapon towerWeapon;

    private void Awake()
    {
        towerWeapon = GetComponent<TowerWeapon>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
        {
            return;
        }

        MoveMent2D moveMent2D = collision.GetComponent<MoveMent2D>();

        // 이동속도 = 이동속도 - 이동속도 * 감속률;
        moveMent2D.MoveSpeed -= moveMent2D.MoveSpeed * towerWeapon.Slow;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
        {
            return;
        }

        collision.GetComponent<MoveMent2D>().ResetMoveSpeed();
    }
}
