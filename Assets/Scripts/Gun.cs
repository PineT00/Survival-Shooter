using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready, // �߻� �غ��
        Empty, // źâ�� ��
        Reloading // ������ ��
    }

    public State state { get; private set; }

    public Transform fireTransform; // �Ѿ��� �߻�� ��ġ
    public ParticleSystem muzzleFlashEffect; // �ѱ� ȭ�� ȿ��
    private LineRenderer bulletLineRenderer; // �Ѿ� ������ �׸��� ���� ������
    private AudioSource gunAudioPlayer; // �� �Ҹ� �����
    public AudioClip shotClip;

    private float fireDistance = 50f;
    private float timeBetFire = 0.16f;
    private float damage = 20f;
    private float lastFireTime;


    private void Awake()
    {
        // ����� ������Ʈ���� ������ ��������
        bulletLineRenderer = GetComponent<LineRenderer>();
        gunAudioPlayer = GetComponent<AudioSource>();

        bulletLineRenderer.enabled = false;
        bulletLineRenderer.positionCount = 2;
    }

    private void OnEnable()
    {
        lastFireTime = 0;
        state = State.Ready;

    }

    public void Fire()
    {
        if (Time.time > lastFireTime + timeBetFire)
        {
            lastFireTime = Time.time;
            Shot();
        }
    }

    private void Shot()
    {
        var hitPoint = Vector3.zero;
        var ray = new Ray(fireTransform.position, fireTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, fireDistance))
        {
            hitPoint = hitInfo.point;

            var damagable = hitInfo.collider.GetComponent<IDamageable>();
            if (damagable != null)
            {
                damagable.OnDamage(damage, hitPoint, hitInfo.normal);
            }


        }
        else
        {
            hitPoint = fireTransform.position + fireTransform.forward * fireDistance;
        }

        StartCoroutine(ShotEffect(fireTransform.position + fireTransform.forward * 20f));
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        // ���� �������� Ȱ��ȭ�Ͽ� �Ѿ� ������ �׸���

        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        muzzleFlashEffect.Play();

        gunAudioPlayer.PlayOneShot(shotClip);

        // 0.03�� ���� ��� ó���� ���
        yield return new WaitForSeconds(0.03f);

        // ���� �������� ��Ȱ��ȭ�Ͽ� �Ѿ� ������ �����
        bulletLineRenderer.enabled = false;
    }
}