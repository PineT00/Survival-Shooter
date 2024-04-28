using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolMgr : MonoBehaviour
{

    public static ObjectPoolMgr instance;

    public int defaultCapacity = 10;
    public int maxPoolSize = 15;
    public GameObject[] prefabs;

    public IObjectPool<GameObject> Pool { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        Init();
    }

    private void Init()
    {
        Pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
        OnDestroyPoolObject, true, defaultCapacity, maxPoolSize);

        // �̸� ������Ʈ ���� �س���
        for (int i = 0; i < defaultCapacity; i++)
        {
            Enemy enemy = CreatePooledItem().GetComponent<Enemy>();
            enemy.Pool.Release(enemy.gameObject);
        }
    }

    // ����
    private GameObject CreatePooledItem()
    {
        GameObject poolGo = Instantiate(prefabs[Random.Range(0, prefabs.Length)]);
        poolGo.GetComponent<Enemy>().Pool = this.Pool;
        return poolGo;
    }

    // ���
    private void OnTakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }

    // ��ȯ
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }

    // ����
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }
}