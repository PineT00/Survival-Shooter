using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Slider healthSlider; // 체력을 표시할 UI 슬라이더

    public AudioClip deathClip; // 사망 소리
    public AudioClip hitClip; // 피격 소리

    private AudioSource playerAudioPlayer; // 플레이어 소리 재생기
    private Animator playerAnimator; // 플레이어의 애니메이터

    private PlayerMovement playerMovement; // 플레이어 움직임 컴포넌트
    private PlayerShooter playerShooter; // 플레이어 슈터 컴포넌트

    //public event System.Action OnRespawn;

    private void Awake()
    {
        // 사용할 컴포넌트를 가져오기
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
        playerAudioPlayer = GetComponent<AudioSource>();

    }

    protected override void OnEnable()
    {
        base.OnEnable();

        ApplyUpdateHealth(200, false);

        //healthSlider.gameObject.SetActive(true);
        //healthSlider.value = health / startingHealth;

        playerMovement.enabled = true;
        playerShooter.enabled = true;

        //UIManager.instance.SetActiveGameoverUI(false);
    }

    public override void RestoreHealth(float newHealth)
    {
        // LivingEntity의 RestoreHealth() 실행 (체력 증가)
        base.RestoreHealth(newHealth);
        healthSlider.value = health / startingHealth;
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (dead)
        {
            return;
        }
        base.OnDamage(damage, hitPoint, hitDirection);
        //healthSlider.value = health / startingHealth;
        playerAudioPlayer.PlayOneShot(hitClip);

        Debug.Log("플레이어 아프다");
    }
    public override void Die()
    {
        base.Die();
        //healthSlider.gameObject.SetActive(false);
        playerAudioPlayer.PlayOneShot(deathClip);
        playerAnimator.SetTrigger("Die");

        playerMovement.enabled = false;
        playerShooter.enabled = false;


        //Invoke("Respawn", 5f); //일정시간후 해당함수 호출
    }
}
