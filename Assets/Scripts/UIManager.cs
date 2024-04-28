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

    private static UIManager m_instance; // 싱글톤이 할당될 변수

    public TextMeshProUGUI timeText; // 점수 표시용 텍스트
    public TextMeshProUGUI scoreText; // 점수 표시용 텍스트
    public TextMeshProUGUI waveText; // 적 웨이브 표시용 텍스트
    public GameObject gameoverUI; // 게임 오버시 활성화할 UI 
    public GameObject onDamageUI;
    public GameObject pauseMenu;

    public Slider volumeSlider;
    public AudioSource audioSource;

    private bool paused = false;

    // 점수 텍스트 갱신
    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score : " + newScore;
    }

    public void UpdateTimeText(int newTime)
    {
        timeText.text = "Time : " + newTime;
    }

    // 적 웨이브 텍스트 갱신
    public void UpdateWaveText(int waves, int count)
    {
        //waveText.text = "Wave : " + waves + "\nEnemy Left : " + count;
    }

    // 게임 오버 UI 활성화
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    // 게임 재시작
    public void GameRestart()
    {

    }

    public void SetActiveOnDamageUI(bool active)
    {
        onDamageUI.SetActive(active);
    }

    public void UpdateVolume()
    {
        // 슬라이더의 값으로 오디오 소스의 볼륨 업데이트
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
        Application.Quit(); // 어플리케이션 종료
    }
}
