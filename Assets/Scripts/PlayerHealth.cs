using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Slider healthSlider; 

    public AudioClip deathClip; 
    public AudioClip hitClip; 

    private AudioSource playerAudioPlayer;
    private Animator playerAnimator;

    private PlayerMovement playerMovement;
    private PlayerShooter playerShooter;

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

        ApplyUpdateHealth(300, false);
        playerMovement.enabled = true;
        playerShooter.enabled = true;

        healthSlider.gameObject.SetActive(true);
        healthSlider.value = health / startingHealth;

        UIManager.instance.SetActiveGameoverUI(false);
    }

    public override void RestoreHealth(float newHealth)
    {
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
        healthSlider.value = health / startingHealth;
        playerAudioPlayer.PlayOneShot(hitClip);

        StartCoroutine(OnDamageUI(0.07f));
    }
    public override void Die()
    {
        base.Die();
        healthSlider.gameObject.SetActive(false);
        playerAudioPlayer.PlayOneShot(deathClip);
        playerAnimator.SetTrigger("Die");

        playerMovement.enabled = false;
        playerShooter.enabled = false;

    }

    public void RestartLevel()
    {
        GameManager.instance.ReStart();
    }

    IEnumerator OnDamageUI(float time)
    {
        UIManager.instance.SetActiveOnDamageUI(true);
        yield return new WaitForSeconds(time);
        UIManager.instance.SetActiveOnDamageUI(false);

    }
}
