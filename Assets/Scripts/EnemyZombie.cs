using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyZombie : Enemy
{
    public float timeBetAttack = 0.5f; // ���� ����
    private float lastAttackTime; // ������ ���� ����



    private void OnTriggerStay(Collider other)
    {
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            var entity = other.GetComponent<LivingEntity>();
            if (entity != null && entity == targetEntity)
            {
                var pos = transform.position;
                pos.y = 1f;
                var hitPoint = other.ClosestPoint(pos);
                var hitNormal = transform.position - other.transform.position;

                targetEntity.OnDamage(damage, hitPoint, hitNormal.normalized);
                lastAttackTime = Time.time;
            }

        }

    }
}
