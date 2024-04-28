using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }

    private static UIManager m_instance; // �̱����� �Ҵ�� ����

    public TextMeshProUGUI timeText; // ���� ǥ�ÿ� �ؽ�Ʈ
    public TextMeshProUGUI scoreText; // ���� ǥ�ÿ� �ؽ�Ʈ
    public TextMeshProUGUI waveText; // �� ���̺� ǥ�ÿ� �ؽ�Ʈ
    public GameObject gameoverUI; // ���� ������ Ȱ��ȭ�� UI 
    public GameObject onDamageUI;
    public GameObject pauseMenu;

    public Slider volumeSlider;
    public AudioSource audioSource;

    private bool paused = false;

    // ���� �ؽ�Ʈ ����
    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score : " + newScore;
    }

    public void UpdateTimeText(int newTime)
    {
        timeText.text = "Time : " + newTime;
    }

    // �� ���̺� �ؽ�Ʈ ����
    public void UpdateWaveText(int waves, int count)
    {
        //waveText.text = "Wave : " + waves + "\nEnemy Left : " + count;
    }

    // ���� ���� UI Ȱ��ȭ
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    // ���� �����
    public void GameRestart()
    {

    }

    public void SetActiveOnDamageUI(bool active)
    {
        onDamageUI.SetActive(active);
    }

    public void UpdateVolume()
    {
        // �����̴��� ������ ����� �ҽ��� ���� ������Ʈ
        audioSource.volume = volumeSlider.value;
    }

    public void SetActivePauseMenu()
    {
        paused = !paused;
        pauseMenu.SetActive(paused);
        if (paused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }    
    }

    public void QuitGame()
    {
        Application.Quit(); // ���ø����̼� ����
    }
}
