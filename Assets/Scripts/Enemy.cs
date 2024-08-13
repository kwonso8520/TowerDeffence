using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int wayPointCount; // �̵� ��� ����
    private Transform[] wayPoints; // �̵� ��� ����
    private int currentIndex = 0; // ���� ��ǥ ���� �ε���
    private MoveMent2D moveMent2D; // ������Ʈ �̵� ����
    private EnemySpawner enemySpawner; // ���� ������ ������ ���� �ʰ� EnemySpawner�� �˷��� ����
    public void Setup(EnemySpawner enemySpawner, Transform[] wayPoints)
    {
        moveMent2D = GetComponent<MoveMent2D>();
        this. enemySpawner = enemySpawner;

        // �� �̵���� wayPoints ���� ����
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        // ���� ��ġ�� ù��° wayPoint ��ġ�� ����
        transform.position = wayPoints[currentIndex].position;

        // �� �̵�/��ǥ���� ���� �ڷ�ƾ �Լ� ����
        StartCoroutine("OnMove");
    }
    private IEnumerator OnMove()
    {
        NextMoveTo();

        while (true)
        {
            // �� ������Ʈ ȸ��
            transform.Rotate(Vector3.forward * 10);

            // ���� ���� ��ġ�� ��ǥ ��ġ�� �Ÿ��� 0.02 *  moveMent2D.MoveSpeed���� ���� �� if ���ǹ� ����
            // Tip. moveMent2D.MoveSpeed�� �����ִ� ������ �ӵ��� ������ �� �����ӿ� 0.02���� ũ�� �����̱� ������
            // if ���ǹ��� �ɸ��� �ʰ� ��θ� Ż���ϴ� ������Ʈ�� �߻��� �� �ִ�.
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * moveMent2D.MoveSpeed)
            {
                NextMoveTo();
            }
            yield return null;
        }
    }
    private void NextMoveTo()
    {
        // ���� �̵��� wayPoints�� �����ִٸ�
        if (currentIndex < wayPoints.Length - 1)
        {
            // ���� ��ġ�� ��Ȯ�ϰ�  ��ǥ ��ġ�� ����(wayPoints)
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            moveMent2D.MoveTo(direction);
        }
        // ���� ��ġ�� ������ wayPoints�̸�
        else
        {
            // �� ������Ʈ ����
            OnDie();
        }
    }
    public void OnDie()
    {
        // EnemySpawner���� ����Ʈ�� �� ������ �����ϱ� ������ Destroy()�� �������� �ʰ�
        // EnemySpawner���� ������ ������ �� �ʿ��� ó���� �ϵ��� DestroyEnemy() �Լ� ȣ��
        enemySpawner.DestroyEnemy(this);
    }
}
    