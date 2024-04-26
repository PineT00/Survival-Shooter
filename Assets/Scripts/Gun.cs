using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready, // 발사 준비됨
        Empty, // 탄창이 빔
        Reloading // 재장전 중
    }

    public State state { get; private set; }

    public Transform fireTransform; // 총알이 발사될 위치
    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    private LineRenderer bulletLineRenderer; // 총알 궤적을 그리기 위한 렌더러
    private AudioSource gunAudioPlayer; // 총 소리 재생기
    public AudioClip shotClip;

    private float fireDistance = 50f;
    private float timeBetFire = 0.16f;
    private float damage = 20f;
    private float lastFireTime;


    private void Awake()
    {
        // 사용할 컴포넌트들의 참조를 가져오기
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
        // 라인 렌더러를 활성화하여 총알 궤적을 그린다

        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        muzzleFlashEffect.Play();

        gunAudioPlayer.PlayOneShot(shotClip);

        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);

        // 라인 렌더러를 비활성화하여 총알 궤적을 지운다
        bulletLineRenderer.enabled = false;
    }
}