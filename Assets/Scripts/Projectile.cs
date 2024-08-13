using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private MoveMent2D moveMent2D;
    private Transform target;

   public void Setup(Transform target)
    {
        moveMent2D = GetComponent<MoveMent2D>();
        this.target = target; // Ÿ���� �������� target
    }

    private void Update()
    {
        if(target != null) // target�� �����ϸ�
        {
            // �߻�ü�� target�� ��ġ�� �̵�
            Vector3 direction = (target.position - transform.position).normalized;
            moveMent2D.MoveTo(direction);
        }
        else
        {
            // �߻�ü ������Ʈ ����
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;
        if(collision.transform != target) return;

        collision.GetComponent<Enemy>().OnDie(); // �� ����Լ� ȣ��
        Destroy(gameObject); // �߻�ü ������Ʈ ����
    }
}
