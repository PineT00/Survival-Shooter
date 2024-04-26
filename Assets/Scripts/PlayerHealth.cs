using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Slider healthSlider; // ü���� ǥ���� UI �����̴�

    public AudioClip deathClip; // ��� �Ҹ�
    public AudioClip hitClip; // �ǰ� �Ҹ�

    private AudioSource playerAudioPlayer; // �÷��̾� �Ҹ� �����
    private Animator playerAnimator; // �÷��̾��� �ִϸ�����

    private PlayerMovement playerMovement; // �÷��̾� ������ ������Ʈ
    private PlayerShooter playerShooter; // �÷��̾� ���� ������Ʈ

    private void Awake()
    {
        // ����� ������Ʈ�� ��������
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

        //healthSlider.gameObject.SetActive(true);
        //healthSlider.value = health / startingHealth;

        UIManager.instance.SetActiveGameoverUI(false);
    }

    public override void RestoreHealth(float newHealth)
    {
        // LivingEntity�� RestoreHealth() ���� (ü�� ����)
        base.RestoreHealth(newHealth);
        //healthSlider.value = health / startingHealth;
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

        Debug.Log("�÷��̾� ������");

        StartCoroutine(OnDamageUI(0.07f));
    }
    public override void Die()
    {
        base.Die();
        //healthSlider.gameObject.SetActive(false);
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
