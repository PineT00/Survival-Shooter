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

    private static GameManager m_instance; // �̱����� �Ҵ�� static ����

    private int score = 0;
    public bool isGameover { get; private set; }

    private void OnDie()
    {

    }


    private void Awake()
    {
        // ���� �̱��� ������Ʈ�� �� �ٸ� GameManager ������Ʈ�� �ִٸ�
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }



    private void Start()
    {
        //virtualCamera = FindObjectOfType<CinemachineVirtualCamera>(); ���ɳ���! �׳� �ν����ͷ� �Ҵ�

        var pos = Random.insideUnitSphere * 10;

        if (NavMesh.SamplePosition(pos, out var hit, 10, NavMesh.AllAreas))
        {
            pos = hit.position;
        }
        else
        {
            pos = Vector3.zero;
        }

        //�÷��̾� ĳ������ ��� �̺�Ʈ �߻��� ���� ����
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
