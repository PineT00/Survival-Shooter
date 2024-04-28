using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int currStage;

    public Transform[] spawnPoints;

    public float spawnTimer = 3f;
    public float spawnTime = 0f;

    public List<Enemy> enemies = new List<Enemy>();

    public int enemyCount;



    private void Start()
    {
        for (int i = 0; i < 10; ++i)
        {
            var enemyGo = ObjectPoolMgr.instance.Pool.Get();

            var enemy = enemyGo.GetComponent<Enemy>();
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

        //UpdateUI();
    }


    private void UpdateUI()
    {

    }

    private void SetPool(int stage)
    {
        foreach(var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        enemies.Clear();
        enemyCount = 0;
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
                    enemies[num].transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                    //enemies[num].transform.position = Vector3.zero;
                    enemies[num].Revive();
                    enemyCount += 1;
                    break;
                }
            }
        }


    }

    private void CreateEnemy()
    {
        //var go = Instantiate(enemyPool1[Random.Range(0, enemyPool1.Length)],
        //    spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

        //var enemy = go.GetComponent<Enemy>();

        //enemy.Setup();


        //enemy.onDeath += () =>
        //{
        //    enemies.Remove(enemy);
        //    enemyCount = enemies.Count;
        //   // StartCoroutine(CoDestroyAfter(go.gameObject, 3f));
        //    GameManager.instance.AddScore(10);

        //};

        //enemies.Add(enemy);
        //enemyCount = enemies.Count;
        //var live = enemy.GetComponent<LivingEntity>();

    }

    IEnumerator CoDestroyAfter(Enemy go, float time)
    {
        if(go.gameObject.activeSelf)
        {
            yield return new WaitForSeconds(time);
            go.gameObject.SetActive(false);
        }


        //Destroy(go);
    }

}
