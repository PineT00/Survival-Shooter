using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    public LayerMask whatIsTarget; // ���� ��� ���̾�

    private LivingEntity targetEntity; // ������ ���
    private NavMeshAgent pathFinder; // ��ΰ�� AI ������Ʈ

    public ParticleSystem hitEffect; // �ǰݽ� ����� ��ƼŬ ȿ��
    public AudioClip deathSound; // ����� ����� �Ҹ�
    public AudioClip hitSound; // �ǰݽ� ����� �Ҹ�

    private Animator enemyAnimator; // �ִϸ����� ������Ʈ
    private AudioSource enemyAudioPlayer; // ����� �ҽ� ������Ʈ
    private Renderer enemyRenderer; // ������ ������Ʈ

    public float maxHealth = 100f;
    public float speed = 5f;
    public float damage = 20f; // ���ݷ�
    public float timeBetAttack = 0.5f; // ���� ����
    private float lastAttackTime; // ������ ���� ����



    // ������ ����� �����ϴ��� �˷��ִ� ������Ƽ
    private bool hasTarget
    {
        get
        {
            // ������ ����� �����ϰ�, ����� ������� �ʾҴٸ� true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }

            // �׷��� �ʴٸ� false
            return false;
        }
    }

    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        enemyAudioPlayer = GetComponent<AudioSource>();
        enemyRenderer = GetComponentInChildren<Renderer>();
    }

    // �� AI�� �ʱ� ������ �����ϴ� �¾� �޼���

    public void Setup()
    {
        Setup(maxHealth, damage, speed);
    }


    public void Setup(float newHealth, float newDamage, float newSpeed)
    {
        startingHealth = newHealth;
        damage = newDamage;
        pathFinder.speed = newSpeed;
    }

    private void Start()
    {
        StartCoroutine(UpdatePath());

    }

    private void Update()
    {
        enemyAnimator.SetBool("HasTarget", hasTarget);
    }


    private IEnumerator UpdatePath()
    {
        // ����ִ� ���� ���� ����
        while (!dead)
        {
            if (hasTarget)
            {
                pathFinder.SetDestination(targetEntity.gameObject.transform.position);
            }
            else
            {
                pathFinder.ResetPath();
                Collider[] cols = Physics.OverlapSphere(transform.position, 100f, whatIsTarget);
                foreach (Collider col in cols)
                {
                    LivingEntity livingEntity = col.GetComponent<LivingEntity>();
                    if (livingEntity != null)
                    {
                        targetEntity = livingEntity;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.25f);

        }
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        hitEffect.transform.position = hitPoint;
        hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
        hitEffect.Play();

        enemyAudioPlayer.PlayOneShot(hitSound);

        // LivingEntity�� OnDamage()�� �����Ͽ� ������ ����
        base.OnDamage(damage, hitPoint, hitNormal);
        Debug.Log("�������");
    }
    public override void Die()
    {
        base.Die();
        //pathFinder.isStopped = true;
        pathFinder.enabled = false;

        enemyAnimator.SetTrigger("Die");
        enemyAudioPlayer.PlayOneShot(deathSound);
    }

    public void Revive()
    {
        pathFinder.enabled = true;
        SetActiveCollider();
        Setup();
        dead = false;
        StartCoroutine(UpdatePath());
    }

    public void StartSinking()
    {
        var cols = GetComponentsInChildren<Collider>();
        foreach (Collider col in cols)
        {
            col.enabled = false;
        }
    }

    public void SetActiveCollider()
    {
        var cols = GetComponentsInChildren<Collider>();
        foreach (Collider col in cols)
        {
            col.enabled = true;
        }
    }

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
