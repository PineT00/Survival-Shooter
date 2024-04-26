using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy[] enemyPrefabs; // 생성할 적 AI

    public Enemy go;

    public Transform[] spawnPoints;

    public float spawnTimer = 3f;
    public float spawnTime = 0f;

    public float damageMax = 40f;
    public float damageMin = 20f;

    public float healthMax = 200f;
    public float healthMin = 100f;

    public float speedMax = 3f;
    public float speedMin = 1f;

    public Color strongEnemyColor = Color.red;

    private List<Enemy> enemies = new List<Enemy>();

    private int enemyCount;
    private int wave;


    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            go = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]);
            go.Setup();
            go.gameObject.SetActive(false); 
            enemies.Add(go);
        }


    }


    private void Update()
    {
        // 게임 오버 상태일때는 생성하지 않음
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
                    go.Setup();
                    go.SetActiveCollider();
                    enemies[num].transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                    enemyCount += 1;
                    break;
                }
            }
        }

        go.onDeath += () =>
        {
            Debug.Log("onDeath 호출!");
            enemyCount -= 1;
            StartCoroutine(CoDestroyAfter(go.gameObject, 3f));
            GameManager.instance.AddScore(10);
        };
    }

    private void CreateEnemy()
    {
        var go = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)],
            spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

        var enemy = go.GetComponent<Enemy>();

        enemy.Setup();

        //델리게이트 이벤트 (죽을때 발생할 일들 연결)
        enemy.onDeath += () =>
        {
            enemies.Remove(enemy);
            enemyCount = enemies.Count;
            StartCoroutine(CoDestroyAfter(go.gameObject, 3f));
            GameManager.instance.AddScore(10);

        };

        enemies.Add(enemy);
        enemyCount = enemies.Count;
        var live = enemy.GetComponent<LivingEntity>();

    }

    IEnumerator CoDestroyAfter(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        go.SetActive(false);

        //Destroy(go);
    }

}
