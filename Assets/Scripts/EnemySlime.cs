using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : Enemy
{
    public float timeBetAttack = 1.5f; // ���� ����
    private float lastAttackTime; // ������ ���� ����

    private void OnTriggerStay(Collider other)
    {
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            var entity = other.GetComponent<PlayerMovement>(); 
            if (entity != null)
            {
                StartCoroutine(Sticky(entity, 3f));
                lastAttackTime = Time.time;
            }

        }


    }
    private IEnumerator Sticky(PlayerMovement player, float time)
    {
        Debug.Log("�ӵ�����!");
        player.moveSpeed *= 0.5f;
        yield return new WaitForSeconds(time);
        player.moveSpeed *= 2f;
    }
}
