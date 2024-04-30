using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Enemy : LivingEntity
{
    public LayerMask whatIsTarget; // ���� ��� ���̾�

    public LivingEntity targetEntity; // ������ ���
    public NavMeshAgent pathFinder;

    public ParticleSystem hitEffect; 
    public AudioClip deathSound; 
    public AudioClip hitSound;

    public Animator enemyAnimator; 
    private AudioSource enemyAudioPlayer;
    private Renderer enemyRenderer;

    public float maxHealth = 100f;
    public float speed = 5f;
    public float damage = 20f; // ���ݷ�

    public IObjectPool<GameObject> Pool { get; set; }


    private bool hasTarget
    {
        get
        {
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }

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


}
