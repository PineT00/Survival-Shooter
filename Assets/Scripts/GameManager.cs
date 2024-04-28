using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    public int stage = 1;

    public bool paused = false;
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }

            return m_instance;
        }
    }

    private static GameManager m_instance; // 싱글톤이 할당될 static 변수

    private int score = 0;
    public bool isGameover { get; private set; }

    private void OnDie()
    {

    }


    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }



    private void Start()
    {
        //virtualCamera = FindObjectOfType<CinemachineVirtualCamera>(); 성능낭비! 그냥 인스펙터로 할당

        var pos = Random.insideUnitSphere * 10;

        if (NavMesh.SamplePosition(pos, out var hit, 10, NavMesh.AllAreas))
        {
            pos = hit.position;
        }
        else
        {
            pos = Vector3.zero;
        }

        //플레이어 캐릭터의 사망 이벤트 발생시 게임 오버
        FindObjectOfType<PlayerHealth>().onDeath += EndGame;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.instance.SetActivePauseMenu();
        }

        UIManager.instance.UpdateTimeText((int)Time.time);
    }

    public void AddScore(int newScore)
    {
        if (!isGameover)
        {
            score += newScore;

            UIManager.instance.UpdateScoreText(score);

        }
    }


    public void EndGame()
    {
        isGameover = true;

        UIManager.instance.SetActiveGameoverUI(true);
    }

    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
