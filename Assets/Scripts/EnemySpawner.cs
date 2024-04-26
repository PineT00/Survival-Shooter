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





    private void Update()
    {

        // 게임 오버 상태일때는 생성하지 않음
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            return;
        }

        if(spawnTime > spawnTimer)
        {
            CreateEnemy();
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
        // 현재 웨이브와 남은 적의 수 표시
        //UIManager.instance.UpdateWaveText(wave, enemyCount);
    }

    // 현재 웨이브에 맞춰 적을 생성
    private void SpawnWave()
    {

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
        //go.SetActive(false);

        Destroy(go);
    }

}
