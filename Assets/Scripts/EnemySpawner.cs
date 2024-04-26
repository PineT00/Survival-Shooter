using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy[] enemyPrefabs; // ������ �� AI
    public Transform[] spawnPoints;

    public float spawnTimer = 3f;
    public float spawnTime = 0f;

    public List<Enemy> enemies = new List<Enemy>();

    private int enemyCount;


    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            var go = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]);
            var enemy = go.GetComponent<Enemy>();
            enemy.Setup();
            enemy.onDeath += () =>
            {
                enemyCount--;
                StartCoroutine(CoDestroyAfter(enemy, 3f));
                GameManager.instance.AddScore(10);
            };
            enemy.gameObject.SetActive(false);
            enemies.Add(enemy);
        }
    }


    private void Update()
    {
        // ���� ���� �����϶��� �������� ����
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            return;
        }

        if(spawnTime > spawnTimer)
        {
            SpawnEnemy();
            spawnTime = 0f;
        }
        else
        {
            spawnTime += Time.deltaTime;
        }

        UpdateUI();
    }


    private void UpdateUI()
    {
        //Debug.Log(enemyCount);
    }


    private void SpawnEnemy()
    {
        if(enemyCount < 10)
        {
            for (int i = 0; i < enemies.Count; ++i)
            {
                int num = Random.Range(0, enemies.Count);
                if (!enemies[num].gameObject.activeSelf)
                {
                    enemies[num].gameObject.SetActive(true);
                    enemies[num].Revive();

                    enemies[num].transform.position = Vector3.zero;

                    //enemies[num].transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                    enemyCount += 1;
                    break;
                }
            }
        }


    }

    private void CreateEnemy()
    {
        var go = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)],
            spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

        var enemy = go.GetComponent<Enemy>();

        enemy.Setup();


        enemy.onDeath += () =>
        {
            enemies.Remove(enemy);
            enemyCount = enemies.Count;
           // StartCoroutine(CoDestroyAfter(go.gameObject, 3f));
            GameManager.instance.AddScore(10);

        };

        enemies.Add(enemy);
        enemyCount = enemies.Count;
        var live = enemy.GetComponent<LivingEntity>();

    }

    IEnumerator CoDestroyAfter(Enemy go, float time)
    {
        yield return new WaitForSeconds(time);
        go.gameObject.SetActive(false);

        //Destroy(go);
    }

}
