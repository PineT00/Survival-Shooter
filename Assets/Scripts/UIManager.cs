using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    public TextMeshProUGUI scoreText; // ���� ǥ�ÿ� �ؽ�Ʈ
    public TextMeshProUGUI waveText; // �� ���̺� ǥ�ÿ� �ؽ�Ʈ
    public GameObject gameoverUI; // ���� ������ Ȱ��ȭ�� UI 
    public GameObject onDamageUI;


    // ���� �ؽ�Ʈ ����
    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score : " + newScore;
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
}
