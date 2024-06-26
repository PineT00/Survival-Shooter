using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    public LayerMask whatIsTarget; // 추적 대상 레이어

    private LivingEntity targetEntity; // 추적할 대상
    private NavMeshAgent pathFinder; // 경로계산 AI 에이전트

    public ParticleSystem hitEffect; // 피격시 재생할 파티클 효과
    public AudioClip deathSound; // 사망시 재생할 소리
    public AudioClip hitSound; // 피격시 재생할 소리

    private Animator enemyAnimator; // 애니메이터 컴포넌트
    private AudioSource enemyAudioPlayer; // 오디오 소스 컴포넌트
    private Renderer enemyRenderer; // 렌더러 컴포넌트

    public float maxHealth = 100f;
    public float speed = 5f;
    public float damage = 20f; // 공격력
    public float timeBetAttack = 0.5f; // 공격 간격
    private float lastAttackTime; // 마지막 공격 시점



    // 추적할 대상이 존재하는지 알려주는 프로퍼티
    private bool hasTarget
    {
        get
        {
            // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }

            // 그렇지 않다면 false
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

    // 적 AI의 초기 스펙을 결정하는 셋업 메서드

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
        // 살아있는 동안 무한 루프
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

        // LivingEntity의 OnDamage()를 실행하여 데미지 적용
        base.OnDamage(damage, hitPoint, hitNormal);
        Debug.Log("좀비아파");
    }
    public override void Die()
    {
        base.Die();
        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        enemyAnimator.SetTrigger("Die");
        enemyAudioPlayer.PlayOneShot(deathSound);
    }

    public void StartSinking()
    {
        var cols = GetComponentsInChildren<Collider>();
        foreach (Collider col in cols)
        {
            col.enabled = false;
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
                Debug.Log(targetEntity.health);
                lastAttackTime = Time.time;
            }

        }

    }

}
