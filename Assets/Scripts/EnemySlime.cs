using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : Enemy
{
    public float timeBetAttack = 1.5f; // ���� ����
    private float lastAttackTime; // ������ ���� ����
    //public bool attack = false;

    private void OnTriggerStay(Collider other)
    {
        if (!dead && (Time.time >= lastAttackTime + timeBetAttack))
        {
            var entity = other.GetComponent<PlayerMovement>();
            if (entity != null)
            {
                Debug.Log("������ ����!");
                enemyAnimator.SetTrigger("Attack");
                StartCoroutine(Sticky(entity, 1.5f));
                lastAttackTime = Time.time;
            }
        }
    }

    public IEnumerator Sticky(PlayerMovement target, float time)
    {
        Debug.Log("�ӵ�����!");
        pathFinder.isStopped = true;
        target.moveSpeed *= 0.5f;
        yield return new WaitForSeconds(time);
        target.moveSpeed *= 2f;
        pathFinder.isStopped = false;
    }

}
